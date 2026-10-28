namespace Laboratorio.Api.Middleware;

// Imprime en consola la ruta y el método HTTP de cada solicitud.
public class RequestLoggingMiddleware(RequestDelegate next)
{
    public Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {context.Request.Method} {context.Request.Path}");
        return next(context);
    }
}