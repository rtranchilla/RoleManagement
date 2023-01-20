using MediatR;
using RoleManager.DataPersistence;

namespace RoleManager.Commands;

public abstract record AggregateRootDelete : AggregateRootCommon;

public abstract class AggregateRootDeleteHandler<TRequest, TAggregateRoot> : IRequestHandler<TRequest>
    where TRequest : AggregateRootDelete
    where TAggregateRoot : class, IEntity
{
    private readonly RoleDbContext dbContext;

    public AggregateRootDeleteHandler(RoleDbContext dbContext) => this.dbContext = dbContext;

    protected abstract TAggregateRoot? GetEntity(TRequest request, RoleDbContext dbContext);
    protected virtual Task PostSave(TRequest request, RoleDbContext dbContext) => Task.CompletedTask;

    public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await dbContext.Database.BeginTransactionAsync();
        await Task.Run(() =>
        {
            var entity = GetEntity(request, dbContext);
            if (entity != null)
                dbContext.Remove(entity);
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await dbContext.Database.CommitTransactionAsync();
        await PostSave(request, dbContext);
        return Unit.Value;
    }
}