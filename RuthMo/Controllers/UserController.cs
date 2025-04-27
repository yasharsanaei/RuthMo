using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuthMo.Data;
using RuthMo.Dtos;
using RuthMo.Extensions;
using RuthMo.Models;

namespace RuthMo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        UserManager<RuthMoUser> userManager,
        AppDbContext context) : ControllerBase
    {
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Invalid token");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roles = await userManager.GetRolesAsync(user);

            return Ok(new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList(),
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            });
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult<UserDto>> Update(UserUpdateDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide valid data");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != user.Id)
            {
                return Unauthorized("You can't change someone else's profile data!");
            }

            var userFullData = await userManager.FindByIdAsync(userId);
            if (userFullData == null)
            {
                return NotFound("User not found!");
            }

            userFullData.FirstName = user.FirstName;
            userFullData.LastName = user.LastName;
            userFullData.UserName = user.UserName;
            userFullData.Email = user.Email;

            var result = await userManager.UpdateAsync(userFullData);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(userFullData.Adapt<UserDto>());
        }

        [Authorize]
        [HttpPut("")]
        public async Task<ActionResult<UserDto>> UpdateUser(UserUpdateDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide valid data");
            }

            var userFullData = await userManager.FindByIdAsync(user.Id);
            if (userFullData == null)
            {
                return NotFound("User not found!");
            }

            userFullData.FirstName = user.FirstName;
            userFullData.LastName = user.LastName;
            userFullData.UserName = user.UserName;
            userFullData.Email = user.Email;

            var result = await userManager.UpdateAsync(userFullData);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(userFullData.Adapt<UserDto>());
        }

        [Authorize]
        [HttpGet("admins")]
        public async Task<ActionResult<UserDto[]>> GetAllAdmins()
        {
            var users = await userManager.GetUsersInRoleAsync("Admin");
            return Ok(users.Adapt<List<UserDto>>());
        }

        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<PagedResult<UserDto>>> GetAllUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null)
        {
            var query = userManager.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(u =>
                    EF.Functions.Like(u.FirstName.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(u.LastName.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(u.Email!.ToLower(), $"%{search}%") ||
                    EF.Functions.Like(u.UserName!.ToLower(), $"%{search}%"));
            }

            var pagedUser = await query.OrderBy(u => u.Id)
                .AsNoTracking().ToPagedResultAsync(page, pageSize);

            return Ok(pagedUser.Adapt<PagedResult<UserDto>>());
        }
    }
}