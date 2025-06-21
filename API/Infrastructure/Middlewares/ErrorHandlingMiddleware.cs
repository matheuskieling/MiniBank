using System.Net;
using System.Text.Json;
using API.Models.DTO;

namespace API.Infrastructure.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                await HandleUntreatedErrorAsync(context);
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private static Task HandleUntreatedErrorAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var errorResponse = new ErrorResponse("An unexpected error occurred", StatusCodes.Status500InternalServerError);
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        return context.Response.WriteAsync(jsonResponse);
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var errorResponse = new ErrorResponse("An unexpected error occurred", StatusCodes.Status500InternalServerError);
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        return context.Response.WriteAsync(jsonResponse);
    }
}