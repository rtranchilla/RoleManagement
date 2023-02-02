using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel;
using Castle.Windsor;
using MediatR;
using MediatR.Pipeline;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using RoleManager.PowerShell.Cmdlets.Configuration;

namespace RoleManager.PowerShell.Cmdlets;

internal static class Mediator
{
    static readonly IWindsorContainer container;
    static Mediator()
    {
        container = new WindsorContainer();
        container.ConfigureMediatR();
        container.ConfigureHttpClient();
        container.ConfigureAutoMapper();
        mediator = container.Resolve<IMediator>();
    }

    private static IMediator mediator;

    public static Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) => 
        mediator.Send(request, cancellationToken);
}
