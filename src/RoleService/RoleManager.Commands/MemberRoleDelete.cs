using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;

namespace RoleManager.Commands
{
    public sealed record MemberRoleDelete(Guid MemberId, Guid TreeId) : AggregateRootCommon;
    public sealed class MemberRoleDeleteHandler : IRequestHandler<MemberRoleDelete>
    {
        private readonly RoleDbContext dbContext;

        public MemberRoleDeleteHandler(RoleDbContext dbContext) => this.dbContext = dbContext;

        public async Task<Unit> Handle(MemberRoleDelete request, CancellationToken cancellationToken)
        {
            var entity = dbContext.Members!.Include(e => e.Roles).FirstOrDefault(e => e.Id == request.MemberId)?.Roles.FirstOrDefault(e => e.TreeId == request.TreeId);
            if (entity != null)
                dbContext.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
