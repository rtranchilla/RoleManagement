namespace RoleManager;

public sealed class MemberRole : AssociatorEntity<MemberRole>
{
    public MemberRole(Guid memberId, Guid treeId, Guid roleId) : base(mr => mr.MemberId, mr => mr.TreeId)
    {
        MemberId = memberId;
        TreeId = treeId;
        RoleId = roleId;
    }

    public Guid MemberId { get; private set; }
    public Guid TreeId { get; private set; }
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}
