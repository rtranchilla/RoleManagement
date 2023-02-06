using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MediatR;
using RoleManager.PowerShell.Cmdlets.Configuration;
using RoleManager.PowerShell.Requests.Communication;

namespace RoleManager.PowerShell.Cmdlets.Services;

internal static class Mediator
{
    static readonly IWindsorContainer container;
    static Mediator()
    {
        container = new WindsorContainer();
        container.ConfigureMediatR();
        container.ConfigureHttpClient();
        container.ConfigureAutoMapper();
        container.Register(Component.For<IHttpClientProvider>().ImplementedBy<HttpClientProvider>());
        container.Register(Component.For<IJsonSerializerSettingsProvider>().ImplementedBy<JsonSerializerSettingsProvider>());
        mediator = container.Resolve<IMediator>();
    }

    private static readonly IMediator mediator;

    public static Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) =>
        mediator.Send(request, cancellationToken);
}
