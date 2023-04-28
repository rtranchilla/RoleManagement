namespace RoleConfiguration.Yaml;

public sealed class MemberRoleContent : IEquatable<MemberRoleContent>
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Tree { get; set; }

    public bool Equals(MemberRoleContent? other) => other is not null && 
        StringComparer.InvariantCultureIgnoreCase.Equals(Name, other.Name) && 
        StringComparer.InvariantCultureIgnoreCase.Equals(Tree, other.Tree);

    public override bool Equals(object? obj) => obj is not null && Equals(obj as MemberRoleContent);
    public override int GetHashCode() => HashCode.Combine(Name, Tree);
}
