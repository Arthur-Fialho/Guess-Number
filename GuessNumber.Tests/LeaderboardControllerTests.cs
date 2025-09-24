using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GuessNumber.Data;
using GuessNumber.Entities;
using GuessNumber.Enums;
using GuessNumber.Models;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GuessNumber.Tests;

// Classe de teste agora HERDA de WebApplicationFactory
public class LeaderboardControllerTests : WebApplicationFactory<Program>
{
    private readonly string _databaseName;
    
    public LeaderboardControllerTests()
    {
        _databaseName = $"TestDb_{Guid.NewGuid()}";
    }

    // Sobrescreve o método que constrói o host da aplicação
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove todas as configurações de DbContext existentes
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType == typeof(DbContextOptions<GameDbContext>) ||
                d.ServiceType == typeof(GameDbContext)).ToList();

            // Percorre e remove cada uma das configurações encontradas
            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            // Cria um service provider personalizado apenas para InMemory
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Adiciona o DbContext com InMemory database usando o service provider dedicado
            services.AddDbContext<GameDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: _databaseName)
                       .UseInternalServiceProvider(serviceProvider)
                       .EnableSensitiveDataLogging(); // Para ajudar no debug
            });
        });
    }

    [Fact]
    public async Task PostAndGetLeaderboard_ShouldSaveAndReturnScore()
    {
        // ARRANGE
        // Criamos o cliente a partir da própria classe de teste
        var client = CreateClient(); 
        var newScore = new SubmitScoreRequest
        {
            PlayerName = "Integration Tester",
            Attempts = 3,
            Difficulty = DifficultyLevel.Hard
        };

        // ACT (POST)
        var postResponse = await client.PostAsJsonAsync("/api/leaderboard/submit-score", newScore); // Endpoint de submissão de score

        // ASSERT (POST)
        postResponse.EnsureSuccessStatusCode(); 

        // ACT (GET)
        var getResponse = await client.GetAsync("/api/leaderboard/top-scores?difficulty=2"); // Endpoint de obtenção do leaderboard

        // ASSERT (GET)
        getResponse.EnsureSuccessStatusCode(); // Verifica se o status code é 200 (OK)
        var leaderboard = await getResponse.Content.ReadFromJsonAsync<List<PlayerScore>>(); // Desserializa a resposta JSON

        Assert.NotNull(leaderboard); // Verifica se a lista não é nula
        Assert.Single(leaderboard); // Verifica se há exatamente um item na lista
        Assert.Equal("Integration Tester", leaderboard[0].PlayerName); // Verifica o nome do jogador
        Assert.Equal(3, leaderboard[0].Attempts); // Verifica o número de tentativas
    }
}