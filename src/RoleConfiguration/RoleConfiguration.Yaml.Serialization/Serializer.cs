using YamlDotNet.Serialization;

namespace RoleConfiguration.Yaml.Serialization;

public sealed class Serializer
{
    private readonly ISerializer serializer;
    public Serializer() => serializer = new SerializerBuilder()
        .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull | DefaultValuesHandling.OmitEmptyCollections | DefaultValuesHandling.OmitDefaults)
        .Build();

    public string Serialize(object graph, bool compressImplied = false)
    {
        if (compressImplied)
            if (graph is RoleTreeFileContent fileContent)
                graph = Sort(Compress(fileContent));
            else if (graph is MemberFileContent memberContent)
                graph = Sort(Compress(memberContent));

        return serializer.Serialize(graph);
    }

    public RoleTreeFileContent Compress(RoleTreeFileContent content)
    {
        if (string.IsNullOrWhiteSpace(content.DefaultRoleTree))
        {
            var treeCount = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var role in content.Roles)
                if (!string.IsNullOrWhiteSpace(role.Tree))
                {
                    if (!treeCount.ContainsKey(role.Tree))
                        treeCount.Add(role.Tree, 0);
                    treeCount[role.Tree]++;
                }

            content.DefaultRoleTree = treeCount.OrderByDescending(x => x.Value).FirstOrDefault().Key;
        }

        foreach (var role in content.Roles)
            if (!string.IsNullOrWhiteSpace(role.Tree) && StringComparer.InvariantCultureIgnoreCase.Equals(content.DefaultRoleTree, role.Tree))
                role.Tree = null;
        return content;
    }

    public RoleTreeFileContent Sort(RoleTreeFileContent content)
    {
        content.Roles = content.Roles.OrderBy(x => x.Name).ToList();
        foreach (var role in content.Roles)
            role.RequiredNodes = role.RequiredNodes.OrderBy(x => x.Name).ToList();

        content.Trees = content.Trees.OrderBy(x => x.Name).ToList();
        foreach (var tree in content.Trees)
            tree.RequiredNodes = tree.RequiredNodes.OrderBy(x => x.Name).ToList();

        return content;
    }

    public MemberFileContent Compress(MemberFileContent content)
    {
        var roleCount = new Dictionary<MemberRoleContent, int>();
        foreach (var role in content.Roles)
            if (!string.IsNullOrWhiteSpace(role.Name) && !string.IsNullOrWhiteSpace(role.Tree))
            {
                if (!roleCount.ContainsKey(role))
                    roleCount.Add(role, 0);
                roleCount[role]++;
            }

        var globalRoles = roleCount.Where(e => e.Value == content.Roles.Count).Select(e => e.Key).ToList();

        foreach (var role in globalRoles)
            if (!content.Roles.Contains(role))
                content.Roles.Add(role);

        foreach (var member in content.Members)
        {
            if (StringComparer.InvariantCultureIgnoreCase.Equals(member.DisplayName, member.UniqueName))
                member.DisplayName = null;
            foreach (var role in globalRoles)
                if (member.Roles.Contains(role))
                    member.Roles.Remove(role);
        }

        return content;
    }

    public MemberFileContent Sort(MemberFileContent content)
    {
        content.Roles = content.Roles.OrderBy(x => x.Name).ToList();
        content.Members = content.Members.OrderBy(x => x.UniqueName).ToList();
        foreach (var member in content.Members)
            member.Roles = member.Roles.OrderBy(x => x.Name).ToList();
        return content;
    }
}
