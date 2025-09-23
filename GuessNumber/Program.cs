using GuessNumber.Data;
using Microsoft.EntityFrameworkCore;
using GuessNumber.Interfaces;
using GuessNumber.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Configuração do DbContext para usar SQLite
builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependência para o serviço de leaderboard
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

// Configuração do CORS para permitir requisições do frontend
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// Usado Singleton para manter o estado do número aleatório entre requisições
builder.Services.AddSingleton<GuessNumber.Services.GameService>();
builder.Services.AddSingleton<GuessNumber.Interfaces.IRandomNumberProvider, GuessNumber.Providers.SystemRandomProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();

app.Run();
