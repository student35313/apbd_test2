using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Test2.Models;

[Table("Race_Participation")]
[PrimaryKey(nameof(TrackRaceId),nameof(RacerId))]
public class RaceParticipation
{
    [ForeignKey(nameof(Racer))]
    public int TrackRaceId { get; set; }
    [ForeignKey(nameof(TrackRace))]
    public int RacerId { get; set; }

    public int FinishTimeInSeconds { get; set; }

    public int Position { get; set; }

    public Racer Racer { get; set; }

    public TrackRace TrackRace { get; set; }
}