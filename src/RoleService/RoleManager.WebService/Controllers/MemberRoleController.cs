using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoleManager.Commands;

namespace RoleManager.Web.Controllers
{
    public sealed class MemberRoleController : MediatorControllerBase
    {
        public MemberRoleController(IMediator mediator) : base(mediator) { }

        [HttpPut]
        public Task<IActionResult> Update(Guid memberId, Guid treeId, Guid roleId) => SendCommand(new MemberRoleUpdate(memberId, treeId, roleId));

        [HttpDelete]
        public Task<IActionResult> Delete(Guid memberId, Guid treeId) => SendCommand(new MemberRoleDelete(memberId, treeId));
    }
}
