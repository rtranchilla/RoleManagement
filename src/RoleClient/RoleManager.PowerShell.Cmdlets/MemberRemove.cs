using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Remove, "Member", DefaultParameterSetName = parameterSetByProperty)]
public sealed class MemberRemove : RmCmdlet
{
    const string parameterSetByObject = "ByObject";
    const string parameterSetByProperty = "ByProperty";

    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ParameterSetName = parameterSetByObject)]
    public Member? Member { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 0,
        ParameterSetName = parameterSetByProperty)]
    public Guid Id { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (ParameterSetName == parameterSetByProperty)
            SendRequest(new MemberDelete(Id));
        else
            SendRequest(new MemberDelete(Member!.Id));
    }
}
