using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using RoleManager.DataPersistence;

namespace RoleManager.Commands;

public abstract record AggregateRootUpdate : AggregateRootCommon;

public abstract class AggregateRootUpdateHandler<TRequest, TAggregateRoot, TDto>(RoleDbContext dbContext, IMapper mapper) : IRequestHandler<TRequest>
    where TRequest : AggregateRootUpdate
    where TAggregateRoot : class, IEntity
    where TDto : class, Dto.IEntity
{
    protected abstract TDto GetDto(TRequest request);
    protected abstract TAggregateRoot? GetEntity(TRequest request, RoleDbContext dbContext);
    protected virtual Task PostMap(TRequest request, TAggregateRoot aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) => Task.CompletedTask;
    protected virtual Task PostSave(TAggregateRoot aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task Handle(TRequest request, CancellationToken cancellationToken)
    {
        var dto = await Task.Run(() => GetDto(request), cancellationToken) ?? throw new ArgumentNullException(nameof(request), $"Unable to find the Dto object from the supplied request.");
        var entity = await Task.Run(() => GetEntity(request, dbContext), cancellationToken) ?? throw new KeyNotFoundException($"Failed to find an entity with the supplied id: {dto.Id}.");
        using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            entity = mapper.Map(dto, entity);
            await PostMap(request, entity, dbContext, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await PostSave(entity, dbContext, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}