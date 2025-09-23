import axios from 'axios';

// Base URL
const API_HOST = 'http://localhost:5193';
const GAME_API_URL = `${API_HOST}/api/game`;
const LEADERBOARD_API_URL = `${API_HOST}/api/leaderboard`;

// Função para iniciar o jogo
export const startGame = async (startRequest) => {
    try {
        const response = await axios.post(`${GAME_API_URL}/start`, startRequest); // Envia a dificuldade no corpo da requisição
        return response.data;
    } catch (error) {
        console.error('Error starting game:', error);
        throw error;
    }
}

// Função para fazer uma tentativa de adivinhação
export const makeGuess = async (guess) => {
    try {
        const response = await axios.post(`${GAME_API_URL}/guess`, { guess }, {
            headers: {
                'Content-Type': 'application/json'
            }
        });
        return response.data;
    } catch (error) {
        console.error('Error making guess:', error);
        throw error;
    }
}

// Função para obter o leaderboard
export const getLeaderboard = async (difficulty) => {
    try {
        const response = await axios.get(`${LEADERBOARD_API_URL}/top-scores`, {
            params: { difficulty }
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching leaderboard:', error);
        throw error;
    }
};

// Função para enviar uma nova pontuação ao leaderboard
export const submitScore = async (playerName, attempts, difficulty) => {
    const scoreData = { playerName, attempts, difficulty };
    try {
        const response = await axios.post(`${LEADERBOARD_API_URL}/submit-score`, scoreData);
        return response.data;
    } catch (error) {
        console.error('Error submitting score:', error);
        throw error;
    }
};