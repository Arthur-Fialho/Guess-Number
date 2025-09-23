using GuessNumber.Models;
using GuessNumber.Interfaces;
using GuessNumber.Enums;

namespace GuessNumber.Services
{
    public class GameService
    {
        // Dependência para geração de números aleatórios
        private readonly IRandomNumberProvider _randomNumberProvider;

        // Número aleatório a ser adivinhado
        private int RandomNumber { get; set; }

        private int _attempts;

        private DifficultyLevel _currentDifficulty;

        // Construtor que injeta a dependência do provedor de números aleatórios
        public GameService(IRandomNumberProvider randomNumberProvider)
        {
            _randomNumberProvider = randomNumberProvider;
        }

        // Inicializa o número aleatório e zera as tentativas
        public void StartGame(DifficultyLevel difficulty)
        {
            _currentDifficulty = difficulty;
            RandomNumber = GenerateRandomNumber(difficulty);
            _attempts = 0;
        }

        // Gera um numero aleatório entre 1 e 100
        public int GenerateRandomNumber(DifficultyLevel difficulty)
        {
            int maxNumber = difficulty switch
            {
                DifficultyLevel.Easy => 51,   // Gera números de 1 a 50
                DifficultyLevel.Hard => 501,  // Gera números de 1 a 500
                _ => 101,                     // Padrão (Medium) gera de 1 a 100
            };
            return _randomNumberProvider.Next(1, maxNumber);
        }

        // Faz uma tentativa de adivinhar o número e retorna a resposta no modelo GuessResponse dto
        public GuessResponse MakeGuess(string playerInput)
        {
            // Valida a entrada do jogador se não consegue converter para inteiro ou está fora do intervalo
            if (!int.TryParse(playerInput, out int playerNumber))
            {
                return new GuessResponse
                {
                    Message = "Entrada inválida. Por favor, insira um número inteiro.",
                    Attempts = _attempts,
                    IsGameOver = false
                };
            }

            int maxNumber = _currentDifficulty switch
            {
                DifficultyLevel.Easy => 50,
                DifficultyLevel.Hard => 500,
                _ => 100,
            };
            // Verifica se o número está dentro do intervalo permitido
            if (playerNumber < 1 || playerNumber > maxNumber)
            {
                return new GuessResponse
                {
                    Message = $"Número {playerNumber} fora do intervalo. Por favor, insira um número entre 1 e {maxNumber}.",
                    Attempts = _attempts,
                    IsGameOver = false
                };
            }
            // Compara o palpite do jogador com o número aleatório e retorna a resposta
            return new GuessResponse
            {
                Message = CompareGuess(playerNumber, RandomNumber),
                Attempts = ++_attempts,
                IsGameOver = playerNumber == RandomNumber
            };
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