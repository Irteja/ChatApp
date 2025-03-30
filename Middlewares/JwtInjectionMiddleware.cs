using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
public class JwtInjectionMiddleware
{
    private readonly RequestDelegate _next;

    public JwtInjectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Read token from cookies
        var token = context.Request.Cookies["jwtToken"];

        if (!string.IsNullOrEmpty(token))
        {
            // Add Authorization header with Bearer token
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        }

        await _next(context);
    }
}


public static class JwtInjectionMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtInjection(this IApplicationBuilder app)
    {
        return app.UseMiddleware<JwtInjectionMiddleware>();
    }
}