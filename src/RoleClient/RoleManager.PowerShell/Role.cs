namespace RoleManager.PowerShell;

public sealed class Role
{
    public Role(Guid id, string name, Guid treeId)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        TreeId = treeId;
    }

    public Guid Id { get; }
    public bool Reversible { get; set; }
    public string Name { get; }
    public Guid TreeId { get; }
    public Guid[] RequiredNodes { get; set; } = Array.Empty<Guid>();
}