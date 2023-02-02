using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;

namespace RoleManager.WebService.Controllers;

public sealed class MemberRoleController : SenderControllerBase
{
    public MemberRoleController(ISender sender) : base(sender) { }

    [HttpPut("{memberId}/{roleId}/{treeId}")]
    public Task<IActionResult> Update(Guid memberId, Guid roleId, Guid treeId) => SendCommand(new MemberRoleUpdate(memberId, treeId, roleId));

    [HttpDelete("{memberId}/{treeId}")]
    public Task<IActionResult> Delete(Guid memberId, Guid treeId) => SendCommand(new MemberRoleDelete(memberId, treeId));
}
