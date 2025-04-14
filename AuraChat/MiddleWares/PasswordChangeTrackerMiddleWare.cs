using AuraChat.Entities;
using AuraChat.Exceptions;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Text.Json;

namespace AuraChat.MiddleWares;

public class PasswordChangeTrackerMiddleWare(
    RequestDelegate next, 
    IServiceProvider serviceProvider,
    IStringLocalizer<PasswordChangeTrackerMiddleWare> stringLocalizer)

{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirstValue("Id")!;

        var passwordTrackClaim = httpContext.User.FindFirstValue("PCT") 
            ?? throw new BadRequestException(stringLocalizer["password change tracker claim not found"]);

        User user;
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>()!;
            user = dbContext.Users.FirstOrDefault(u => u.Id == int.Parse(userIdClaim))!;
        }

        if (user.UserSettings.PasswordChangeCounter == int.Parse(passwordTrackClaim))
            await next.Invoke(httpContext);

        httpContext.Response.StatusCode = 401;
        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(
                new { Message = stringLocalizer["Password has been changed please relog"] }
            )
        );
    }
}
