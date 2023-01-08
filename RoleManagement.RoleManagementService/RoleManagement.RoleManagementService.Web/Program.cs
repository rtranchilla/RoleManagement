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
builder.Services.AddControllers();
builder.Services.AddDbContext<RoleDbContext>(opt => opt.UseSqlServer("Server=localhost;Database=roleManagement;User Id=testuser;Password=testuser1;TrustServerCertificate=True"), ServiceLifetime.Transient, ServiceLifetime.Transient);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
