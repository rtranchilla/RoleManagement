namespace RoleConfiguration.Yaml;

public sealed class RoleContent
{
    [Required]
    public string? Name { get; set; }
    public string? Tree { get; set; }
    public bool Reversible { get; set; }
    public bool BuildParents { get; set; }
    public List<RequiredNodeContent> RequiredNodes { get; set; } = new();
}
