namespace RoleConfiguration;

public sealed class Member : EntityWithId
{
    public Member(string uniqueName) : this(Guid.NewGuid(), uniqueName) { }
    public Member(Guid id, string uniqueName) : base(id)
    {
        if (string.IsNullOrWhiteSpace(uniqueName))
            throw new ArgumentException($"'{nameof(uniqueName)}' cannot be null or whitespace.", nameof(uniqueName));

        UniqueName = uniqueName;
    }

    public string UniqueName { get; private set; }
}
