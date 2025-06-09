using Test2.DTOs;

namespace Test2.Services;

public interface IDbService
{
    Task<GetRacerDto> GetRacer(int id);
    Task AddTrackRaceParticipants(PostParticipationDto dto);
}