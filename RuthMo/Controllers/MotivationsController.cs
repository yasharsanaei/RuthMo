using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using RuthMo.Models;

namespace RuthMo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MotivationsController : ControllerBase
{
    private readonly MotivationContext _motivationContext;

    public MotivationsController(MotivationContext context)
    {
        _motivationContext = context;
        _motivationContext.Database.EnsureCreated();
    }

    [HttpGet]
    public async Task<ActionResult<Motivation>> GetAllMotivations()
    {
        var motivations = await _motivationContext.Motivations.ToListAsync();
        return Ok(motivations);
    }

    [HttpGet("{id}")]
    public ActionResult<Motivation> GetMotivationById(int id)
    {
        var motivation = _motivationContext.Motivations.Find(id);
        if (motivation == null)
        {
            return NotFound();
        }

        return Ok(motivation);
    }
}