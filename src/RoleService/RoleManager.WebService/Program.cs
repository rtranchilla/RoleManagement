using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Google.Api;
using Microsoft.EntityFrameworkCore;
using RoleManager.Commands;
using RoleManager.DataPersistence;
using RoleManager.Events;
using RoleManager.MapperConfig;
using RoleManager.Queries;
using RoleManager.WebService.Configuration;
using System.ComponentModel;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
//var container = new WindsorContainer();
//builder.Host.UseWindsorContainerServiceProvider(container);

// Add services to the container.
builder.Services.AddDaprClient(); // Adds dapr client for consumption
builder.Services.AddControllers().AddDapr(); // Adds dapr integration for controlers
builder.Services.AddDbContext<RoleDbContext>(opt =>
    opt.UseSqlServer("Server=rolemanager-mssql;Initial Catalog=RoleManagement2;User Id=sa;Password=MyPass@word;Persist Security Info=False;Encrypt=False"),
    //opt.UseSqlServer("Server=localhost,1439;Initial Catalog=RoleManagement2;User Id=sa;Password=MyPass@word;Persist Security Info=False;Encrypt=False"),
    ServiceLifetime.Scoped, ServiceLifetime.Scoped);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MemberProfile).Assembly);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<MemberCreate>();
    cfg.RegisterServicesFromAssemblyContaining<MemberQuery>();
    cfg.RegisterServicesFromAssemblyContaining<MemberCreated>();
});

var app = builder.Build();

app.Services.UpdateDatabase();
//container.UpdateDatabase();
//container.ConfigureMediatR();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCloudEvents(); // Adds event metadata for dapr pubsub
app.UseAuthorization();

app.MapControllers();

app.Run();
