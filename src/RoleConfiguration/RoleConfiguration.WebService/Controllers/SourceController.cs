using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleConfiguration.Commands;

namespace RoleConfiguration.WebService.Controllers;

public sealed class SourceController : SenderControllerBase
{
    public SourceController(ISender sender) : base(sender) { }

    [HttpPost("{source}")]
    public async Task Update(string source) =>
        await SendCommand(new SourceCreate(source));

    [HttpDelete("{source}")]
    public async Task Delete(string source) =>
        await SendCommand(new SourceDelete(source));
}
