using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using RoleManager.DataPersistence;

namespace RoleManager.Commands;

public abstract record AggregateRootCreate : AggregateRootCommon;

public abstract class AggregateRootCreateHandler<TRequest, TAggregateRoot, TDto> : IRequestHandler<TRequest>
    where TRequest : AggregateRootCreate
    where TAggregateRoot : class, IEntity
    where TDto : class, Dto.IEntity
{
    private readonly RoleDbContext dbContext;
    private readonly IMapper mapper;

    public AggregateRootCreateHandler(RoleDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    protected abstract TDto GetDto(TRequest request);
    protected virtual TAggregateRoot Map(TDto dto, IMapper mapper) => mapper.Map<TDto, TAggregateRoot>(dto);
    protected virtual Task PreCreate(TRequest request, RoleDbContext dbContext, CancellationToken cancellationToken) => Task.CompletedTask;
    protected virtual Task PostMap(TRequest request, TAggregateRoot aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) => Task.CompletedTask;
    protected virtual Task PostSave(TAggregateRoot aggregateRoot, RoleDbContext dbContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task Handle(TRequest request, CancellationToken cancellationToken)
    {
        using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await PreCreate(request, dbContext, cancellationToken);
            var newEntity = await Task.Run(() => Map(GetDto(request), mapper), cancellationToken);
            if (newEntity == null)
                throw new NullReferenceException($"Failed to generate new object for supplied dto of type: {GetDto(request)?.GetType().Name ?? "Unknown"}.");
            await PostMap(request, newEntity, dbContext, cancellationToken);
            await dbContext.AddAsync(newEntity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await PostSave(newEntity, dbContext, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}