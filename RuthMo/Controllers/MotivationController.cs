using System.Security.Claims;
using Mapster;
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
        public async Task<ActionResult<List<MotivationDto>>> GetAll()
        {
            var motivations =
                await appDbContext.Motivations
                    .Include(m => m.User)
                    .ToListAsync();
            return Ok(motivations.Adapt<MotivationDto[]>());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<MotivationDto>> Create([FromBody] CreateMotivationDto dto)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId.IsNullOrEmpty())
            {
                return Unauthorized("User Id not found!");
            }


            var motivation = new Motivation
            {
                Content = dto.Content,
                UserId = userId!,
                Status = user.IsInRole(RuthMoUserRoles.Admin.ToString())
                    ? MotivationStatus.Accept
                    : MotivationStatus.Waiting
            };
            var createdMotivation = await appDbContext.Motivations.AddAsync(motivation);
            await appDbContext.SaveChangesAsync();
            return Ok(createdMotivation.Entity.Adapt<MotivationDto>());
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<MotivationDto>> Update([FromBody] UpdateMotivationDto updateMotivation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide valid data");
            }

            var user = HttpContext.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId.IsNullOrEmpty())
            {
                return Unauthorized("User Id not found!");
            }

            var motivation = await appDbContext.Motivations.FindAsync(updateMotivation.Id);
            if (motivation == null)
            {
                return NotFound("Motivation not found!");
            }

            motivation.Content = updateMotivation.Content;
            appDbContext.Motivations.Update(motivation);
            await appDbContext.SaveChangesAsync();

            return Ok(motivation.Adapt<MotivationDto>());
        }
    }
}