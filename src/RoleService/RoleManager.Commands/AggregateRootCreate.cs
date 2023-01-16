using AutoMapper;
using MediatR;
using RoleManagement.RoleManagementService.DataPersistence;

namespace RoleManagement.RoleManagementService.Commands;

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
    protected virtual Task PreCreate(TRequest request, RoleDbContext dbContext) => Task.CompletedTask;

    public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await PreCreate(request, dbContext);
        var newEntity = await Task.Run(() => Map(GetDto(request), mapper), cancellationToken);
        if (newEntity == null)
            throw new NullReferenceException($"Failed to generate new object for supplied dto of type: {GetDto(request)?.GetType().Name ?? "Unknown"}.");

        await dbContext.AddAsync(newEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}