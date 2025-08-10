using System.Text.RegularExpressions;
using VirocGanpati.Middlewares;

namespace VirocGanpati.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        app.MapWhen(context =>
        {
            var isNotLogin = !context.Request.Path.StartsWithSegments("/api/Auth/login");

            var isNotGetRecordsById = !(
                Regex.IsMatch(context.Request.Path.Value ?? "", @"^/api/Records/\d+$") ||
                Regex.IsMatch(context.Request.Path.Value ?? "", @"^/api/\w+/Records/\d+$")
            ) || !context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase);

            return isNotLogin && isNotGetRecordsById;
        },
        appBuilder =>
        {
            appBuilder.UseMiddleware<LoggedInUserMiddleware>();
            appBuilder.UseRouting();
            appBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
        });

        return app;
    }
}
