namespace RoleConfiguration;

public sealed class FileRole : AssociatorEntity<FileRole>
{
    public FileRole(Guid fileId, Guid roleId) : base(e => e.FileId, e => e.RoleId)
    {
        FileId = fileId;
        RoleId = roleId;
    }

    public Guid FileId { get; private set; }
    public Guid RoleId { get; private set; }
    public Role? Role { get; set; }
}
