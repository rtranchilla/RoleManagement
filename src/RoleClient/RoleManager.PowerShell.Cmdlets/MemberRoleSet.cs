using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Set, "MemberRole", DefaultParameterSetName = parameterSetById)]
public sealed class MemberRoleSet : RmCmdlet
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
    public Role? Role { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 0,
        ParameterSetName = parameterSetById)]
    public Guid MemberId { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 1,
        ParameterSetName = parameterSetById)]
    public Guid RoleId { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 2,
        ParameterSetName = parameterSetById)]
    public Guid TreeId { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (ParameterSetName == parameterSetById)
            SendRequest(new MemberRoleUpdate(MemberId, RoleId, TreeId));
        else
            SendRequest(new MemberRoleUpdate(Member!.Id, Role!.Id, Role.TreeId));
    }
}
