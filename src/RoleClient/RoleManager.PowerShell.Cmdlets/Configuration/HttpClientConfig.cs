using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManager.PowerShell.Cmdlets.Configuration;

public static class HttpClientConfig
{
    public static void ConfigureHttpClient(this IWindsorContainer container)
    {
        container.Register(Component.For<HttpClient>());
    }
}
