﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using RoleManager.DataPersistence;

namespace RoleManager.Commands;

public abstract record AggregateRootUpdate : AggregateRootCommon;

public abstract class AggregateRootUpdateHandler<TRequest, TAggregateRoot, TDto> : IRequestHandler<TRequest>
    where TRequest : AggregateRootUpdate
    where TAggregateRoot : class, IEntity
    where TDto : class, Dto.IEntity
{
    private readonly RoleDbContext dbContext;
    private readonly IMapper mapper;

    public AggregateRootUpdateHandler(RoleDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    protected abstract TDto GetDto(TRequest request);
    protected abstract TAggregateRoot? GetEntity(TRequest request, RoleDbContext dbContext);
    protected virtual Task PostSave(TAggregateRoot aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var dto = await Task.Run(() => GetDto(request), cancellationToken);
        if (dto == null)
            throw new ArgumentNullException(nameof(request), $"Unable to find the Dto object from the supplied request.");

        var entity = await Task.Run(() => GetEntity(request, dbContext), cancellationToken);
        if (entity == null)
            throw new NullReferenceException($"Failed to find an entity with the supplied id: {dto.Id}.");

        using (IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken))
            try
            {
                entity = mapper.Map(dto, entity);
                await dbContext.SaveChangesAsync(cancellationToken);
                await PostSave(entity, dbContext, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        return Unit.Value;
    }
}