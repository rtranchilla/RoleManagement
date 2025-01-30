using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Events;

namespace RoleManagerSubscriberExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    [Dapr.Topic("pubsub", "members")]
    [HttpPost("created")]
    public IActionResult MemberCreated(MemberUpdated created)
    {
        return Ok(created.ToString());
    }
}
