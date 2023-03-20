using AutoMapper;
using MediatR;
using RoleConfiguration.DataPersistence;

namespace RoleConfiguration.Commands;

public sealed record SourceCreate(string Name) : CommandRequest;

public sealed class SourceCreateHandler : IRequestHandler<SourceCreate>
{
    private readonly ConfigDbContext dbContext;

    public SourceCreateHandler(ConfigDbContext dbContext) => this.dbContext = dbContext;

    public async Task<Unit> Handle(SourceCreate request, CancellationToken cancellationToken)
    {
        var source = new Source(request.Name);
        await dbContext.Sources!.AddAsync(source, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}