using JS17.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace JS17.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PingController : BaseController
{
    public PingController(WebDbContext dbContext) : base(dbContext) { }

    [HttpGet]
    public IActionResult PingServer()
    {
        return Ok("pong");
    }

    [HttpGet]
    public IActionResult PingSqlServer()
    {
        var success = _dbContext.Database.CanConnect();
        if (success)
        {
            return Ok("pong");
        }
        else
        {
            return BadRequest("cannot connect");
        }
    }
}
