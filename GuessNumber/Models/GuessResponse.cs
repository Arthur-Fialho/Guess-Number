namespace GuessNumber.Models
{
    public class GuessResponse
    {
        // Resposta da tentativa do jogador (DTO)
        public string? Message { get; set; }
        public int Attempts { get; set; }
        public bool IsGameOver { get; set; }
    }
}