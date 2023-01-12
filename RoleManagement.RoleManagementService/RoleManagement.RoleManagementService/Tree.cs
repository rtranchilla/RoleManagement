namespace RoleManagement.RoleManagementService;

public sealed class Tree : EntityWithId
{
    public Tree(string name) : this(Guid.NewGuid(), name) { }
    public Tree(Guid id, string name) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        Name = name;
    }

    public string Name { get; private set; }
}
