using System.Security.Claims;

namespace FrontEnd.Middleware;

public class RequireLoginMiddleware
{
    private readonly RequestDelegate _next;
    private readonly LinkGenerator _linkGenerator;

    public RequireLoginMiddleware(RequestDelegate next, LinkGenerator linkGenerator)
    {
        _next = next;
        _linkGenerator = linkGenerator;
    }

    public Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var isAdmin = context.User.IsAdmin();

        // User authenticated but not an Attendee
        // Admin is not required to make itself an Attendee
        // For pages that aren't marked to skip attendee welcome
        if (context.User.Identity.IsAuthenticated
            && !isAdmin
            && endpoint?.Metadata.GetMetadata<SkipWelcomeAttribute>() == null)
        {
            var isAttendee = context.User.IsAttendee();

            // No attendee registered for this user
            if (!isAttendee)
            {
                // Redirect to Welcome
                var url = _linkGenerator.GetUriByPage(context, page: "/Welcome");
                context.Response.Redirect(url);

                return Task.CompletedTask;
            }
        }

        return _next(context);
    }
}
