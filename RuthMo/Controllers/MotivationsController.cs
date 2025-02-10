using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RuthMo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MotivationsController : ControllerBase
{
    [HttpGet]
    public string RandomMotivation()
    {
        return "OK";
    }
}