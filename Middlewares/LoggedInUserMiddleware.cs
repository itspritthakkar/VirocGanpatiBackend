using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using System.IdentityModel.Tokens.Jwt;

namespace VirocGanpati.Middlewares
{
    public class LoggedInUserMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggedInUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                // Resolve ApplicationDbContext from the scoped request
                using var scope = context.RequestServices.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Extract the email (from JwtRegisteredClaimNames.Sub)
                var email = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                // Extract the userId (custom claim "userId")
                var userId = Int32.Parse(context.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);

                // Extract the role (custom claim "role")
                var role = context.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                // Check if the user exists and status is active
                var user = await dbContext.Users
                    .Where(u => u.UserId == userId && u.Status == Models.ActiveInactiveStatus.Active)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Invalid credentials or user inactive" });
                    return; // Stop the middleware pipeline
                }

                if (!user.IsMobileVerified && !context.Request.Path.StartsWithSegments("/api/Otp") && !context.Request.Path.StartsWithSegments("/api/User/me"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new { message = "Mobile number not verified" });
                    return;
                }

                // Store the extracted data in HttpContext.Items
                context.Items["UserId"] = userId;
                context.Items["UserEmail"] = email;
                context.Items["UserRole"] = role;  // Store the single role as a string
            }

            await _next(context);
        }
    }
}
