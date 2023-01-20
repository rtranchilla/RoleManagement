namespace RoleManager.Events.Payloads;

public sealed class MemberCreated
{
    public Guid Id { get; set; }
    public Guid[] NodeId { get; set; }
    public string UniqueName { get; set; }

}