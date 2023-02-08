using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Remove, "MemberRole", DefaultParameterSetName = parameterSetById)]
public sealed class MemberRoleRemove : RmCmdlet
{
    const string parameterSetByObject = "ByObject";
    const string parameterSetById = "ById";

    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ParameterSetName = parameterSetByObject)]
    public Member? Member { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 1,
        ValueFromPipeline = true,
        ParameterSetName = parameterSetByObject)]
    public Tree? Tree { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 0,
        ParameterSetName = parameterSetById)]
    public Guid MemberId { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 1,
        ParameterSetName = parameterSetById)]
    public Guid TreeId { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (ParameterSetName == parameterSetById)
            SendRequest(new MemberRoleDelete(MemberId, TreeId));
        else
            SendRequest(new MemberRoleDelete(Member!.Id, Tree!.Id));
    }
}
