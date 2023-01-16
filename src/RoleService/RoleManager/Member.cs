namespace RoleManagement.RoleManagementService;

public sealed class Member : EntityWithId
{
    public Member(string displayName, string uniqueName) : this(Guid.NewGuid(), displayName, uniqueName) { }
    public Member(Guid id, string displayName, string uniqueName) : base(id)
    {
        if (string.IsNullOrWhiteSpace(displayName))        
            throw new ArgumentException($"'{nameof(displayName)}' cannot be null or whitespace.", nameof(displayName));
        if (string.IsNullOrWhiteSpace(uniqueName))
            throw new ArgumentException($"'{nameof(uniqueName)}' cannot be null or whitespace.", nameof(uniqueName));     

        DisplayName = displayName;
        UniqueName = uniqueName;
    }

    public string DisplayName { get; set; }
    public string UniqueName { get; private set; }

    public IList<MemberRole> Roles { get; private set; } = new List<MemberRole>();
}
