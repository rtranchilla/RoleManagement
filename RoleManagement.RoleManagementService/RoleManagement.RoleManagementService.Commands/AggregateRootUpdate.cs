using AutoMapper;
using MediatR;
using RoleManagement.RoleManagementService.DataPersistence;

namespace RoleManagement.RoleManagementService.Commands;

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
    protected abstract TAggregateRoot? GetAggregateRoot(TRequest request, RoleDbContext dbContext);

    public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var dto = await Task.Run(() => GetDto(request), cancellationToken);
        if (dto == null)
            throw new ArgumentNullException(nameof(request), $"Unable to find the Dto object from the supplied request.");

        var entity = await Task.Run(() => GetAggregateRoot(request, dbContext), cancellationToken);
        if (entity == null)
            throw new NullReferenceException($"Failed to find an entity with the supplied id: {dto.Id}.");

        entity = mapper.Map(dto, entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}