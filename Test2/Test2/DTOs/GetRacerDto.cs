namespace Test2.DTOs;

public class GetRacerDto
{
    public int RacerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<ParticipationDto> Participations { get; set; }
}