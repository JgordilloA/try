using Laboratorio.Api.Interfaces;
using Laboratorio.Api.Middleware;
using Laboratorio.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ILoginService, LoginService>();

const string CorsPolicy = "Frontend";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Orden importante: el manejo de errores va después de CORS para que
// las respuestas de error también incluyan las cabeceras CORS.
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseCors(CorsPolicy);
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();