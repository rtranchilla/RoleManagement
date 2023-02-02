namespace RoleManager.PowerShell;

public sealed class Tree
{
    public Tree(Guid id, string name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public Guid Id { get; }
    public string Name { get; }
    public Guid[] RequiredNodes { get; set; } = Array.Empty<Guid>();
}