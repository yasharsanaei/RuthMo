using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using RuthMo.Data;
using RuthMo.Dtos;
using RuthMo.Models;

namespace RuthMo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        UserManager<RuthMoUser> userManager,
        RoleManager<IdentityRole> roleManager,
        AppDbContext context,
        IConfiguration configuration,
        TokenValidationParameters tokenValidationParameters)
        : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest("Please fill required fields.");

            var isUserEmailExist = await userManager.FindByEmailAsync(registerDto.Email);

            if (isUserEmailExist != null) return BadRequest("This email address exist on our servers!");

            var isUserUsernameExist = await userManager.FindByNameAsync(registerDto.Username);

            if (isUserUsernameExist != null) return BadRequest("This username address exist on our servers!");

            RuthMoUser ruthMoUser = new RuthMoUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(ruthMoUser, registerDto.Password);

            return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest("Fill required fields");
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                var isPasswordCorrect = await userManager.CheckPasswordAsync(user, loginDto.Password);
                if (isPasswordCorrect)
                {
                    var token = await GenerateJwtTokenAsync(user, null);
                    return Ok(token);
                }
            }

            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("PLease Send all required fields");
            }

            var result = await VerifyAndGenerateTokenAsync(refreshTokenDto);

            return Ok(result);
        }

        private async Task<AuthDto?> VerifyAndGenerateTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var storedToken =
                await context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshTokenDto.RefreshToken);

            if (storedToken == null) return null;

            var dbUser = await userManager.FindByIdAsync(storedToken.UserId);

            if (dbUser == null) return null;

            try
            {
                var tokenCheckResult = jwtSecurityTokenHandler.ValidateToken(refreshTokenDto.Token,
                    tokenValidationParameters, out var validatedToken);
                return await GenerateJwtTokenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException e)
            {
                if (storedToken.DateExpire >= DateTime.UtcNow) return await GenerateJwtTokenAsync(dbUser, storedToken);

                return await GenerateJwtTokenAsync(dbUser, null);
            }
        }

        private async Task<AuthDto> GenerateJwtTokenAsync(RuthMoUser user, RefreshToken? rToken)
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Sub, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var authSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"]!));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"]!,
                audience: configuration["JWT:Audience"]!,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                expires: DateTime.UtcNow.AddMinutes(10)
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if (rToken != null)
            {
                return new AuthDto
                {
                    Token = jwtToken,
                    RefreshToken = rToken.Token,
                    ExpiresAt = token.ValidTo
                };
            }

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = $"{Guid.NewGuid().ToString()}-{Guid.NewGuid().ToString()}"
            };

            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();

            return new AuthDto
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };
        }
    }
}