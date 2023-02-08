using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Set, "Role")]
[OutputType(typeof(Role))]
public sealed class RoleSet : RmCmdlet
{
    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true)]
    public Role? Role { get; set; }

    [Parameter(
        Mandatory = false,
        Position = 1)]
    public bool? Reversible { get; set; }

    [Parameter(
        Mandatory = false,
        Position = 2)]
    public Guid[]? RequiredNodes { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (Reversible != null)
            Role!.Reversible = (bool)Reversible;
        if (RequiredNodes != null)
            Role!.RequiredNodes = RequiredNodes;
        SendRequest(new RoleUpdate(Role!));
        WriteObject(Role);
    }
}
