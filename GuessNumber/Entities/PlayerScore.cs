namespace GuessNumber.Entities
{
    // Entidade para armazenar a pontuação do jogador no banco de dados
    public class PlayerScore
    {
        public int Id { get; set; }
        public required string PlayerName { get; set; }
        public int Attempts { get; set; }
        public DateTime DateRecorded { get; set; }
    }
}