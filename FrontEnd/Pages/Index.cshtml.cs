using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FrontEnd.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    // Protected so it can be used by MyAgenda
    protected readonly IApiClient _apiClient;

    public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; } = null!;
    public IEnumerable<(int Offset, DayOfWeek? DayofWeek)> DayOffsets { get; set; } = null!;
    public int CurrentDayOffset { get; set; }


    public IndexModel(ILogger<IndexModel> logger, IApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    // Task to use here and in MyAgenda
    protected virtual Task<List<SessionResponse>> GetSessionsAsync()
    {
        return _apiClient.GetSessionsAsync();
    }

    public bool IsAdmin { get; set; }

    [TempData]
    public string? Message { get; set; }
    public bool ShowMessage => !string.IsNullOrEmpty(Message);
    public List<int> UserSessions { get; set; } = new List<int>();

    // Obtener datos
    public async Task OnGetAsync(int day = 0)
    {
        IsAdmin = User.IsAdmin();
        // Inicializa el día actual
        CurrentDayOffset = day;
        // 
        if (User.Identity.IsAuthenticated)
        {
            var userSessions = await _apiClient.GetSessionsByAttendeeAsync(User.Identity.Name);
            UserSessions = userSessions.Select(u => u.Id).ToList();
        }
        // Obtiene la petición de Session
        var sessions = await GetSessionsAsync();
        // Primer fecha de la lista
        var startDate = sessions.Min(s => s.StartTime?.Date);
        // Lista de fechas, calcula el día según la fecha inicial 
        DayOffsets = sessions.Select(s => s.StartTime?.Date)
                             .Distinct()
                             .OrderBy(d => d)
                             .Select(day => ((int)Math.Floor((day!.Value - startDate)?.TotalDays ?? 0), day?.DayOfWeek))
                             .ToList();
        // Valor de día seleccionado en filtro
        var filterDate = startDate?.AddDays(day);
        // Obtiene las sesiones de acuerdo al día seleccionado
        Sessions = sessions.Where(s => s.StartTime?.Date == filterDate)
                           .OrderBy(s => s.TrackId)
                           .GroupBy(s => s.StartTime)
                           .OrderBy(g => g.Key);
    }

    // Add session to agenda
    public async Task<IActionResult> OnPostAsync(int sessionId)
    {
        await _apiClient.AddSessionToAttendeeAsync(User.Identity.Name, sessionId);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveAsync(int sessionId)
    {
        await _apiClient.RemoveSessionFromAttendeeAsync(User.Identity.Name, sessionId);

        return RedirectToPage();
    }

}
