namespace RoleConfiguration.Yaml;

public sealed class MemberContent
{
    public string? DisplayName { get; set; }
    [Required]
    public string? UniqueName { get; set; }
    public List<MemberRoleContent> Roles { get; set; } = new();
}
