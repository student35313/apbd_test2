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

}