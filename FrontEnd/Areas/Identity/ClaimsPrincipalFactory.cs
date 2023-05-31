﻿using FrontEnd.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FrontEnd.Areas.Identity;

public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    private readonly IApiClient _apiClient;

    public ClaimsPrincipalFactory(IApiClient apiClient, UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    {
        _apiClient = apiClient;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        // Checks for Admin
        if (user.IsAdmin)
        {
            identity.MakeAdmin();
        }
        // The rest are attendees
        var attendee = await _apiClient.GetAttendeeAsync(user.UserName);
        if (attendee != null)
        {
            identity.MakeAttendee();
        }

        return identity;
    }
}
