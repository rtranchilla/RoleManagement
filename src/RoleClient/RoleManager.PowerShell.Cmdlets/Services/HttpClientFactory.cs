using RoleManager.PowerShell.Requests.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManager.PowerShell.Cmdlets.Services;

internal class HttpClientProvider : IHttpClientProvider
{
    private readonly HttpClient _httpClient = new();

    public HttpClient Get() => _httpClient;
}
