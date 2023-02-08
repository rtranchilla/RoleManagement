using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.New, "Member", DefaultParameterSetName = parameterSetByProperty)]
[OutputType(typeof(Member))]
public sealed class MemberNew : RmCmdlet
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
        Mandatory = false,
        ValueFromPipeline = true,
        ParameterSetName = parameterSetByProperty)]
    public Guid? Id { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ParameterSetName = parameterSetByProperty)]
    public string? DisplayName { get; set; }

    [Parameter(
        Mandatory = true,
        Position = 1,
        ValueFromPipeline = true,
        ParameterSetName = parameterSetByProperty)]
    public string? UniqueName { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (ParameterSetName == parameterSetByProperty)
            Member = new Member(Id ?? Guid.NewGuid(), DisplayName!, UniqueName!);

        SendRequest(new MemberCreate(Member!));
    }
}
