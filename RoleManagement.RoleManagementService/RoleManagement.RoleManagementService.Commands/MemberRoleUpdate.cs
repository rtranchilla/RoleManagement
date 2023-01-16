using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoleManagement.RoleManagementService.DataPersistence;

namespace RoleManagement.RoleManagementService.Commands
{
    public sealed record MemberRoleUpdate(Guid MemberId, Guid TreeId, Guid RoleId) : AggregateRootCommon;
    public sealed class MemberRoleUpdateHandler : IRequestHandler<MemberRoleUpdate>
    {
        private readonly RoleDbContext dbContext;

        public MemberRoleUpdateHandler(RoleDbContext dbContext) => this.dbContext = dbContext;

        public async Task<Unit> Handle(MemberRoleUpdate request, CancellationToken cancellationToken)
        {
            var member = dbContext.Members!.Include(e => e.Roles).FirstOrDefault(e => e.Id == request.MemberId);
            if (member == null)
                throw new NullReferenceException("Failed to locate member with specified id.");

            var entity = member.Roles.FirstOrDefault(e => e.TreeId == request.TreeId);
            if (entity == null)
            {
                entity = new MemberRole(request.MemberId, request.TreeId, request.RoleId);
                await dbContext.AddAsync(entity, cancellationToken);
            }
            else
                entity.RoleId = request.RoleId;

            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
