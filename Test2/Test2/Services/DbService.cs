using Microsoft.EntityFrameworkCore;
using Test2.Data;
using Test2.DTOs;
using Test2.Exceptions;
using Test2.Models;

namespace Test2.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<GetRacerDto> GetRacer(int id)
    {
        var racer = await _context.Racers
            .Include(r => r.RaceParticipations)
            .ThenInclude(rp => rp.TrackRace)
            .ThenInclude(tr => tr.Race)
            .Include(r => r.RaceParticipations)
            .ThenInclude(rp => rp.TrackRace)
            .ThenInclude(tr => tr.Track)
            .FirstOrDefaultAsync(r => r.RacerId == id);

        if (racer == null)
            throw new NotFoundException("Racer not found");

        return new GetRacerDto
        {
            RacerId = racer.RacerId,
            FirstName = racer.FirstName,
            LastName = racer.LastName,
            Participations = racer.RaceParticipations.Select(rp => new ParticipationDto
            {
                FinishTimeInSeconds = rp.FinishTimeInSeconds,
                Position = rp.Position,
                Laps = rp.TrackRace.Laps,
                Race = new RaceDto
                {
                    Name = rp.TrackRace.Race.Name,
                    Location = rp.TrackRace.Race.Location,
                    Date = rp.TrackRace.Race.Date
                },
                Track = new TrackDto
                {
                    Name = rp.TrackRace.Track.Name,
                    LengthInKm = rp.TrackRace.Track.LengthInKm
                }
            }).ToList()
        };
    }
    
    public async Task AddTrackRaceParticipants(PostParticipationDto dto)
{
    await using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        var race = await _context.Races.FirstOrDefaultAsync(r => r.Name == dto.RaceName);
        if (race == null)
            throw new NotFoundException("Race not found");

        var track = await _context.Tracks.FirstOrDefaultAsync(t => t.Name == dto.TrackName);
        if (track == null)
            throw new NotFoundException("Track not found");
        
        var trackRace = await _context.TrackRaces
            .FirstOrDefaultAsync(tr => tr.RaceId == race.RaceId && tr.TrackId == track.TrackId);
        
        var racerIds = dto.Participations.Select(p => p.RacerId).Distinct().ToList();
        var existingRacerIds = await _context.Racers
            .Where(r => racerIds.Contains(r.RacerId))
            .Select(r => r.RacerId)
            .ToListAsync();
        
        if (existingRacerIds.Count != racerIds.Count)
            throw new NotFoundException("One or more racers not found");

        foreach (var participation in dto.Participations)
        {
            var rp = new RaceParticipation
            {
                TrackRaceId = trackRace.TrackRaceId,
                RacerId = participation.RacerId,
                FinishTimeInSeconds = participation.FinishTimeInSeconds,
                Position = participation.Position
            };
            await _context.RaceParticipations.AddAsync(rp);
            
            if (trackRace.BestTimeInSeconds == null ||
                participation.FinishTimeInSeconds < trackRace.BestTimeInSeconds)
                trackRace.BestTimeInSeconds = participation.FinishTimeInSeconds;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}

}