using Aeternum.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Aeternum.Middlewares.Authorization
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    // Resolve UserManager from the service provider
                    var userManager = context.RequestServices.GetService<UserManager<ApplicationUser>>();

                    if (userManager == null)
                    {
                        // Handle the case where UserManager could not be resolved
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync("Interní chyba serveru: UserManager není k dispozici.");
                        return;
                    }

                    var user = await userManager.GetUserAsync(context.User);

                    // Check if user is not found or is blocked
                    if (user == null || user.IsBlocked)
                    {
                        await RespondWithForbidden(context, "Přístup byl zamítnut. Účet je blokován.");
                        return;
                    }

                    // Get required role or claim for the endpoint
                    var requiredRole = context.GetEndpoint()?.Metadata.GetMetadata<RequiredRoleAttribute>()?.RoleName;
                    var requiredClaim = context.GetEndpoint()?.Metadata.GetMetadata<RequiredClaimAttribute>();

                    // Check role
                    if (!string.IsNullOrEmpty(requiredRole) && !await userManager.IsInRoleAsync(user, requiredRole))
                    {
                        await RespondWithForbidden(context, "Nedostatečná oprávnění (role).");
                        return;
                    }

                    // Check claim
                    if (requiredClaim != null && !context.User.HasClaim(c => c.Type == requiredClaim.Type && c.Value == requiredClaim.Value))
                    {
                        await RespondWithForbidden(context, "Nedostatečná oprávnění (claim).");
                        return;
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception (ideálně použijte logovací knihovnu)
                Console.WriteLine($"Chyba v autorizačním middleware: {ex.Message}");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Interní chyba serveru.");
            }
        }

        private async Task RespondWithForbidden(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(message);
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class RequiredRoleAttribute : Attribute
        {
            public string RoleName { get; }
            public RequiredRoleAttribute(string roleName) => RoleName = roleName;
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class RequiredClaimAttribute : Attribute
        {
            public string Type { get; }
            public string Value { get; }

            public RequiredClaimAttribute(string type, string value)
            {
                Type = type;
                Value = value;
            }
        }
    }
}
