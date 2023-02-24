namespace RoleConfiguration.Yaml;

public sealed class RoleTreeFileContent
{
    public string? DefaultRoleTree { get; set; }
    public List<RoleContent> Roles { get; set; } = new();
    public List<TreeContent> Trees { get; set; } = new();
}