﻿using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using RoleManager.DataPersistence;

namespace RoleManager.Commands;

public abstract record AggregateRootDelete : AggregateRootCommon;

public abstract class AggregateRootDeleteHandler<TRequest, TAggregateRoot>(RoleDbContext dbContext) : IRequestHandler<TRequest>
    where TRequest : AggregateRootDelete
    where TAggregateRoot : class, IEntity
{
    protected abstract TAggregateRoot? GetEntity(TRequest request, RoleDbContext dbContext);
    protected virtual Task PostSave(TRequest request, RoleDbContext dbContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task Handle(TRequest request, CancellationToken cancellationToken)
    {
        using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await Task.Run(() =>
            {
                var entity = GetEntity(request, dbContext);
                if (entity != null)
                    dbContext.Remove(entity);
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await PostSave(request, dbContext, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}