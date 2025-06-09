using System.ComponentModel.DataAnnotations;

namespace Test2.DTOs;

public class PostParticipantDto
{
    [Required, Range(1, int.MaxValue)]
    public int RacerId { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int Position { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int FinishTimeInSeconds { get; set; }
}