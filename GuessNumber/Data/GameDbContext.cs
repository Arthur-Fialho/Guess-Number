using GuessNumber.Entities;
using Microsoft.EntityFrameworkCore;

namespace GuessNumber.Data
{
    public class GameDbContext : DbContext
    {
        // Construtor que aceita opções e as passa para a classe base DbContext
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }
        // DbSet para a entidade PlayerScore, representando a tabela no banco de dados
        public DbSet<PlayerScore> PlayerScores { get; set; }

    }
}