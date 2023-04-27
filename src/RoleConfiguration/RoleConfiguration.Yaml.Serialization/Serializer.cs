using YamlDotNet.Serialization;

namespace RoleConfiguration.Yaml.Serialization;

public sealed class Serializer
{
    private readonly ISerializer serializer;
    public Serializer() => serializer = new SerializerBuilder().Build();

    public string Serialize(object graph, bool compressImplied = false)
    {
        if (compressImplied)
            if (graph is RoleTreeFileContent fileContent)
                graph = Compress(fileContent);
            else if (graph is MemberFileContent memberContent)
                graph = Compress(memberContent);

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

    public MemberFileContent Compress(MemberFileContent content)
    {
        foreach (var member in content.Members)
        {
            if (StringComparer.InvariantCultureIgnoreCase.Equals(member.DisplayName, member.UniqueName))
                member.DisplayName = null;

            // ToDo: Compress roles
        }
        return content;
    }
}
