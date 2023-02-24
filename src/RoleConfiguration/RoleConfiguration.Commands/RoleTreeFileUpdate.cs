namespace RoleConfiguration.Commands;

public sealed record RoleTreeFileUpdate(string Source, string Path, string Content) : IRequest;
