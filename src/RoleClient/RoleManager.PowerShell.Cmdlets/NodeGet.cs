using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Get, "Node", DefaultParameterSetName = parameterSetNone)]
[OutputType(typeof(Node))]
public sealed class NodeGet : RmCmdlet
{
    const string parameterSetNone = "None";
    const string parameterSetId = "NameOrId";
    const string parameterSetRole = "RoleId";

    [Parameter(Position = 0, ParameterSetName = parameterSetId)]
    public string? Identifier { get; set; }

    [Parameter(ParameterSetName = parameterSetRole)]
    public Guid? RoleId { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (ParameterSetName == parameterSetNone)
            WriteObject(SendRequest(new NodeQuery()));
        else if (ParameterSetName == parameterSetRole)
            WriteObject(SendRequest(new NodeQuery { RoleId = RoleId }), true);
        else if (Guid.TryParse(Identifier, out var id))
            WriteObject(SendRequest(new NodeQuery { Id = id }), true);
        else
            WriteObject(SendRequest(new NodeQuery { Name = Identifier }), true);
    }
}