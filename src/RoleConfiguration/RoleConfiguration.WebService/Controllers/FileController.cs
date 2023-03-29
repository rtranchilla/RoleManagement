using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleConfiguration.Commands;
using RoleConfiguration.Dto;

namespace RoleConfiguration.WebService.Controllers;

public sealed class FileController : SenderControllerBase
{
    public FileController(ISender sender) : base(sender) { }

    [HttpDelete("{source}/{path}")]
    public async Task<IActionResult> Delete(string source, string path) =>
        await SendCommand(new FileDelete(source, path));
}