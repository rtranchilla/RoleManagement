using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Microsoft.EntityFrameworkCore;
using RoleManager.DataPersistence;
using RoleManager.MapperConfig;
using RoleManager.WebService.Configuration;
using System.ComponentModel;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var container = new WindsorContainer();
builder.Host.UseWindsorContainerServiceProvider(container);

// Add services to the container.
builder.Services.AddDaprClient(); // Adds dapr client for consumption
builder.Services.AddControllers().AddDapr(); // Adds dapr integration for controlers
builder.Services.AddDbContext<RoleDbContext>(opt =>
    opt.UseSqlServer("Server=rolemanager-mssql;Initial Catalog=RoleManagement2;User Id=sa;Password=MyPass@word;"),
    ServiceLifetime.Scoped, ServiceLifetime.Scoped);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MemberProfile).Assembly);

var app = builder.Build();

container.UpdateDatabase();
container.ConfigureMediatR();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCloudEvents(); // Adds event metadata for dapr pubsub
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    //endpoints.MapSubscribeHandler(); // Adds dapr pubsub subscribe endpoints
    endpoints.MapControllers();
});

app.Run();
