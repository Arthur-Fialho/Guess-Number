import { useState } from 'react'
import { startGame, makeGuess } from './services/gameService';
import guessNumberLogo from './assets/guess-number-logo.svg'
import './App.css'

function App() {
  const [guess, setGuess] = useState('');
  const [message, setMessage] = useState('Bem-vindo! Clique em "Novo Jogo" para começar.');
  const [isLoadingNewGame, setIsLoadingNewGame] = useState(false); // Estado para controlar o carregamento do novo jogo
  const [isLoadingGuess, setIsLoadingGuess] = useState(false); // Estado para controlar o carregamento do palpite
  const [attempts, setAttempts] = useState(0);
  const [guessHistory, setGuessHistory] = useState([]);
  const [isGameOver, setIsGameOver] = useState(false);

  // Função para iniciar um novo jogo
  const handleStartGame = async () => {
    setIsLoadingNewGame(true);
    const result = await startGame();
    setIsLoadingNewGame(false);
    setAttempts(0);
    setGuessHistory([]);
    setIsGameOver(false);
    setMessage("Novo jogo iniciado! Faça seu palpite.");
  };

  // Função para enviar um palpite
  const handleMakeGuess = async () => {
    if (!guess) {
      setMessage('Por favor, insira um número entre 1 e 100.');
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

  return (
    <>
      <div className="App">
        <a href="">
          <img src={guessNumberLogo} className="logo react" alt="Guess Number logo" />
        </a>
      <h1>Jogo Adivinhe o Número</h1>
      <h2>{message}</h2>
      <br />  
      <button onClick={handleStartGame} disabled={isLoadingNewGame}>
        {isLoadingNewGame ? 'Iniciando...' : 'Novo Jogo'}
      </button>
      <br /> <br />
      <div>
        <input type="number" value={guess} onChange={(e) => setGuess(e.target.value)} 
          placeholder='Digite seu palpite' disabled={isGameOver || isLoadingGuess} />
        <br /> <br />
        <button onClick={handleMakeGuess} disabled={isGameOver || isLoadingGuess}>
          {isLoadingGuess ? 'Enviando...' : 'Enviar Palpite'}
        </button>
        {/* Só mostra essa seção se o jogo já tiver começado */}
        {attempts > 0 && (
          <div className="game-info">
            <p>Tentativas: {attempts}</p>
            <p>Palpites já feitos: {guessHistory.join(', ')}</p>
          </div>
        )}
      </div>
      <br /> <hr /> <br />
      </div>
    </>
  )
}

export default App
