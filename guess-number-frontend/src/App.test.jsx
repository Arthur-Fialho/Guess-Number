import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { describe, it, expect, vi } from 'vitest';
import App from './App';
import * as gameService from './services/gameService'; // Importa todos os serviços para mockar

// Mocka o módulo de serviços para controlar suas respostas nos testes
vi.mock('./services/gameService');

describe('Componente App', () => {
  it('deve renderizar o título principal do jogo', async () => {
    // Arrange: Mocka a chamada que o useEffect faz.
    gameService.getLeaderboard.mockResolvedValue([]); 

    render(<App />);

    // Act & Assert: Usado findByText para garantir que o teste espera a UI estabilizar
    const mainTitle = await screen.findByText(/Jogo Adivinhe o Número/i);
    expect(mainTitle).toBeInTheDocument();
  });

  it('deve buscar e exibir o placar quando o componente for montado', async () => {
    // Arrange
    const fakeScores = [
      { id: 1, playerName: 'Alice', attempts: 5, difficulty: 0 },
      { id: 2, playerName: 'Bob', attempts: 7, difficulty: 1 },
    ];
    gameService.getLeaderboard.mockResolvedValue(fakeScores);

    // Act
    render(<App />);

    // Assert
    expect(await screen.findByText(/Alice - 5 tentativas \(Fácil\)/i)).toBeInTheDocument();
    expect(await screen.findByText(/Bob - 7 tentativas \(Médio\)/i)).toBeInTheDocument();
  });
  
  it('deve permitir que um usuário jogue uma partida completa e salve sua pontuação', async () => {
    // ARRANGE: Configuração completa do Mock para um jogo vitorioso
    gameService.getLeaderboard.mockResolvedValue([]); // Placar inicial vazio
    gameService.startGame.mockResolvedValue(undefined); // Início do jogo não retorna nada
    gameService.makeGuess.mockResolvedValue({ // Resposta para um palpite correto
      message: 'Parabéns! Você acertou o número!',
      attempts: 1,
      isGameOver: true,
    });
    gameService.submitScore.mockResolvedValue({ // Resposta ao salvar o score
      id: 100,
      playerName: 'Jogador Teste',
      attempts: 1,
      difficulty: 1
    });

    const user = userEvent.setup();
    render(<App />);

    // ACT: Simula as ações do usuário
    // 1. Clica em "Novo Jogo"
    const newGameButton = screen.getByRole('button', { name: /novo jogo/i });
    await user.click(newGameButton);

    // 2. Digita um palpite
    const guessInput = screen.getByPlaceholderText(/digite seu palpite/i);
    await user.type(guessInput, '10');

    // 3. Envia o palpite
    const submitGuessButton = screen.getByRole('button', { name: /enviar palpite/i });
    await user.click(submitGuessButton);

    // 4. O jogo acaba, o formulário de nome aparece. Digita o nome.
    const nameInput = await screen.findByPlaceholderText(/digite seu nome/i);
    await user.type(nameInput, 'Jogador Teste');
    
    // 5. Salva a pontuação
    const saveScoreButton = screen.getByRole('button', { name: /salvar pontuação/i });
    await user.click(saveScoreButton);

    // ASSERT: Verifica o resultado final na tela
    expect(await screen.findByText(/sua pontuação foi salva/i)).toBeInTheDocument();
    
    // Verifica também se o formulário de palpite desapareceu
    expect(screen.queryByPlaceholderText(/digite seu palpite/i)).not.toBeInTheDocument();
  });
});