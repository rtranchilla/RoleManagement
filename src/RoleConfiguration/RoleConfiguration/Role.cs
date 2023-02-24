namespace RoleConfiguration;

public class Role : EntityWithId
{
    public Role(string name, Guid treeId) : this(Guid.NewGuid(), name, treeId) { }
    public Role(Guid id, string name, Guid treeId) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

        Name = name;
        TreeId = treeId;
    }

    public string Name { get; private set; }
    public Guid TreeId { get; private set; }
    public Tree? Tree { get; set; }
}
