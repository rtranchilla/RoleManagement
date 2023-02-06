using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Get, "Trees")]
[OutputType(typeof(Tree))]
public sealed class GetTrees : RmCmdlet
{
    [Parameter(Position = 0)]
    public string? Identifier { get; set; }

    protected override void BeginProcessingErrorHandling()
    {
        if (Guid.TryParse(Identifier, out var id))
            WriteObject(SendRequest(new TreeQuery { Id = id }), true);
        else if (!string.IsNullOrEmpty(Identifier))
            WriteObject(SendRequest(new TreeQuery { Name = Identifier }), true);
        else
            WriteObject(SendRequest(new TreeQuery()), true);
    }
}