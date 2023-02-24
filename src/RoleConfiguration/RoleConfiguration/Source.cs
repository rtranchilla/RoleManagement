namespace RoleConfiguration;

public sealed class Source : EntityWithId
{
    public Source(string name) : this(Guid.NewGuid(), name) { }
    public Source(Guid id, string name) : base(id) => Name = name;

    public string Name { get; private set; }
}
