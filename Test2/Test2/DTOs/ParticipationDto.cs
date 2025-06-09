namespace Test2.DTOs;

public class ParticipationDto
{
    public RaceDto Race { get; set; }
    public TrackDto Track { get; set; }

    public int Laps { get; set; }
    public int FinishTimeInSeconds { get; set; }
    public int Position { get; set; }
}