using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel;
using Castle.Windsor;
using MediatR.Pipeline;
using MediatR;
using System.Diagnostics;
using RoleManager.PowerShell.Requests;

namespace RoleManager.PowerShell.Cmdlets.Configuration;

internal static class MediatRConfig
{
    //public static void ConfigureMediatR(this IWindsorContainer container)
    //{
    //    container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
    //    container.Kernel.AddHandlersFilter(new ContravariantFilter());

    //    container.RegisterMediatRServices(Classes.FromAssemblyContaining<MemberQuery>()); // Register Commands

    //    container.Register(Component.For<IMediator, IPublisher, ISender>().ImplementedBy<MediatR.Mediator>());

    //    container.Register(Component.For(typeof(IPipelineBehavior<,>)).ImplementedBy(typeof(RequestExceptionProcessorBehavior<,>)));
    //    container.Register(Component.For(typeof(IPipelineBehavior<,>)).ImplementedBy(typeof(RequestExceptionActionProcessorBehavior<,>)));

    //    container.Register(Component.For<ServiceFactory>().UsingFactoryMethod<ServiceFactory>(k => (type =>
    //    {
    //        var enumerableType = type
    //            .GetInterfaces()
    //            .Concat(new[] { type })
    //            .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

    //        var service = enumerableType?.GetGenericArguments()?[0];
    //        var resolvedType = enumerableType != null ? k.ResolveAll(service) : k.Resolve(type);
    //        var genericArguments = service?.GetGenericArguments();

    //        // Handle exceptions even using the base request types for IRequestExceptionHandler<,,>
    //        var isRequestExceptionHandler = service?.GetGenericTypeDefinition()
    //            ?.IsAssignableTo(typeof(IRequestExceptionHandler<,,>)) ?? false;
    //        if (isRequestExceptionHandler)
    //            return ResolveRequestExceptionHandler(k, type, service, resolvedType, genericArguments);

    //        // Handle exceptions even using the base request types for IRequestExceptionAction<,>
    //        var isRequestExceptionAction = service?.GetGenericTypeDefinition()
    //            ?.IsAssignableTo(typeof(IRequestExceptionAction<,>)) ?? false;
    //        if (isRequestExceptionAction)
    //            return ResolveRequestExceptionAction(k, type, service, resolvedType, genericArguments);

    //        return resolvedType;
    //    })));
    //}

    //private static IWindsorContainer RegisterMediatRServices(this IWindsorContainer container, FromAssemblyDescriptor assemblyDescriptor) =>
    //    container.Register(
    //        assemblyDescriptor.BasedOn(typeof(IRequestHandler<,>)).LifestyleTransient().WithServiceAllInterfaces().AllowMultipleMatches(),
    //        assemblyDescriptor.BasedOn(typeof(INotificationHandler<>)).LifestyleTransient().WithServiceAllInterfaces().AllowMultipleMatches(),
    //        assemblyDescriptor.BasedOn(typeof(IRequestExceptionAction<,>)).LifestyleTransient().WithServiceAllInterfaces().AllowMultipleMatches(),
    //        assemblyDescriptor.BasedOn(typeof(IRequestExceptionHandler<,,>)).LifestyleTransient().WithServiceAllInterfaces().AllowMultipleMatches(),
    //        assemblyDescriptor.BasedOn(typeof(IStreamRequestHandler<,>)).LifestyleTransient().WithServiceAllInterfaces().AllowMultipleMatches(),
    //        assemblyDescriptor.BasedOn(typeof(IRequestPreProcessor<>)).LifestyleTransient().WithServiceAllInterfaces().AllowMultipleMatches(),
    //        assemblyDescriptor.BasedOn(typeof(IRequestPostProcessor<,>)).LifestyleTransient().WithServiceAllInterfaces().AllowMultipleMatches()
    //        );

    //private static object ResolveRequestExceptionHandler(IKernel k, Type type, Type? service, object resolvedType, Type[]? genericArguments)
    //{
    //    if (service == null
    //    || genericArguments == null
    //    || !service.IsInterface
    //    || !service.IsGenericType
    //    || !service.IsConstructedGenericType
    //    || !(service.GetGenericTypeDefinition()
    //    ?.IsAssignableTo(typeof(IRequestExceptionHandler<,,>)) ?? false)
    //    || genericArguments.Length != 3)
    //    {
    //        return resolvedType;
    //    }

    //    var serviceFactory = k.Resolve<ServiceFactory>();
    //    var baseRequestType = genericArguments[0].BaseType;
    //    var response = genericArguments[1];
    //    var exceptionType = genericArguments[2];

    //    // Check if the base request type is valid
    //    if (baseRequestType == null
    //    || !baseRequestType.IsClass
    //    || baseRequestType == typeof(object)
    //    || ((!baseRequestType.GetInterfaces()
    //        ?.Any(i => i.IsAssignableFrom(typeof(IRequest<>)))) ?? true))
    //    {
    //        return resolvedType;
    //    }

    //    var exceptionHandlerInterfaceType = typeof(IRequestExceptionHandler<,,>).MakeGenericType(baseRequestType, response, exceptionType);
    //    var enumerableExceptionHandlerInterfaceType = typeof(IEnumerable<>).MakeGenericType(exceptionHandlerInterfaceType);
    //    Array resultArray = CreateArraysOutOfResolvedTypeAndEnumerableInterfaceTypes(type, resolvedType, serviceFactory, enumerableExceptionHandlerInterfaceType);

    //    return resultArray;
    //}

    //private static object ResolveRequestExceptionAction(IKernel k, Type type, Type? service, object resolvedType, Type[]? genericArguments)
    //{
    //    if (service == null
    //    || genericArguments == null
    //    || !service.IsInterface
    //    || !service.IsGenericType
    //    || !service.IsConstructedGenericType
    //    || !(service.GetGenericTypeDefinition()
    //    ?.IsAssignableTo(typeof(IRequestExceptionAction<,>)) ?? false)
    //    || genericArguments.Length != 2)
    //    {
    //        return resolvedType;
    //    }

    //    var serviceFactory = k.Resolve<ServiceFactory>();
    //    var baseRequestType = genericArguments[0].BaseType;
    //    var exceptionType = genericArguments[1];

    //    // Check if the base request type is valid
    //    if (baseRequestType == null
    //    || !baseRequestType.IsClass
    //    || baseRequestType == typeof(object)
    //    || ((!baseRequestType.GetInterfaces()
    //        ?.Any(i => i.IsAssignableFrom(typeof(IRequest<>)))) ?? true))
    //    {
    //        return resolvedType;
    //    }

    //    var exceptionHandlerInterfaceType = typeof(IRequestExceptionAction<,>).MakeGenericType(baseRequestType, exceptionType);
    //    var enumerableExceptionHandlerInterfaceType = typeof(IEnumerable<>).MakeGenericType(exceptionHandlerInterfaceType);
    //    Array resultArray = CreateArraysOutOfResolvedTypeAndEnumerableInterfaceTypes(type, resolvedType, serviceFactory, enumerableExceptionHandlerInterfaceType);

    //    return resultArray;
    //}

    //private static Array CreateArraysOutOfResolvedTypeAndEnumerableInterfaceTypes(Type type, object resolvedType, ServiceFactory serviceFactory, Type enumerableExceptionHandlerInterfaceType)
    //{
    //    var firstArray = serviceFactory.Invoke(enumerableExceptionHandlerInterfaceType) as Array;
    //    Debug.Assert(firstArray != null, $"Array '{nameof(firstArray)}' should not be null because this method calls ResolveAll when a {typeof(IEnumerable<>).FullName} " +
    //        $"is passed as argument in argument named '{nameof(type)}'");

    //    var secondArray = resolvedType is Array ? resolvedType as Array : new[] { resolvedType };
    //    Debug.Assert(secondArray != null, $"Array '{nameof(secondArray)}' should not be null because '{nameof(resolvedType)}' is an array or created as an array");

    //    var resultArray = Array.CreateInstance(typeof(object), firstArray.Length + secondArray.Length);
    //    Array.Copy(firstArray, resultArray, firstArray.Length);
    //    Array.Copy(secondArray, 0, resultArray, firstArray.Length, secondArray.Length);
    //    return resultArray;
    //}
}
