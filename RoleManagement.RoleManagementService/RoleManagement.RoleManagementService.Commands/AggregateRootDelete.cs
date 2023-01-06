using MediatR;
using RoleManagement.RoleManagementService.DataPersistence;

namespace RoleManagement.RoleManagementService.Commands;

public abstract record AggregateRootDelete : AggregateRootCommon;

public abstract class AggregateRootDeleteHandler<TRequest, TAggregateRoot> : IRequestHandler<TRequest>
    where TRequest : AggregateRootDelete
    where TAggregateRoot : class, IEntity
{
    private readonly RoleDbContext dbContext;

    public AggregateRootDeleteHandler(RoleDbContext dbContext) => this.dbContext = dbContext;

    protected abstract TAggregateRoot? GetEntity(TRequest request, RoleDbContext dbContext);

    public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            var entity = GetEntity(request, dbContext);
            if (entity != null)
                dbContext.Remove(entity);
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}