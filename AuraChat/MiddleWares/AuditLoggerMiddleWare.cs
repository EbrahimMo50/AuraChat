using AuraChat.Entities;
using System.Security.Claims;

namespace AuraChat.MiddleWares;

public class AuditLoggerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var dbContext = context.RequestServices.GetRequiredService<AppDbContext>();
        int? RequesterIdClaim = Convert.ToInt32(context.User.FindFirstValue("Id"));

        // default value returned for the Id claim is zero although NULL is more valid to register
        if (RequesterIdClaim == 0)
            RequesterIdClaim = null;

        await dbContext.Audits.AddAsync(new Audit() { AccessTime = DateTime.UtcNow, RequesterId = RequesterIdClaim, EndPointPath = context.Request.Path });
        await dbContext.SaveChangesAsync();
        await next.Invoke(context);
    }
}
