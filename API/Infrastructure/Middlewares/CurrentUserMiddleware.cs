using System.Security.Claims;
using API.Models;
using API.Services.Interfaces;

namespace API.Infrastructure.Middlewares;

public class CurrentUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = context.User.FindFirst(ClaimTypes.Name)?.Value;

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(username))
            {
                var currentUserService = serviceProvider.GetRequiredService<ICurrentUserService>();
                currentUserService.CurrentUser = new CurrentUser
                {
                    UserId = Guid.Parse(userId),
                    Username = username
                };
            }
        }

        await next(context);
    }
}