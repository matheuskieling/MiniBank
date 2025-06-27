using System.Reflection;
using Core.FluentMigrator;
using Core.Swagger;
using Identity.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.SetAssembly(Assembly.GetExecutingAssembly());
if (builder.Environment.EnvironmentName != "IntegrationTest")
{
    builder.Services.AddFluentMigrator(builder.Configuration, "identity");
}
builder.Services.SwaggerConfigInit();
builder.Services.InitDependencyInjection(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseInitSwagger();
}

app.UseHttpsRedirection();
app.UseMigrations();
app.MapControllers();

app.Run();

public partial class IdentityProgram { }