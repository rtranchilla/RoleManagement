namespace RoleManager.PowerShell;

public sealed class Member
{
    public Member(Guid id, string displayName, string uniqueName)
    {
        Id = id;
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        UniqueName = uniqueName ?? throw new ArgumentNullException(nameof(uniqueName));
    }

    public Guid Id { get; }
    public string DisplayName { get; set; }
    public string UniqueName { get; }
}