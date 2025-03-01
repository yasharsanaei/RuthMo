using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RuthMo.Data;
using RuthMo.Dtos;
using RuthMo.Models;

namespace RuthMo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotivationController(AppDbContext appDbContext, UserManager<RuthMoUser> userManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Motivation>>> GetAll()
        {
            var motivations = await appDbContext.Motivations.ToListAsync();
            return Ok(motivations);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Motivation>> Post([FromBody] MotivationDto motivationDto)
        {
            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var user = HttpContext.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId.IsNullOrEmpty())
            {
                return Unauthorized("User Id not found!");
            }

            var motivation = new Motivation
            {
                Content = motivationDto.Content,
                UserId = userId!
            };
            var createdMotivation = await appDbContext.Motivations.AddAsync(motivation);
            await appDbContext.SaveChangesAsync();
            return Ok(createdMotivation.Entity);
        }
    }
}