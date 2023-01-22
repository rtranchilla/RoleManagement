using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RoleManager.DataPersistence;
using RoleManager.Events;
using System.Security.Policy;

namespace RoleManager.Commands
{
    public sealed record MemberRoleUpdate(Guid MemberId, Guid TreeId, Guid RoleId) : AggregateRootCommon;
    public sealed class MemberRoleUpdateHandler : IRequestHandler<MemberRoleUpdate>
    {
        private readonly RoleDbContext dbContext;
        private readonly IPublisher publisher;

        public MemberRoleUpdateHandler(RoleDbContext dbContext, IPublisher publisher)
        {
            this.dbContext = dbContext;
            this.publisher = publisher;
        }

        public async Task<Unit> Handle(MemberRoleUpdate request, CancellationToken cancellationToken)
        {
            using (IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken))
                try
                {
                    var member = dbContext.Members!.Include(e => e.Roles).ThenInclude(e => e.Role!.Nodes).FirstOrDefault(e => e.Id == request.MemberId);
                    if (member == null)
                        throw new NullReferenceException("Failed to locate member with specified id.");

                    var entity = member.Roles.FirstOrDefault(e => e.TreeId == request.TreeId);
                    if (entity == null)
                    {
                        entity = new MemberRole(request.MemberId, request.TreeId, request.RoleId);
                        await dbContext.AddAsync(entity, cancellationToken);
                        member.Roles.Add(entity);
                    }
                    else
                        entity.RoleId = request.RoleId;

                    await dbContext.SaveChangesAsync(cancellationToken);

                    await publisher.Publish(new MemberUpdated(member, MemberFunctions.GetNodeIds(dbContext, member.Id)), cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }            
            return Unit.Value;
        }
    }
}
