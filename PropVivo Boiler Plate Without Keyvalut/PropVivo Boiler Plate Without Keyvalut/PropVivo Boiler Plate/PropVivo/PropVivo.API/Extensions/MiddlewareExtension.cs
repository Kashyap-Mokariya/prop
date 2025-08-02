using PropVivo.API.Extensions.Middleware;

namespace PropVivo.API.Extensions
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder AddMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<LocalApiMiddleware>();
            app.UseMiddleware<IpAddressMiddleware>();
            return app;
        }
    }
}