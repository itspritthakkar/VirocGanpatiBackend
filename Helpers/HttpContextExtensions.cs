namespace VirocGanpati.Helpers
{
    public static class HttpContextExtensions
    {
        public static int? GetLoggedInUserId(this HttpContext context)
        {
            if (context.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is int userId)
            {
                return userId;
            }
            return null; // No user id found
        }

        public static string? GetLoggedInUserEmail(this HttpContext context)
        {
            if (context.Items.TryGetValue("UserEmail", out var emailObj) && emailObj is string email)
            {
                return email;
            }
            return null;
        }

        public static string? GetLoggedInUserRole(this HttpContext context)
        {
            if (context.Items.TryGetValue("UserRole", out var roleObj) && roleObj is string role)
            {
                return role;
            }
            return null;
        }
    }
}
