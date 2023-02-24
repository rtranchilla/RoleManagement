namespace RoleConfiguration.Yaml;

public sealed class MemberRoleContent
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Tree { get; set; }
}
