namespace GuessNumber.Services
{
    public class GameService
    {
        // Número aleatório a ser adivinhado
        private int RandomNumber { get; set; }

        // Gerador de números aleatórios, stático para manter o estado entre chamadas e evitar repetição
        private static readonly Random _random = new Random();

        // Inicia o jogo gerando um número aleatório
        public void StartGame()
        {
            RandomNumber = GenerateRandomNumber();
        }

        // Gera um numero aleatório entre 1 e 100
        public int GenerateRandomNumber()
        {
            return _random.Next(1, 101);
        }

        // Faz uma tentativa de adivinhar o número
        public string MakeGuess(string playerInput)
        {
            // Valida a entrada do jogador se não consegue converter para inteiro ou está fora do intervalo
            if (!int.TryParse(playerInput, out int playerNumber))
            {
                return "Entrada inválida. Por favor, insira um número inteiro entre 1 e 100.";
            }
            if (playerNumber < 1 || playerNumber > 100)
            {
                return "Número fora do intervalo. Por favor, insira um número entre 1 e 100.";
            }
            return CompareGuess(playerNumber, RandomNumber);
        }

        // Compara o palpite do jogador com o número aleatório
        public static string CompareGuess(int playerNumber, int randomNumber)
        {
            if (playerNumber < randomNumber)
            {
                return "Seu palpite é menor que o número aleatório.";
            }
            else if (playerNumber > randomNumber)
            {
                return "Seu palpite é maior que o número aleatório.";
            }
            else
            {
                return "Parabéns! Você acertou o número!";
            }
        }

    }
}