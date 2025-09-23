using GuessNumber.Services;
using Xunit;
using GuessNumber.Interfaces;
using GuessNumber.Enums;

namespace GuessNumber.Tests;

// Criação de uma implementação de criação de um numero falso randomico para IRandomNumberProvider
public class FakeRandomNumberProvider : IRandomNumberProvider
{
    private readonly int _numberToReturn;

    public FakeRandomNumberProvider(int numberToReturn)
    {
        _numberToReturn = numberToReturn;
    }

    public int Next(int minValue, int maxValue)
    {
        return _numberToReturn;
    }
}

public class GameServiceTests
{
    [Fact]
    public void MakeGuess_WhenGuessIsLower_ShouldReturnLowerMessageAndCorrectState()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var difficulty = DifficultyLevel.Medium;
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame(difficulty);

        // Act
        var result = gameService.MakeGuess("5");

        // Assert
        Assert.Equal("Seu palpite 5 é menor que o número aleatório.", result.Message); // Verifica a mensagem
        Assert.Equal(1, result.Attempts); // Verifica o número de tentativas
        Assert.False(result.IsGameOver); // Verifica se o jogo não acabou
    }

    [Fact]

    public void MakeGuess_WhenGuessIsHigher_ShouldReturnHigherMessageAndCorrectState()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var difficulty = DifficultyLevel.Medium;
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame(difficulty);

        // Act
        var result = gameService.MakeGuess("15");

        // Assert
        Assert.Equal("Seu palpite 15 é maior que o número aleatório.", result.Message); // Verifica a mensagem
        Assert.Equal(1, result.Attempts); // Verifica o número de tentativas
        Assert.False(result.IsGameOver); // Verifica se o jogo não acabou
    }

    [Fact]
    public void MakeGuess_WhenGuessIsCorrect_ShouldReturnCorrectMessageAndGameOver()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var difficulty = DifficultyLevel.Medium;
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame(difficulty);

        // Act
        var result = gameService.MakeGuess("10");

        // Assert
        Assert.Equal("Parabéns! Você acertou o número!", result.Message); // Verifica a mensagem
        Assert.Equal(1, result.Attempts); // Verifica o número de tentativas
        Assert.True(result.IsGameOver); // Verifica se o jogo acabou
    }

    [Fact]
    public void MakeGuess_WhenInputIsNotANumber_ShouldReturnInvalidInputMessage()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var difficulty = DifficultyLevel.Medium;
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame(difficulty);

        // Act
        var result = gameService.MakeGuess("abc");

        // Assert
        Assert.Equal("Entrada inválida. Por favor, insira um número inteiro.", result.Message); // Verifica a mensagem
        Assert.Equal(0, result.Attempts); // Verifica o número de tentativas
        Assert.False(result.IsGameOver); // Verifica se o jogo não acabou
    }

    [Theory]
    [InlineData(DifficultyLevel.Easy, 51)]  // Cenário 1: Dificuldade Fácil, palpite 51 (fora)
    [InlineData(DifficultyLevel.Easy, 0)]   // Cenário 2: Dificuldade Fácil, palpite 0 (fora)
    [InlineData(DifficultyLevel.Medium, 101)] // Cenário 3: Dificuldade Média, palpite 101 (fora)
    [InlineData(DifficultyLevel.Hard, 501)] // Cenário 4: Dificuldade Difícil, palpite 501 (fora)
    public void MakeGuess_WhenNumberIsOutOfRange_ShouldReturnOutOfRangeMessage(DifficultyLevel difficulty, int outOfRangeGuess)
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); 
        var maxNumber = difficulty switch
        {
            DifficultyLevel.Easy => 50,
            DifficultyLevel.Medium => 100,
            DifficultyLevel.Hard => 500,
            _ => 100
        };
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame(difficulty); // Inicia o jogo com a dificuldade de cada cenário

        // Act
        var result = gameService.MakeGuess(outOfRangeGuess.ToString());

        // Assert
        Assert.Contains($"Número {outOfRangeGuess} fora do intervalo. Por favor, insira um número entre 1 e {maxNumber}.", result.Message);
        Assert.Equal(0, result.Attempts);
        Assert.False(result.IsGameOver);
    }
    

    [Fact]
    public void MakeGuess_WhenInputIsInvalid_ShouldNotIncrementAttempts()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var difficulty = DifficultyLevel.Medium;
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame(difficulty);

        // Act
        var result1 = gameService.MakeGuess("abc"); // Entrada inválida
        var result2 = gameService.MakeGuess("150"); // Fora do intervalo
        var result3 = gameService.MakeGuess("5");   // Entrada válida

        // Assert
        Assert.Equal(1, result3.Attempts); // Verifica que apenas a entrada válida incrementou as tentativas
    }

    [Fact]
    public void MakeGuess_AfterMultipleValidGuesses_ShouldReportCorrectAttemptCount()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var difficulty = DifficultyLevel.Medium;
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame(difficulty);

        // Act
        gameService.MakeGuess("5");   // 1ª tentativa
        gameService.MakeGuess("15");  // 2ª tentativa
        var result = gameService.MakeGuess("10"); // 3ª tentativa, correta

        // Assert
        Assert.Equal(3, result.Attempts); // Verifica que o número de tentativas está correto
        Assert.True(result.IsGameOver); // Verifica se o jogo acabou
    }

    [Fact]
    public void StartGame_AfterOneGuess_ShouldResetAttemptsToZero()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var difficulty = DifficultyLevel.Medium;
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame(difficulty);

        // Act
        gameService.MakeGuess("5");   // 1ª tentativa
        gameService.StartGame(difficulty);       // Reinicia o jogo
        var result = gameService.MakeGuess("10"); // 1ª tentativa do novo jogo

        // Assert
        Assert.Equal(1, result.Attempts); // Verifica que as tentativas foram resetadas
        Assert.True(result.IsGameOver); // Verifica se o jogo acabou
    }
}    
