using GuessNumber.Services;
using Microsoft.AspNetCore.Mvc;
using GuessNumber.Models;

namespace GuessNumber.Controllers
{

    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        // Serviço de jogo injetado via construtor
        private readonly GameService _gameService;

        // Construtor para injeção de dependência, recebe o serviço de jogo
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        // Endpoint para iniciar o jogo
        [HttpPost("start")]
        public IActionResult StartGame()
        {
            try
            {
                _gameService.StartGame();
                return Ok();
            }
            catch (Exception ex) // Captura exceções genéricas
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        // Endpoint para fazer uma tentativa
        [HttpPost("guess")]
        public IActionResult MakeGuess([FromBody] GuessModel model)
        {
            try
            {
                // Chama o serviço para processar o palpite se for nulo usa string vazia
                var result = _gameService.MakeGuess(model.Guess ?? string.Empty);
                return Ok(result);
            }
            catch (Exception ex) // Captura exceções genéricas
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}