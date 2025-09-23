using GuessNumber.Enums;

namespace GuessNumber.Models
{
    public class StartGameRequest
    {
        public DifficultyLevel Difficulty { get; set; }
    }
}