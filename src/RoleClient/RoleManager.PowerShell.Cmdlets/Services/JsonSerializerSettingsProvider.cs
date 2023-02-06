using Newtonsoft.Json;
using RoleManager.PowerShell.Requests.Communication;
namespace RoleManager.PowerShell.Cmdlets.Services;

internal class JsonSerializerSettingsProvider : IJsonSerializerSettingsProvider
{
    readonly JsonSerializerSettings _settings = new();
    public JsonSerializerSettings Get() => _settings;
}
