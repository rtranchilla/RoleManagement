namespace RoleConfiguration.Yaml;

public sealed class RequiredNodeContent
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Tree { get; set; }
}
