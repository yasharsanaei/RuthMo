using Microsoft.AspNetCore.Mvc;
using RuthMo.Data;

namespace RuthMo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotivationController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok("Hello!");
        }
    }
}