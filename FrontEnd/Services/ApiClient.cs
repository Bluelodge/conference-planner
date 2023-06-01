using System.Formats.Asn1;
using System.Net;
using ConferenceDTO;

namespace FrontEnd.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Attendee
    public async Task<bool> AddAttendeeAsync(Attendee attendee)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/Attendee", attendee);
        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<AttendeeResponse?> GetAttendeeAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        var response = await _httpClient.GetAsync($"/api/Attendee/{name}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AttendeeResponse>();
    }

    public async Task AddSessionToAttendeeAsync(string name, int sessionId)
    {
        var response = await _httpClient.PostAsync($"/api/attendee/{name}/session/{sessionId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveSessionFromAttendeeAsync(string name, int sessionId)
    {
        var response = await _httpClient.DeleteAsync($"/api/attendee/{name}/session/{sessionId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<SessionResponse>> GetSessionsByAttendeeAsync(string name)
    {
        var response = await _httpClient.GetAsync($"/api/attendee/{name}/sessions");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<SessionResponse>>();
    }

    // Session
    public async Task<List<SessionResponse>> GetSessionsAsync()
    {
        var response = await _httpClient.GetAsync("/api/Session");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<SessionResponse>>() ?? new();
    }

    public async Task<SessionResponse?> GetSessionAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/Session/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SessionResponse>();
    }

    public async Task PutSessionAsync(Session session)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/Session/{session.Id}", session);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteSessionAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/Session/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return;
        }

        response.EnsureSuccessStatusCode();
    }

    // Speaker
    public async Task<List<SpeakerResponse>> GetSpeakerAsync()
    {
        var response = await _httpClient.GetAsync("api/Speaker");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<SpeakerResponse>>() ?? new();
    }

    public async Task<SpeakerResponse?> GetSpeakerAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/Speaker/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SpeakerResponse>();
    }

    // Search
    public async Task<List<SearchResult>> SearchAsync(string term)
    {
        var response = await _httpClient.GetAsync($"/api/Search/{term}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<SearchResult>>() ?? new();
    }

    // Health
    public async Task<bool> CheckHealthAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync("/health");

            return string.Equals(response, "Healthy", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

}
