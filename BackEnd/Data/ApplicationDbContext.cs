using Microsoft.EntityFrameworkCore;
namespace BackEnd.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Crea un registro siempre y cuando el indice no tenga un nombre ya definido y sea único
        modelBuilder.Entity<Attendee>().HasIndex(a => a.UserName).IsUnique();

        //Many to Many Session y Attendee
        modelBuilder.Entity<SessionAttendee>().HasKey(ca => new { ca.SessionId, ca.AttendeeId });

        //Many to Many Sepaker y Session
        modelBuilder.Entity<SessionSpeaker>().HasKey(ss => new { ss.SessionId, ss.SpeakerId });

    }

    //This to avoid nullable warnings
    public DbSet<Attendee> Attendees => Set<Attendee>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Speaker> Speakers => Set<Speaker>();
    public DbSet<Track> Tracks => Set<Track>();
}
