namespace RoleConfiguration;

public sealed class File : EntityWithId
{
    public File(string path, Guid sourceId) : this(Guid.NewGuid(), path, sourceId) { }
    public File(Guid id, string path, Guid sourceId) : base(id)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));

        Path = path;
        SourceId = sourceId;
    }

    public string Path { get; private set; }
    public Guid SourceId { get; private set; }
    public Source? Source { get; private set; }

    public IList<FileRole> Roles { get; private set; } = new List<FileRole>();
    public IList<FileTree> Trees { get; private set; } = new List<FileTree>();
    public IList<FileMember> Members { get; private set; } = new List<FileMember>();  
}
