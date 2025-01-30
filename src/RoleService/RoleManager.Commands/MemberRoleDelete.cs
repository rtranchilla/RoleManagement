using AutoMapper;
using Dapr.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using RoleManager.Configuration;
using RoleManager.DataPersistence;
using RoleManager.Events;

namespace RoleManager.Commands
{
    public sealed record MemberRoleDelete(Guid MemberId, Guid TreeId) : AggregateRootCommon;
    public sealed class MemberRoleDeleteHandler(RoleDbContext dbContext, DaprClient daprClient, PubSubConfiguration configuration) : IRequestHandler<MemberRoleDelete>
    {
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

                    await daprClient.PublishEventAsync(
                    configuration.Name,
                        configuration.Topic.Members.Updated,
                        new MemberUpdated
                        {
                            Id = member.Id,
                            NodeIds = MemberFunctions.GetNodeIds(dbContext, member.Id)
                        }, cancellationToken);
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
