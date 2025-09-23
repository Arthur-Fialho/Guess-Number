import { useState } from 'react'
import { startGame, makeGuess } from './services/gameService';
import guessNumberLogo from './assets/guess-number-logo.svg'
import './App.css'

function App() {
  const [guess, setGuess] = useState('');
  const [message, setMessage] = useState('Bem-vindo! Clique em "Novo Jogo" para começar.');
  const [isLoadingNewGame, setIsLoadingNewGame] = useState(false); // Estado para controlar o carregamento do novo jogo
  const [isLoadingGuess, setIsLoadingGuess] = useState(false); // Estado para controlar o carregamento do palpite

  // Função para iniciar um novo jogo
  const handleStartGame = async () => {
    console.log("iniciando jogo...");
    setIsLoadingNewGame(true);
    const result = await startGame();
    setIsLoadingNewGame(false);
    setMessage("Novo jogo iniciado! Faça seu palpite.");
  };

  // Função para enviar um palpite
  const handleMakeGuess = async () => {
    console.log(`Enviando palpite: ${guess}`);
    if (!guess) {
      setMessage('Por favor, insira um número entre 1 e 100.');
      return;
    }
    setIsLoadingGuess(true);
    const result = await makeGuess(guess);
    setIsLoadingGuess(false);
    setMessage(result);
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
        <input type="number" value={guess} onChange={(e) => setGuess(e.target.value)} placeholder='Digite seu palpite' />
        <br /> <br />
        <button onClick={handleMakeGuess} disabled={isLoadingGuess}>
          {isLoadingGuess ? 'Enviando...' : 'Enviar Palpite'}
        </button>
      </div>
      <br /> <hr /> <br />
      </div>
    </>
  )
}

export default App
