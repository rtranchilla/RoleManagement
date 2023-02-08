using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Set, "Member")]
[OutputType(typeof(Member))]
public sealed class MemberSet : RmCmdlet
{
    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true)]
    public Member? Member { get; set; }

    [Parameter(
        Mandatory = false,
        Position = 1)]
    public string? DisplayName { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (DisplayName != null)
            Member!.DisplayName = DisplayName;
        SendRequest(new MemberUpdate(Member!));
        WriteObject(Member);
    }
}
