using Laboratorio.Api.Exceptions;

namespace Laboratorio.Api.Middleware;

// Traduce las excepciones a respuestas HTTP con un mensaje legible.
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadRequestException ex)
        {
            await Write(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (NotFoundException ex)
        {
            await Write(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no controlado en {Method} {Path}", context.Request.Method, context.Request.Path);
            await Write(context, StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado en el servidor.");
        }
    }

    private static Task Write(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = status;
        return context.Response.WriteAsJsonAsync(new { message });
    }
}