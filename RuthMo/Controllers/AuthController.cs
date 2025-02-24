using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        IConfiguration configuration)
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
                    var token = await GenerateJwtTokenAsync(user);
                    return Ok(token);
                }
            }

            return Unauthorized();
        }

        private async Task<AuthDto> GenerateJwtTokenAsync(RuthMoUser user)
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

            return new AuthDto
            {
                Token = jwtToken,
                ExpiresAt = token.ValidTo
            };
        }
    }
}