using GuessNumber.Entities;
using GuessNumber.Interfaces;
using GuessNumber.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GuessNumber.Controllers
{
    [ApiController]
    [Route("api/leaderboard")]

    // Controlador para gerenciar o leaderboard
    public class LeaderboardController : ControllerBase
    {
        // Serviço de leaderboard injetado via construtor
        private readonly ILeaderboardService _leaderboardService;

        // Construtor para injeção de dependência, recebe o serviço de leaderboard
        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        // Endpoint para submeter um score ao leaderboard
        [HttpPost("submit-score")]
        public async Task<IActionResult> SubmitScore([FromBody] SubmitScoreRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _leaderboardService.AddScoreAsync(request);
            return CreatedAtAction(nameof(SubmitScore), new { id = result.Id }, result);
        }

        [HttpGet("top-scores")]
        public async Task<ActionResult<IEnumerable<PlayerScore>>> GetTopScores()
        {
            var scores = await _leaderboardService.GetTopScoresAsync();
            return Ok(scores);
        }
    }
}