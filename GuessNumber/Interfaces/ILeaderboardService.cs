using GuessNumber.Entities;
using GuessNumber.Models;
using System.Threading.Tasks;

namespace GuessNumber.Interfaces
{
    public interface ILeaderboardService
    {
        // Adiciona um score ao leaderboard
        Task<PlayerScore> AddScoreAsync(SubmitScoreRequest request);
        Task<List<PlayerScore>> GetTopScoresAsync(); // Método para obter os melhores scores
    }
}