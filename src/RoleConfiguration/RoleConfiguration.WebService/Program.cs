using Castle.Windsor;
using Microsoft.EntityFrameworkCore;
using RoleConfiguration.Commands;
using RoleConfiguration.DataPersistence;
using RoleConfiguration.Mapper;
using RoleConfiguration.Queries;
using RoleConfiguration.WebService.Configuration;

var builder = WebApplication.CreateBuilder(args);
var container = new WindsorContainer();
builder.Host.UseWindsorContainerServiceProvider(container);

// Add services to the container.
builder.Services.AddDaprClient(); // Adds dapr client for consumption
builder.Services.AddControllers().AddDapr(); // Adds dapr integration for controlers
builder.Services.AddDbContext<ConfigDbContext>(opt =>
    opt.UseSqlServer("Server=rolemanager-mssql;Initial Catalog=RoleConfig;User Id=sa;Password=MyPass@word;"),
    //opt.UseSqlServer("Server=localhost,1439;Initial Catalog=RoleConfig;User Id=sa;Password=MyPass@word;"),
    ServiceLifetime.Scoped, ServiceLifetime.Scoped);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});
builder.Services.ConfigureJsonSettings();
builder.Services.AddAutoMapper(typeof(MemberProfile).Assembly);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<MemberFileUpdate>();
    cfg.RegisterServicesFromAssemblyContaining<MemberFileQuery>();
});

var app = builder.Build();

app.Services.UpdateDatabase();
//container.UpdateDatabase();
container.ConfigureRoleManagerRepositories();
//container.ConfigureMediatR();
container.ConfigureYamlSerialization();
container.ConfigureRoleManagerHttpClient();
//container.ConfigureJsonSettings();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
