using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;

namespace RoleManager.Queries;

public sealed record MemberRoleQuery : AggregateRootQuery<Dto.Role>
{
    public MemberRoleQuery(Guid memberId) => MemberId = memberId;

    public Guid MemberId { get; }
}

public sealed class MemberRoleQueryHandler : IRequestHandler<MemberRoleQuery, IEnumerable<Dto.Role>>
{
    private readonly RoleDbContext dbContext;
    private readonly IMapper mapper;

    public MemberRoleQueryHandler(RoleDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<Dto.Role>> Handle(MemberRoleQuery request, CancellationToken cancellationToken)
    {
        var roles = await Task.Run(() => dbContext.Members!
                                                  .Include(e => e.Roles)
                                                  .ThenInclude(e => e.Role)
                                                  .ThenInclude(e => e!.Nodes.OrderBy(rn => rn.Order))
                                                  .ThenInclude(e => e.Node)
                                                  .SelectMany(e => e.Roles)
                                                  .Select(e => e.Role).ToArray());

        return await Task.Run(() => mapper.Map<IEnumerable<Role>, Dto.Role[]>(roles!), cancellationToken);
    }
}
