using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace RoleConfiguration.Yaml.Serialization;

public sealed class Deserializer
{
    private readonly IDeserializer deserializer;

    public Deserializer() => deserializer = new DeserializerBuilder().Build();

    public RoleTreeFileContent DeserializeRoleTree(string content, bool expandImplied = false)
    {
        var result = deserializer.Deserialize<RoleTreeFileContent>(content);

        Validator.ValidateObject(result, new ValidationContext(result), validateAllProperties: true);
        foreach (var role in result.Roles)
        {
            Validator.ValidateObject(role, new ValidationContext(role), validateAllProperties: true);
            foreach (var node in role.RequiredNodes)
                Validator.ValidateObject(node, new ValidationContext(node), validateAllProperties: true);
        }
        foreach (var tree in result.Trees)
        {
            Validator.ValidateObject(tree, new ValidationContext(tree), validateAllProperties: true);
            foreach (var node in tree.RequiredNodes)
                Validator.ValidateObject(node, new ValidationContext(node), validateAllProperties: true);
        }

        if (expandImplied)
            return Expand(result);

        return result;
    }

    private static RoleTreeFileContent Expand(RoleTreeFileContent content)
    {
        // ToDo: Complete expand logic
        return content;
    }

    public MemberFileContent DeserializeMember(string content, bool expandImplied = false)
    {
        var result = deserializer.Deserialize<MemberFileContent>(content);

        Validator.ValidateObject(result, new ValidationContext(result), validateAllProperties: true);
        foreach (var role in result.Roles)
            Validator.ValidateObject(role, new ValidationContext(role), validateAllProperties: true);
        foreach (var member in result.Members)
        {
            Validator.ValidateObject(member, new ValidationContext(member), validateAllProperties: true);
            foreach (var role in member.Roles)
                Validator.ValidateObject(role, new ValidationContext(role), validateAllProperties: true);
        }

        if (expandImplied)
            return Expand(result);

        return result;
    }

    private static MemberFileContent Expand(MemberFileContent content)
    {
        foreach (var member in content.Members)
        {
            if (string.IsNullOrWhiteSpace(member.DisplayName))
                member.DisplayName = member.UniqueName;

            foreach (var role in content.Roles)
                if (!member.Roles.Any(r => r.Tree == role.Tree))
                    member.Roles.Add(role);
        }
        return content;
    }
}