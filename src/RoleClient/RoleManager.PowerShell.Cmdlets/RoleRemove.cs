using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Remove, "Role", DefaultParameterSetName = parameterSetByProperty)]
public sealed class RoleRemove : RmCmdlet
{
    const string parameterSetByObject = "ByObject";
    const string parameterSetByProperty = "ByProperty";

    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ParameterSetName = parameterSetByObject)]
    public Role? Role { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 0,
        ParameterSetName = parameterSetByProperty)]
    public Guid Id { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (ParameterSetName == parameterSetByProperty)
            SendRequest(new RoleDelete(Id));
        else
            SendRequest(new RoleDelete(Role!.Id));
    }
}
