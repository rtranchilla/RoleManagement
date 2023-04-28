namespace RoleConfiguration.Yaml;

public sealed class MemberContent : IEquatable<MemberContent>
{
    public string? DisplayName { get; set; }
    [Required]
    public string? UniqueName { get; set; }
    public List<MemberRoleContent> Roles { get; set; } = new();

    public bool Equals(MemberContent? other) => other is not null && 
        StringComparer.InvariantCultureIgnoreCase.Equals(UniqueName, other.UniqueName);

    public override bool Equals(object? obj) => obj is not null && Equals(obj as MemberContent);
    public override int GetHashCode() => HashCode.Combine(UniqueName);
}
