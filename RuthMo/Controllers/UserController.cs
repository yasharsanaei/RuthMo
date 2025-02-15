using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RuthMo.Models;

namespace RuthMo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(MotivationContext motivationContext, UserManager<User> userManager)
        : ControllerBase
    {
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var roles = await userManager.GetRolesAsync(user);

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                nickname = user.NickName,
                gender = user.Gender,
                roles
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await motivationContext.Users.FindAsync();
            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> Put(string id, [FromBody] User user)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            switch (userRole)
            {
                case "Admin":
                    var createdUser = await userManager.UpdateAsync(user);
                    return Ok(createdUser);
                case "User":
                    if (id != userId)
                        return Unauthorized("You can't change someone else's profile!");
                    break;
            }

            return BadRequest("We can't understand the request");
        }
    }
}