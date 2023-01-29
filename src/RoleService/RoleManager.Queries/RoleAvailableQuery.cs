using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;
using System.Data;
using System.Linq;

namespace RoleManager.Queries;

public sealed record RoleAvailableQuery(Guid MemberId) : AggregateRootQuery<Dto.Role>;

public sealed class RoleAvailableQueryHandler : AggregateRootQueryHandler<RoleAvailableQuery, Role, Dto.Role>
{
    public RoleAvailableQueryHandler(RoleDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override IQueryable<Role> QueryEntities(RoleAvailableQuery request, RoleDbContext dbContext)
    {
        var dataTable = new DataTable();
        using (var command = dbContext.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "sp_GetAvailableMemberRoles";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("MemberId", request.MemberId));

            dbContext.Database.OpenConnection();

            using var result = command.ExecuteReader();
            dataTable.Load(result);
        }

        var roleIds = new List<Guid>();
        foreach (DataRow row in dataTable.Rows)
            roleIds.Add((Guid)row[0]);

        IQueryable<Role> query = dbContext.Roles!.IncludeSubordinate(true).Where(e => roleIds.Contains(e.Id));

        //if (request.Id != null)
        //    return query.Where(e => e.Id == request.Id);

        return query;
    }
}