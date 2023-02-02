using RoleManager.PowerShell.Requests;
using System.Management.Automation;

namespace RoleManager.PowerShell.Cmdlets;

[Cmdlet(VerbsCommon.Get, "Members")]
public sealed class GetMembers : Cmdlet
{
    protected override void ProcessRecord()
    {
        var task = Mediator.Send(new MemberQuery());
        task.Wait();
        WriteObject(task.Result, true);        
    }
}