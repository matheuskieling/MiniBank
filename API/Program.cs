using System.Reflection;
using API.Infrastructure;
using API.Infrastructure.Middlewares;
using Core.FluentMigrator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Core.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.SwaggerConfigInit();
builder.Services.InitDependencyInjection(builder.Configuration);
builder.Services.AddFluentMigrator(builder.Configuration, Assembly.GetExecutingAssembly(), "bank");
builder.Services.ConfigureJwt(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseInitSwagger();
}

app.UseHttpsRedirection();
app.UseMigrations();
app.ConfigureJwt();
app.UseMiddleware<CurrentUserMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();
app.Run();