namespace RoleConfiguration.Yaml;

public sealed class TreeContent : IEquatable<TreeContent>
{
    [Required]
    public string? Name { get; set; }
    public List<RequiredNodeContent> RequiredNodes { get; set; } = new();

    public bool Equals(TreeContent? other) => other is not null &&
        StringComparer.InvariantCultureIgnoreCase.Equals(Name, other.Name);

    public override bool Equals(object? obj) => obj is not null && Equals(obj as TreeContent);
    public override int GetHashCode() => HashCode.Combine(Name);
}
