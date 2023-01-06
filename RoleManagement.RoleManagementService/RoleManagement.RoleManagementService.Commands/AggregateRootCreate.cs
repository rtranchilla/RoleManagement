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

    public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var newEntity = await Task.Run(() => mapper.Map<TDto, TAggregateRoot>(GetDto(request)), cancellationToken);
        if (newEntity == null)
            throw new NullReferenceException($"Failed to generate new object for supplied dto of type: {GetDto(request)?.GetType().Name ?? "Unknown"}.");

        await dbContext.AddAsync(newEntity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}