using Castle.Windsor;
using Microsoft.EntityFrameworkCore;
using RoleManagement.RoleManagementService.DataPersistence;
using RoleManagement.RoleManagementService.MapperConfig;
using RoleManagement.RoleManagementService.Web.Configuration;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);
var container = new WindsorContainer();
builder.Host.UseWindsorContainerServiceProvider(container);

// Add services to the container.
builder.Services.AddDaprClient(); // Adds dapr client for consumption
builder.Services.AddControllers().AddDapr(); // Adds dapr integration for controlers
builder.Services.AddDbContext<RoleDbContext>(opt => 
    //opt.UseSqlServer("Server=localhost;Database=roleManagement;User Id=testuser;Password=testuser1;TrustServerCertificate=True"),
    opt.UseSqlServer("Server=SQL1\\MSSQLSERVER2;Database=RoleManagement;Trusted_Connection=Yes;TrustServerCertificate=True"),
    ServiceLifetime.Scoped, ServiceLifetime.Scoped);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MemberProfile).Assembly);

var app = builder.Build();

container.ConfigureMediatR();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCloudEvents(); // Adds event metadata for dapr pubsub
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler(); // Adds dapr pubsub subscribe endpoints
    endpoints.MapControllers();
});

app.Run();
