using Microsoft.AspNetCore.Mvc;
using Test2.DTOs;
using Test2.Exceptions;
using Test2.Services;

namespace Test2.Controllers;
[Route("api")]
[ApiController]
public class DbController : ControllerBase
{
    private readonly IDbService _dbService;
    
    public DbController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("racers/{id}/participations")]
    public async Task<IActionResult> GetRacer( int id)
    {
        try
        {
            var result = await _dbService.GetRacer(id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Internal server error" });
        }
    }

    [HttpPost("track-races/participants")]
    public async Task<IActionResult> CreateParticipation([FromBody] PostParticipationDto dto)
    {
        try
        {
            await _dbService.AddTrackRaceParticipants(dto);
            return Created();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(ex.Message);       
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "Internal server error" });
        }
    }
    
}