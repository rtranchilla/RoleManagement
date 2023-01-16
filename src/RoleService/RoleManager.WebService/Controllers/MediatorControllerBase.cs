using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RoleManagement.RoleManagementService.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MediatorControllerBase : ControllerBase
{
    protected readonly IMediator mediator;

    public MediatorControllerBase(IMediator mediator) => this.mediator = mediator;

    protected async Task<ActionResult<TResult>> SendQuery<TResult>(IRequest<TResult> request)
    {
        try
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    protected async Task<IActionResult> SendCommand(IRequest request)
    {
        try
        {
            await mediator.Send(request);
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
