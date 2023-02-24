namespace RoleConfiguration;

public sealed class FileTree : AssociatorEntity<FileTree>
{
    public FileTree(Guid fileId, Guid treeId) : base(e => e.FileId, e => e.TreeId)
    {
        FileId = fileId;
        TreeId = treeId;
    }

    public Guid FileId { get; private set; }
    public Guid TreeId { get; private set; }
    public Tree? Tree { get; set; }
}
