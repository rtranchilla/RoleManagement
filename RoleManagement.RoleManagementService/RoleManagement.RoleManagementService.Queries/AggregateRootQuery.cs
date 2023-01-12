using AutoMapper;
using MediatR;
using RoleManagement.RoleManagementService.DataPersistence;
using System.Linq;

namespace RoleManagement.RoleManagementService.Queries;

public abstract record AggregateRootQuery<TResult> : AggregateRootCommon<IEnumerable<TResult>>;

public abstract class AggregateRootQueryHandler<TRequest, TAggregateRoot, TDto> : IRequestHandler<TRequest, IEnumerable<TDto>>
    where TRequest : AggregateRootQuery<TDto>
    where TAggregateRoot : class, IEntity
    where TDto : class, Dto.IEntity
{
    private readonly RoleDbContext dbContext;
    private readonly IMapper mapper;

    public AggregateRootQueryHandler(RoleDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    protected abstract IQueryable<TAggregateRoot> QueryEntities(TRequest request, RoleDbContext dbContext);

    public async Task<IEnumerable<TDto>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var entities = await Task.Run(() => QueryEntities(request, dbContext), cancellationToken);
        return await Task.Run(() => mapper.Map<IEnumerable<TAggregateRoot>, TDto[]>(entities), cancellationToken);
    }
}