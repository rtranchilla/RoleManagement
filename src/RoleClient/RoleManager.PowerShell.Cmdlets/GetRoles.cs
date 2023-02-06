using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Get, "Roles", DefaultParameterSetName = parameterSetNone)]
[OutputType(typeof(Role))]
public sealed class GetRoles : RmCmdlet
{
    const string parameterSetNone = "None";
    const string parameterSetId = "NameOrId";
    const string parameterSetMemberId = "MemberId";
    const string parameterSetAvailableMemberId = "MemberId";

    [Parameter(Position = 0, ParameterSetName = parameterSetId)]
    public string? Identifier { get; set; }
    [Parameter(ParameterSetName = parameterSetMemberId)]
    public Guid? MemberId { get; set; }
    [Parameter(ParameterSetName = parameterSetAvailableMemberId)]
    public Guid? AvailableForMemberId { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (ParameterSetName == parameterSetNone)
            WriteObject(SendRequest(new RoleQuery()));
        else if (ParameterSetName == parameterSetMemberId)
            WriteObject(SendRequest(new RoleQuery { MemberId = MemberId }), true);
        else if (ParameterSetName == parameterSetAvailableMemberId)
            WriteObject(SendRequest(new RoleQuery { AvailableForMemberId = AvailableForMemberId }), true);
        if (Guid.TryParse(Identifier, out var id))
            WriteObject(SendRequest(new RoleQuery { Id = id }), true);
        else
            WriteObject(SendRequest(new RoleQuery()), true);
    }
}