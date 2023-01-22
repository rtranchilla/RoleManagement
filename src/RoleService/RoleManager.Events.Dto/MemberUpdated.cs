namespace RoleManager.Events.Payloads;

public sealed class MemberUpdated
{
    public Guid Id { get; set; }
    public Guid[] NodeIds { get; set; }
}
