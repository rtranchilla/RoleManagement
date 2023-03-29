using MediatR;
using RoleConfiguration.Repositories;
using RoleConfiguration.Yaml;
using RoleConfiguration.Yaml.Serialization;
using RoleManager.Queries;

namespace RoleConfiguration.Queries;

public sealed record RoleTreeFileQuery : QueryRequest<string>
{
    public RoleTreeFileQuery(params (string Name, string Tree)[] roles) => Roles = roles;
    public RoleTreeFileQuery(string tree) => Tree = tree;

    public (string Name, string Tree)[]? Roles { get; }
    public string? Tree { get; }
}

public sealed class RoleTreeFileQueryHandler : IRequestHandler<RoleTreeFileQuery, string>
{
    private readonly Serializer serializer;
    private readonly IRoleRepository roleRepository;

    public RoleTreeFileQueryHandler(Serializer serializer, IRoleRepository roleRepository)
    {
        this.serializer = serializer;
        this.roleRepository = roleRepository;
    }

    // ToDo: Complete RoleTreeFileQueryHandler
    public async Task<string> Handle(RoleTreeFileQuery request, CancellationToken cancellationToken)
    {
        var content = new RoleTreeFileContent();
        if (string.IsNullOrEmpty(request.Tree))
            await AddSpecifiedTree(request, content, cancellationToken);
        else if (request.Roles?.Any() ?? false)
            await AddSpecifiedRoles(request, content, cancellationToken);

        return serializer.Serialize(content);
    }

    private async Task AddSpecifiedTree(RoleTreeFileQuery request, RoleTreeFileContent content, CancellationToken cancellationToken)
    {

    }

    private async Task AddSpecifiedRoles(RoleTreeFileQuery request, RoleTreeFileContent content, CancellationToken cancellationToken)
    {

    }
}

