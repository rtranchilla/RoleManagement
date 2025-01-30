using AutoMapper;
using Dapr.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RoleManager.Configuration;
using RoleManager.DataPersistence;
using RoleManager.Events;
using System.Data;
using System.Security.Policy;

namespace RoleManager.Commands;

public sealed record RoleDelete(Guid Id) : AggregateRootDelete;
public sealed class RoleDeleteHandler(RoleDbContext dbContext, DaprClient daprClient, PubSubConfiguration configuration) : AggregateRootDeleteHandler<RoleDelete, Role>(dbContext)
{
    protected override Role? GetEntity(RoleDelete request, RoleDbContext dbContext) => 
        dbContext.Roles!.FirstOrDefault(e => e.Id == request.Id);

    protected override async Task PostSave(RoleDelete request, RoleDbContext dbContext, CancellationToken cancellationToken)
    {
        var dataTable = new DataTable();
        using (var command = dbContext.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "Select [Id] from [dbo].[Nodes] where Id not in (Select NodeId from [dbo].[RoleNodes])";
            command.CommandType = CommandType.Text;
            command.Transaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();

            dbContext.Database.OpenConnection();

            using var result = command.ExecuteReader();
            dataTable.Load(result);
        }

        var nodesToDeleteIds = new List<Guid>();
        foreach (DataRow row in dataTable.Rows)
            nodesToDeleteIds.Add((Guid)row[0]);

        dbContext.Nodes!.RemoveRange(dbContext.Nodes.Where(e => nodesToDeleteIds.Contains(e.Id)));
        await dbContext.SaveChangesAsync();

        foreach (var node in nodesToDeleteIds)
            await daprClient.PublishEventAsync(
            configuration.Name,
            configuration.Topic.Members.Created,
            new NodeDeleted { Id = node }, 
            cancellationToken);
    }
}
