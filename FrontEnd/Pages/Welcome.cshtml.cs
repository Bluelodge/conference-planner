using FrontEnd.Services;
using FrontEnd.Middleware;
using FrontEnd.Pages.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace FrontEnd.Pages
{
    [SkipWelcome]
    public class WelcomeModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public WelcomeModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Attendee Attendee { get; set; }

        public IActionResult OnGet()
        {
            var isAttendee = User.IsAttendee();

            //Redirect to home if user is anonymous or already registered as Attendee
            if (!User.Identity.IsAuthenticated || isAttendee)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var success = await _apiClient.AddAttendeeAsync(Attendee);

            if(!success)
            {
                ModelState.AddModelError("", "There was an issue creating the attendee for this user");
                return Page();
            }

            // Re-issue the auth cookie with the new IsAttendee claim
            User.MakeAttendee();
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, User);

            return RedirectToPage("/Index");
        }
    }
}
