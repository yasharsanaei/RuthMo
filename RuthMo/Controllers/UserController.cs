using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RuthMo.Data;
using RuthMo.Dtos;
using RuthMo.Models;

namespace RuthMo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserManager<RuthMoUser> userManager, AppDbContext context) : ControllerBase
    {
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<RuthMoUser>> Me()
        {
            // Get the user ID from the token claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Invalid token");
            }

            // Fetch the user from the database
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(new RuthMoUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Role = RuthMoUserRoles.Admin
            });
        }
    }
}