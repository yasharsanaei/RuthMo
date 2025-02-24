using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill required fields.");
            }

            var isUserExist = await userManager.FindByEmailAsync(registerDto.Email);

            if (isUserExist != null)
            {
                return BadRequest("This email address exist on our servers!");
            }

            RuthMoUser ruthMoUser = new RuthMoUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(ruthMoUser, registerDto.Password);

            return result.Succeeded ? Ok(result) : BadRequest("Can not create the user :(");
        }
    }
}