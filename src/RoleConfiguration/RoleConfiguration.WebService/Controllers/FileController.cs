using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleConfiguration.Commands;
using RoleConfiguration.Dto;

namespace RoleConfiguration.WebService.Controllers;

public sealed class FileController : SenderControllerBase
{
    public FileController(ISender sender) : base(sender) { }

    [HttpPut("RoleTree/{source}")]
    public async Task UpdateRoleTree(string source, ContentUpdate content) => 
        await SendCommand(new RoleTreeFileUpdate(source, content.Path, content.Content));

    [HttpPut("Member/{source}")]
    public async Task UpdateMember(string source, ContentUpdate content) =>
        await SendCommand(new MemberFileUpdate(source, content.Path, content.Content));

    [HttpDelete("{source}/{path}")]
    public async Task Delete(string source, string path) =>
        await SendCommand(new FileDelete(source, path));
}