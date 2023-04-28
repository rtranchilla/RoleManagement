using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleConfiguration.Commands;
using RoleConfiguration.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace RoleConfiguration.WebService.Controllers;

public sealed class FileController : SenderControllerBase
{
    public FileController(ISender sender) : base(sender) { }

    [HttpDelete("{source}/{path}")]
    [SwaggerOperation(Summary = "Delete a yaml file.",
        Description = "Deletes a yaml file for a specified source.")]
    public async Task<IActionResult> Delete(string source, string path) =>
        await SendCommand(new FileDelete(source, path));
}