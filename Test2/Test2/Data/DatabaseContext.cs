using Test2.Models;

namespace Test2.Data;

using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DbSet<Racer> Racers { get; set; }
    public DbSet<Race> Races { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<TrackRace> TrackRaces { get; set; }
    public DbSet<RaceParticipation> RaceParticipations { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Racer>().HasData(new Racer
        {
            RacerId = 1,
            FirstName = "Lewis",
            LastName = "Hamilton"
        });

        modelBuilder.Entity<Race>().HasData(new Race
        {
            RaceId = 1,
            Name = "British Grand Prix",
            Location = "Silverstone, UK",
            Date = new DateTime(2025, 7, 14)
        });

        modelBuilder.Entity<Track>().HasData(new Track
        {
            TrackId = 1,
            Name = "Silverstone Circuit",
            LengthInKm = 5.89m
        });

        modelBuilder.Entity<TrackRace>().HasData(new TrackRace
        {
            TrackRaceId = 1,
            TrackId = 1,
            RaceId = 1,
            Laps = 52,
            BestTimeInSeconds = null
        });

        modelBuilder.Entity<RaceParticipation>().HasData(new RaceParticipation
        {
            TrackRaceId = 1,
            RacerId = 1,
            FinishTimeInSeconds = 5460,
            Position = 1
        });
    }
}