using GuessNumber.Enums;

namespace GuessNumber.Models
{
    // Modelo para requisição de submissão de score
    public class SubmitScoreRequest
    {
        public required string PlayerName { get; set; }
        public int Attempts { get; set; }
        public DifficultyLevel Difficulty { get; set; }
    }
}