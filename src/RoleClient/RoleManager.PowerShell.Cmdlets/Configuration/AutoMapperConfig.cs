using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using RoleManager.PowerShell.Mapper;

namespace RoleManager.PowerShell.Cmdlets.Configuration;

internal static class AutoMapperConfig
{
    public static void ConfigureAutoMapper(this IWindsorContainer container)
    {
        MapperConfiguration configuration;

        configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(MemberProfile).Assembly);
        });

        var mapper = configuration.CreateMapper();

        container.Register(Component.For<IMapper>().Instance(mapper));
    }
}
