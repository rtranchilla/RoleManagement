using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands
{
    public sealed record MemberRoleDelete(Guid MemberId, Guid TreeId) : AggregateRootCommon;
    public sealed class MemberRoleDeleteHandler : IRequestHandler<MemberRoleDelete>
    {
        private readonly RoleDbContext dbContext;
        private readonly IPublisher publisher;

        public MemberRoleDeleteHandler(RoleDbContext dbContext, IPublisher publisher)
        {
            this.dbContext = dbContext;
            this.publisher = publisher;
        }

        public async Task Handle(MemberRoleDelete request, CancellationToken cancellationToken)
        {
            using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var member = dbContext.Members!.Include(e => e.Roles).ThenInclude(e => e.Role!.Nodes).FirstOrDefault(e => e.Id == request.MemberId);
                var entity = member?.Roles.FirstOrDefault(e => e.TreeId == request.TreeId);
                if (entity != null)
                {
                    dbContext.Remove(entity);

                    await dbContext.SaveChangesAsync(cancellationToken);
                    member!.Roles.Remove(entity);

                    await publisher.Publish(new MemberUpdated(member, MemberFunctions.GetNodeIds(dbContext, member.Id)), cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
