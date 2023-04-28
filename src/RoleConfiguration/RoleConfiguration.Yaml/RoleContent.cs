using System.ComponentModel;
using YamlDotNet.Serialization;

namespace RoleConfiguration.Yaml;

public sealed class RoleContent : IEquatable<RoleContent>
{
    [Required]
    public string? Name { get; set; }
    public string? Tree { get; set; }
    [DefaultValue(false)]
    public bool? Reversible { get; set; }
    [DefaultValue(false)]
    public bool? BuildParents { get; set; }
    public List<RequiredNodeContent> RequiredNodes { get; set; } = new();

    public bool Equals(RoleContent? other) => other is not null &&
        StringComparer.InvariantCultureIgnoreCase.Equals(Name, other.Name) &&
        StringComparer.InvariantCultureIgnoreCase.Equals(Tree, other.Tree);

    public override bool Equals(object? obj) => obj is not null && Equals(obj as RoleContent);
    public override int GetHashCode() => HashCode.Combine(Name, Tree);
}
