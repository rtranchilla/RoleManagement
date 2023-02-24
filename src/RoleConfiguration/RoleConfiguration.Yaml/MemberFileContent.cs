namespace RoleConfiguration.Yaml;

public sealed class MemberFileContent
{
    public List<MemberContent> Members { get; set; } = new();
    public List<MemberRoleContent> Roles { get; set; } = new();
}
