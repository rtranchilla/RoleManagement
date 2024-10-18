using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RoleManager.WebService.Controllers;

[ApiController]
[Route("[controller]")]
public class SenderControllerBase : ControllerBase
{
    protected readonly ISender sender;

    public SenderControllerBase(ISender sender) => this.sender = sender;

    protected async Task<ActionResult<TResult>> SendQuery<TResult>(IRequest<TResult> request)
    {
        try
        {
            var result = await sender.Send(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
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
            await sender.Send(request);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    protected Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) => sender.Send(request, cancellationToken);
    protected Task Send(IRequest request, CancellationToken cancellationToken = default) => sender.Send(request, cancellationToken);
}
