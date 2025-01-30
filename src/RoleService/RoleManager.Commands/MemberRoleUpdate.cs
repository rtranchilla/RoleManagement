using AutoMapper;
using Dapr.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using RoleManager.Configuration;
using RoleManager.DataPersistence;
using RoleManager.Events;
using RoleManager.RoleHierarchy;
using System.Security.Policy;

namespace RoleManager.Commands
{
    public sealed record MemberRoleUpdate(Guid MemberId, Guid TreeId, Guid RoleId) : AggregateRootCommon
    {
        public bool Force { get; set; }
    }
    public sealed class MemberRoleUpdateHandler(RoleDbContext dbContext, DaprClient daprClient, PubSubConfiguration configuration) : IRequestHandler<MemberRoleUpdate>
    {
        public async Task Handle(MemberRoleUpdate request, CancellationToken cancellationToken)
        {
            using (IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken))
                try
                {
                    var member = dbContext.Members!.IncludeSubordinate(true)
                                                   .FirstOrDefault(e => e.Id == request.MemberId);
                    if (member == null)
                        throw new NullReferenceException("Failed to locate member with specified id.");

                    var memberNodeIds = member.Roles.SelectMany(e => e.Role!.Nodes).Select(e => e.NodeId);
                    var newRole = dbContext.Roles!.First(e => e.Id == request.RoleId);
                    var treeMap = TreeMap.Build(dbContext.Roles!.IncludeSubordinate(true).Where(e => e.TreeId == request.TreeId));
                    var entity = member.Roles.FirstOrDefault(e => e.TreeId == request.TreeId);
                    if (entity == null)
                    {
                        if (!treeMap.IsAssignable(newRole, memberNodeIds))
                            throw new ArgumentException("Invalid role assignment.");

                        entity = new MemberRole(request.MemberId, request.TreeId, request.RoleId);
                        await dbContext.AddAsync(entity, cancellationToken);
                        member.Roles.Add(entity);
                    }
                    else
                    {
                        var currentRole = entity.Role!;
                        if (!treeMap.IsTraversable(currentRole, newRole, memberNodeIds))
                            throw new ArgumentException("Invalid role assignment.");

                        entity.RoleId = request.RoleId;
                    }

                    await dbContext.SaveChangesAsync(cancellationToken);

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
                catch
                {
                    transaction.Rollback();
                    throw;
                }
        }
    }
}
