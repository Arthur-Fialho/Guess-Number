using GuessNumber.Services;
using Xunit;
using GuessNumber.Interfaces;

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
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame();

        // Act
        var result = gameService.MakeGuess("5");

        // Assert
        Assert.Equal("Seu palpite é menor que o número aleatório.", result.Message); // Verifica a mensagem
        Assert.Equal(1, result.Attempts); // Verifica o número de tentativas
        Assert.False(result.IsGameOver); // Verifica se o jogo não acabou
    }

    [Fact]

    public void MakeGuess_WhenGuessIsHigher_ShouldReturnHigherMessageAndCorrectState()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame();

        // Act
        var result = gameService.MakeGuess("15");

        // Assert
        Assert.Equal("Seu palpite é maior que o número aleatório.", result.Message); // Verifica a mensagem
        Assert.Equal(1, result.Attempts); // Verifica o número de tentativas
        Assert.False(result.IsGameOver); // Verifica se o jogo não acabou
    }

    [Fact]
    public void MakeGuess_WhenGuessIsCorrect_ShouldReturnCorrectMessageAndGameOver()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame();

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
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame();

        // Act
        var result = gameService.MakeGuess("abc");

        // Assert
        Assert.Equal("Entrada inválida. Por favor, insira um número inteiro entre 1 e 100.", result.Message); // Verifica a mensagem
        Assert.Equal(0, result.Attempts); // Verifica o número de tentativas
        Assert.False(result.IsGameOver); // Verifica se o jogo não acabou
    }

    [Fact]
    public void MakeGuess_WhenNumberIsOutOfRange_ShouldReturnOutOfRangeMessage()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame();

        // Act
        var result = gameService.MakeGuess("150");

        // Assert
        Assert.Equal("Número 150 fora do intervalo. Por favor, insira um número entre 1 e 100.", result.Message); // Verifica a mensagem
        Assert.Equal(0, result.Attempts); // Verifica o número de tentativas
        Assert.False(result.IsGameOver); // Verifica se o jogo não acabou
    }

    [Fact]
    public void MakeGuess_WhenInputIsInvalid_ShouldNotIncrementAttempts()
    {
        // Arrange
        var fakeRandomProvider = new FakeRandomNumberProvider(10); // Número fixo para teste
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame();

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
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame();

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
        var gameService = new GameService(fakeRandomProvider);
        gameService.StartGame();

        // Act
        gameService.MakeGuess("5");   // 1ª tentativa
        gameService.StartGame();       // Reinicia o jogo
        var result = gameService.MakeGuess("10"); // 1ª tentativa do novo jogo

        // Assert
        Assert.Equal(1, result.Attempts); // Verifica que as tentativas foram resetadas
        Assert.True(result.IsGameOver); // Verifica se o jogo acabou
    }
}    
