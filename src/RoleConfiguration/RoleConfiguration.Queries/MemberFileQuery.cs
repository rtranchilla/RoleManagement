using MediatR;
using RoleConfiguration.Repositories;
using RoleConfiguration.Yaml;
using RoleConfiguration.Yaml.Serialization;
using RoleManager.Queries;

namespace RoleConfiguration.Queries;

public sealed record MemberFileQuery : QueryRequest<string>
{
    public MemberFileQuery(params (string Name, string Tree)[] roles) => Roles = roles;
    public MemberFileQuery(params string[] members) => Members = members;

    public (string Name, string Tree)[]? Roles { get; set; }
    public string[]? Members { get; }
}

public sealed class MemberFileQueryHandler : IRequestHandler<MemberFileQuery, string>
{
    private readonly Serializer serializer;
    private readonly IMemberRepository memberRepository;
    private readonly IRoleRepository roleRepository;

    public MemberFileQueryHandler(Serializer serializer, IMemberRepository memberRepository, IRoleRepository roleRepository)
    {
        this.serializer = serializer;
        this.memberRepository = memberRepository;
        this.roleRepository = roleRepository;
    }

    public async Task<string> Handle(MemberFileQuery request, CancellationToken cancellationToken)
    {
        var content = new MemberFileContent();

        if (request.Members?.Any() ?? false)
            await AddSpecifiedMembers(request, content, cancellationToken);
        else if (request.Roles?.Any() ?? false)
            await AddSpecifiedRoleMembers(request, content, cancellationToken);

        MergeRoles(content);

        return serializer.Serialize(content);
    }

    private async Task AddSpecifiedRoleMembers(MemberFileQuery request, MemberFileContent content, CancellationToken cancellationToken)
    {
        var currentMembers = new HashSet<Guid>();
        foreach (var (roleName, roleTree) in request.Roles!)
        {
            var (roleEntity, _) = await roleRepository.Get(roleName, roleTree, cancellationToken);
            if (roleEntity != null)
            {
                var members = await memberRepository.GetByRole(roleEntity.Id, cancellationToken);
                foreach (var (memberEntity, memberContent) in members)
                    if (memberEntity != null && !currentMembers.Contains(memberEntity.Id))
                    {
                        content.Members.Add(memberContent!);
                        currentMembers.Add(memberEntity.Id);
                    }
            }
        }
    }

    private async Task AddSpecifiedMembers(MemberFileQuery request, MemberFileContent content, CancellationToken cancellationToken)
    {
        foreach (var member in request.Members!)
        {
            var (_, memberContent) = await memberRepository.Get(member, cancellationToken);
            if (memberContent != null)
                content.Members.Add(memberContent);
        }
    }

    public void MergeRoles(MemberFileContent content)
    {
        var memberRoles = content.Members.FirstOrDefault()?.Roles.ToDictionary(k => $"{k.Name}_{k.Tree}", StringComparer.InvariantCultureIgnoreCase);
        if (memberRoles != null)
        {
            foreach (var member in content.Members.TakeLast(content.Members.Count - 1))
            {
                var roleNames = member.Roles.Select(r => $"{r.Name}_{r.Tree}").ToHashSet(StringComparer.InvariantCultureIgnoreCase);
                foreach (var removeName in memberRoles.Keys.Where(k => !roleNames.Contains(k)).ToArray())
                    memberRoles.Remove(removeName);
            }

            if (memberRoles.Any())
                foreach (var member in content.Members)
                    member.Roles = member.Roles.Where(r => !memberRoles.ContainsKey($"{r.Name}_{r.Tree}")).ToList();
        }

        content.Roles = memberRoles?.Values.ToList() ?? new List<MemberRoleContent>();
    }
}