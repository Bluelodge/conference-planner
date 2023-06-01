using ConferenceDTO;

namespace FrontEnd.Services;

public interface IApiClient
{
    // Session
    Task<List<SessionResponse>> GetSessionsAsync();
    Task<SessionResponse?> GetSessionAsync(int id);
    Task PutSessionAsync(Session session);
    Task DeleteSessionAsync(int id);

    // Speaker
    Task<List<SpeakerResponse>> GetSpeakerAsync();
    Task<SpeakerResponse?> GetSpeakerAsync(int id);

    // Attendee
    Task<bool> AddAttendeeAsync(Attendee attendee);
    Task<AttendeeResponse?> GetAttendeeAsync(string name);
    Task<List<SessionResponse>> GetSessionsByAttendeeAsync(string name);
    Task AddSessionToAttendeeAsync(string name, int sessionId);
    Task RemoveSessionFromAttendeeAsync(string name, int sessionId);

    // Search
    Task<List<SearchResult>> SearchAsync(string term);

    // Health
    Task<bool> CheckHealthAsync();

}
