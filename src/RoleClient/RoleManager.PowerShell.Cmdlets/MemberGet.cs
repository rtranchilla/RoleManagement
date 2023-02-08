using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Get, "Member", DefaultParameterSetName = parameterSetNone)]
[OutputType(typeof(Member))]
public sealed class MemberGet : RmCmdlet
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
            WriteObject(SendRequest(new MemberQuery()));
        else if (ParameterSetName == parameterSetRole)
            WriteObject(SendRequest(new MemberQuery { RoleId = RoleId }), true);
        else if (Guid.TryParse(Identifier, out var id))
            WriteObject(SendRequest(new MemberQuery { Id = id }), true);
        else
            WriteObject(SendRequest(new MemberQuery { UniqueName = Identifier }), true);
    }
}