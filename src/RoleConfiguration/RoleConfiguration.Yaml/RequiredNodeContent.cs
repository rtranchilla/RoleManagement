namespace RoleConfiguration.Yaml;

public sealed class RequiredNodeContent
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Tree { get; set; }

    public bool Equals(RequiredNodeContent? other) => other is not null &&
        StringComparer.InvariantCultureIgnoreCase.Equals(Name, other.Name) &&
        StringComparer.InvariantCultureIgnoreCase.Equals(Tree, other.Tree);

    public override bool Equals(object? obj) => obj is not null && Equals(obj as RequiredNodeContent);
    public override int GetHashCode() => HashCode.Combine(Name, Tree);
}
