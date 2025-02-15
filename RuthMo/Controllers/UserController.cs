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