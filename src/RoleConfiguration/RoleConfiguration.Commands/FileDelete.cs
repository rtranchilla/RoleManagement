using RoleConfiguration.DataPersistence;

namespace RoleConfiguration.Commands;

public sealed record FileDelete(string Source, string Path) : IRequest;

public sealed class FileDeleteHandler : IRequestHandler<FileDelete>
{
    private readonly ConfigDbContext dbContext;

    public FileDeleteHandler(ConfigDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Unit> Handle(FileDelete request, CancellationToken cancellationToken)
    {
        var fileRecord = dbContext.Files!.FirstOrDefault(e => e.Path == request.Path && e.Source!.Name == request.Source);
        if (fileRecord != null)
        {
            var source = dbContext.Sources!.First(e => e.Name == request.Source);
            fileRecord = new File(request.Path, source.Id);
            dbContext.Files!.Remove(fileRecord);
            await dbContext.SaveChangesAsync();
        }

        return Unit.Value;
    }
}
