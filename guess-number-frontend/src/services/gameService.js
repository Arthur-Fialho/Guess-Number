import axios from 'axios';

// Base URL para a API do jogo
const API_BASE_URL = 'http://localhost:5193/api/game';

// Função para iniciar o jogo
export const startGame = async () => {
    try {
        const response = await axios.post(`${API_BASE_URL}/start`);
        return response.data;
    } catch (error) {
        console.error('Error starting game:', error);
        throw error;
    }
}

// Função para fazer uma tentativa de adivinhação
export const makeGuess = async (guess) => {
    try {
        const response = await axios.post(`${API_BASE_URL}/guess`, { guess }, {
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