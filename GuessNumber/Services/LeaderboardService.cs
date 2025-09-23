using GuessNumber.Data;
using GuessNumber.Entities;
using GuessNumber.Interfaces;
using GuessNumber.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GuessNumber.Services
{
    // Serviço para gerenciar o leaderboard
    public class LeaderboardService : ILeaderboardService
    {
        private readonly GameDbContext _context; // DbContext para acesso ao banco de dados

        // Injeção de dependência do DbContext
        public LeaderboardService(GameDbContext context)
        {
            _context = context;
        }

        // Adiciona um score ao leaderboard 
        public async Task<PlayerScore> AddScoreAsync(SubmitScoreRequest request)
        {
            var newScore = new PlayerScore
            {
                PlayerName = request.PlayerName,
                Attempts = request.Attempts,
                DateRecorded = DateTime.UtcNow.AddHours(-3) // Ajuste para o horário de Brasília
            };

            await _context.PlayerScores.AddAsync(newScore);
            await _context.SaveChangesAsync();

            return newScore;
        }
        
        // Obtém os melhores scores do leaderboard
        public async Task<List<PlayerScore>> GetTopScoresAsync()
        {
            return await _context.PlayerScores
                .OrderBy(score => score.Attempts) // Ordena pelo menor número de tentativas
                .ThenBy(score => score.DateRecorded) // Usa a data como critério de desempate
                .Take(10) // Pega apenas os 10 primeiros
                .ToListAsync();
        }

    }
}