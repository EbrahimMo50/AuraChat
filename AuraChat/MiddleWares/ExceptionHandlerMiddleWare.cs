using AuraChat.Exceptions;

namespace AuraChat.MiddleWares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (BadRequestException ex)
        {
            logger.LogError(ex, "bad request exception occurred.");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "not found request exception occurred.");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
        catch (ConflictException ex)
        {
            logger.LogError(ex, "conflicting exception occurred.");
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
        // generic exception handling 500s
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { Error = "An error occurred. Please try again later." });
        }
    }
}