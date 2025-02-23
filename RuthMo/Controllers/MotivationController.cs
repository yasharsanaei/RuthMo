using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuthMo.Data;
using RuthMo.Models;

namespace RuthMo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotivationController(MotivationContext context) : ControllerBase
    {
        // [Authorize(Roles = "Admin")]
        [HttpGet("debug-user")]
        public ActionResult GetDebugUser()
        {
            var user = User.Identity?.Name;
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            Console.WriteLine(
                $"🔍 Debug User: {user}, Authenticated: {isAuthenticated}, Roles: {string.Join(", ", roles)}");

            return Ok(new { user, isAuthenticated, roles });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Motivation>>> GetAll()
        {
            // var user = User.Identity?.Name;
            // var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            //
            // Console.WriteLine($"User: {user}, Authenticated: {User.Identity?.IsAuthenticated}, Roles: {string.Join(", ", roles)}");
            //
            // if (!User.Identity?.IsAuthenticated ?? false)
            // {
            //     return Unauthorized("User is not authenticated.");
            // }
            //
            // if (!roles.Contains("Admin"))
            // {
            //     return Forbid("User is not in the Admin role.");
            // }

            var motivations = await context.Motivation.ToListAsync();
            return Ok(motivations);
        }

        [HttpGet("Random")]
        public async Task<ActionResult<Motivation>> GetRandom()
        {
            var count = context.Motivation.Count();
            if (count == 0) return NotFound();

            var random = new Random().Next(count);
            var motivation = await context.Motivation.Skip(random).FirstOrDefaultAsync();
            return Ok(motivation);
        }


        [HttpGet("success")]
        public ActionResult<string> Success()
        {
            return Ok("This is aweseme!");
        }
    }
}