namespace RoleConfiguration.Yaml;

public sealed class TreeContent
{
    [Required]
    public string? Name { get; set; }
    public List<RequiredNodeContent> RequiredNodes { get; set; } = new();
}
