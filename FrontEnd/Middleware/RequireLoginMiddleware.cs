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

        if (context.User.Identity.IsAuthenticated && endpoint?.Metadata.GetMetadata<SkipWelcomeAttribute>() == null)
        {
            var isAttendee = context.User.IsAttendee();

            if (!isAttendee)
            {
                var url = _linkGenerator.GetUriByPage(context, page: "/Welcome");
                // No attendee registered for this user
                context.Response.Redirect(url);

                return Task.CompletedTask;
            }

        }

        return _next(context);
    }
}
