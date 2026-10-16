using Microsoft.AspNetCore.Mvc;

namespace TeamMeetingRemoteSupport.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "ok" });
    }
}
