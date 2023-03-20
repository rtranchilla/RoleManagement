namespace RoleConfiguration.Repositories;

public interface IRepository<TEntity, TContent>
{
    Task<(TEntity? Entity, TContent? Content)> Get(Guid id, CancellationToken cancellationToken = default);
    Task Add(TEntity entity, TContent content, CancellationToken cancellationToken = default);
    Task Update(TEntity entity, TContent content, CancellationToken cancellationToken = default);
    Task Delete(Guid id, CancellationToken cancellationToken = default);
}

public interface IMemberRepository : IRepository<Member, MemberContent>
{
    Task<(Member? Entity, MemberContent? Content)> Get(string uniqueName, CancellationToken cancellationToken = default);
    Task<IEnumerable<(Member? Entity, MemberContent? Content)>> GetByRole(Guid id, CancellationToken cancellationToken = default);
}

public interface IRoleRepository : IRepository<Role, RoleContent>
{
    Task<(Role? Entity, RoleContent? Content)> Get(string name, string tree, CancellationToken cancellationToken = default);
    Task<IEnumerable<(Role? Entity, MemberRoleContent? Content)>> GetByMember(Guid memberId, CancellationToken cancellationToken = default);
}
public interface ITreeRepository : IRepository<Tree, TreeContent>
{
    Task<(Tree? Entity, TreeContent? Content)> Get(string name, CancellationToken cancellationToken = default);
}