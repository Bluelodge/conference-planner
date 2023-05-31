using BackEnd.Data;
using ConferenceDTO;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Search/{term}", async (string term, ApplicationDbContext db) =>
        {
            var sessionResults = await db.Sessions.Include(s => s.Track)
                                    .Include(ss => ss.SessionSpeakers)
                                    .ThenInclude(s => s.Speaker)
                                    .Where(ss =>
                                        ss.Title!.Contains(term) ||
                                        ss.Track!.Name!.Contains(term)
                                     )
                                    .ToListAsync();

            var speakerResults = await db.Speakers.Include(ss => ss.SessionSpeakers)
                                    .ThenInclude(s => s.Session)
                                    .Where(ss =>
                                        ss.Name!.Contains(term) ||
                                        ss.Bio!.Contains(term) ||
                                        ss.WebSite!.Contains(term)
                                    )
                                    .ToListAsync();

            var results = sessionResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Session,
                Session = s.MapSessionResponse()
            })
            .Concat(speakerResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Speaker,
                Speaker = s.MapSpeakerResponse()
            }));

            return results
                is IEnumerable<SearchResult> model
                    ? Results.Ok(model)
                    : Results.NotFound();

        })
        .WithTags("Search")
        .WithName("GetSearchResults")
        .Produces<IEnumerable<SearchResult>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}

