using System.ComponentModel.DataAnnotations;

namespace Test2.DTOs;

public class PostParticipationDto
{
    [Required]
    public string RaceName { get; set; }
    [Required]
    public string TrackName { get; set; }
    public List<PostParticipantDto> Participations { get; set; }
}