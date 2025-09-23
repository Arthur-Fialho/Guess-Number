import { useState, useEffect } from 'react'
import { startGame, makeGuess, getLeaderboard, submitScore } from './services/gameService';
import guessNumberLogo from './assets/guess-number-logo.svg'
import './App.css'

function App() {
  const [guess, setGuess] = useState('');
  const [message, setMessage] = useState('Selecione a dificuldade e clique em "Novo Jogo" para começar.');
  const [isLoadingNewGame, setIsLoadingNewGame] = useState(false); // Estado para controlar o carregamento do novo jogo
  const [isLoadingGuess, setIsLoadingGuess] = useState(false); // Estado para controlar o carregamento do palpite
  const [attempts, setAttempts] = useState(0);
  const [guessHistory, setGuessHistory] = useState([]);
  const [isGameOver, setIsGameOver] = useState(false);
  const [difficulty, setDifficulty] = useState(1); // 0: Fácil, 1: Médio, 2: Difícil
  const [playerName, setPlayerName] = useState('');

  const [leaderboard, setLeaderboard] = useState([]);
  // Carrega o leaderboard ao montar o componente
    useEffect(() => {
    const fetchLeaderboard = async () => {
      try {
        const scores = await getLeaderboard();
        setLeaderboard(scores);
      } catch (error) {
        console.error("Failed to load leaderboard");
      }
    };

      fetchLeaderboard();
    }, []);

  // Função para iniciar um novo jogo
  const handleStartGame = async () => {
    setIsLoadingNewGame(true);
    const result = await startGame({difficulty});
    setIsLoadingNewGame(false);
    setAttempts(0);
    setGuessHistory([]);
    setIsGameOver(false);
    if (difficulty === 0) {
      setMessage(`Jogo iniciado no nível Fácil! Faça seu palpite.`);
    } else if (difficulty === 1) {
      setMessage(`Jogo iniciado no nível Médio! Faça seu palpite.`);
    } else {
      setMessage(`Jogo iniciado no nível Difícil! Faça seu palpite.`);
    }
  };

  // Função para enviar um palpite
  const handleMakeGuess = async () => {
    // Verifica se o palpite é válido e seta a dificuldade para validar o intervalo
    const guessNum = parseInt(guess);
    let maxNumber = 100;
    if (difficulty === 0) maxNumber = 50;
    if (difficulty === 2) maxNumber = 500;

    if (!guess || isNaN(guessNum) || guessNum < 1 || guessNum > maxNumber) {
      setMessage(`Por favor, insira um número entre 1 e ${maxNumber}.`);
      return;
    }
    setIsLoadingGuess(true);
    const result = await makeGuess(guess);
    setIsLoadingGuess(false);
    setMessage(result.message);
    setAttempts(result.attempts);
    setIsGameOver(result.isGameOver);
    setGuessHistory([...guessHistory, guess]);
    setGuess('');
  };

  const handleSubmitScore = async () => {
    // Valida o nome do jogador
    if (!playerName.trim()) {
        setMessage("Por favor, digite um nome válido.");
        return;
    }
    try {
        await submitScore(playerName, attempts);
        setMessage(`Parabéns, ${playerName}! Sua pontuação foi salva.`);

        // Atualiza o placar em tempo real
        const updatedScores = await getLeaderboard();
        setLeaderboard(updatedScores);
    } catch (error) {
        setMessage("Ocorreu um erro ao salvar sua pontuação.");
    }
};

  return (
    <>
      <div className="App">
        <a href="">
          <img src={guessNumberLogo} className="logo react" alt="Guess Number logo" />
        </a>
      <h1>Jogo Adivinhe o Número</h1>
      <h2>{message}</h2>
      <p><strong>Dificuldade Selecionada:</strong> {difficulty === 0 ? 'Fácil números entre 1 e 50' : difficulty === 1 ? 'Médio números entre 1 e 100' : 'Difícil números entre 1 e 500'}</p>
      <br />
      <div className="difficulty-selector">
        <span>Dificuldade: </span>
        <button onClick={() => setDifficulty(0)}>Fácil</button>
        <button onClick={() => setDifficulty(1)}>Médio</button>
        <button onClick={() => setDifficulty(2)}>Difícil</button>
      </div> <br />
      <button onClick={handleStartGame} disabled={isLoadingNewGame}>
        {isLoadingNewGame ? 'Iniciando...' : 'Novo Jogo'}
      </button>
      <br /> <br />
      <div>
      {/* Formulário para salvar a pontuação */}
      { isGameOver ? (
        // Se o jogo acabou, mostra o formulário para salvar o score
        <div className="submit-score">
            <input
                type="text"
                placeholder="Digite seu nome para o placar!"
                value={playerName}
                onChange={(e) => setPlayerName(e.target.value)}
            />
            <button onClick={handleSubmitScore}>Salvar Pontuação</button>
        </div>
      ) : (
      // Se o jogo NÃO acabou, mostra o formulário de palpite
        <div>
            <input 
              type="number" 
              value={guess} 
              onChange={(e) => setGuess(e.target.value)} 
              placeholder='Digite seu palpite' 
              disabled={isLoadingGuess} 
            />
            <br /> <br />
            <button onClick={handleMakeGuess} disabled={isLoadingGuess}>
              {isLoadingGuess ? 'Enviando...' : 'Enviar Palpite'}
            </button>
        </div>
      )}
        <br /> <br />
        <button onClick={handleMakeGuess} disabled={isGameOver || isLoadingGuess}>
          {isLoadingGuess ? 'Enviando...' : 'Enviar Palpite'}
        </button>
        {/* Só mostra essa seção se o jogo já tiver começado */}
        {attempts > 0 && (
          <div className="game-info">
            <p>Tentativas válidas: {attempts}</p>
            <p>Palpites já feitos: {guessHistory.join(', ')}</p>
          </div>
        )}
      </div>
      <br /> <hr /> <br />
      <div className="leaderboard">
        <h3>🏆 Placar dos Melhores 🏆</h3>
        <ol>
          {leaderboard.map((score) => (
            <li key={score.id}>
              {score.playerName} - {score.attempts} tentativas
            </li>
          ))}
        </ol>
      </div>
      <br /> <hr /> <br />
      <p>Criado por Arthur Fialho</p>
      </div>
    </>
  )
}

export default App
