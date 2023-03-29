using MediatR;
using RoleConfiguration.DataPersistence;
using RoleManager.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleConfiguration.Queries;

public sealed record SourceQuery : QueryRequest<string[]>;

public sealed class SourceQueryHandler : IRequestHandler<SourceQuery, string[]>
{
    private readonly ConfigDbContext dbContext;

    public SourceQueryHandler(ConfigDbContext dbContext) => this.dbContext = dbContext;

    public Task<string[]> Handle(SourceQuery request, CancellationToken cancellationToken) => 
        Task.Run(() => dbContext.Sources!.Select(e => e.Name).ToArray(), cancellationToken);
}