using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RuthMo.Dtos;
using RuthMo.Models;

namespace RuthMo.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, roles);

        // Response.Cookies.Append("AuthToken", token, new CookieOptions
        // {
        //     HttpOnly = true,
        //     Secure = true,
        //     SameSite = SameSiteMode.Strict
        // });

        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,
            // Secure = true,  // Ensure it's secure in production
            SameSite = SameSiteMode.Lax, // Adjust based on frontend/backend setup
            Expires = DateTime.UtcNow.AddMinutes(60)
        });

        return Ok(new { message = "Login successful" });
    }

    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return Ok(new { message = "Logged out" });
    }

    private string GenerateJwtToken(User user, IList<string> roles)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}