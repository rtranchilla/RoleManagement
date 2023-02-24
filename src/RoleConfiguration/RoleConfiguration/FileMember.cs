namespace RoleConfiguration;

public sealed class FileMember : AssociatorEntity<FileMember>
{
    public FileMember(Guid fileId, Guid memberId) : base(e => e.FileId, e => e.MemberId)
    {
        FileId = fileId;
        MemberId = memberId;
    }

    public Guid FileId { get; private set; }
    public Guid MemberId { get; private set; }
    public Member? Member { get; set; }
}
