using RoleConfiguration.DataPersistence;

namespace RoleConfiguration.Commands;

public sealed record SourceDelete(string Name) : IRequest;

public sealed class SourceDeleteHandler : IRequestHandler<SourceDelete>
{
    private readonly ConfigDbContext dbContext;

    public SourceDeleteHandler(ConfigDbContext dbContext) => this.dbContext = dbContext;

    public async Task<Unit> Handle(SourceDelete request, CancellationToken cancellationToken)
    {
        var source = dbContext.Sources!.FirstOrDefault(e => e.Name == request.Name);
        if (source != null)
        {
            dbContext.Sources!.Remove(source);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}