# Jogo Adivinhe o Número

Web App "Adivinhe o Número" com backend em .NET e frontend em React.

## 📜 Descrição do Projeto

O projeto consiste em uma aplicação web onde o usuário deve adivinhar um número secreto. O jogo possui três níveis de dificuldade, um placar de líderes e um design simples e intuitivo.

![Imagem do Inicio do jogo](</Images/HomeScreen.png>)

## ✨ Funcionalidades

- Três níveis de dificuldade: Fácil (1-50), Médio (1-100) e Difícil (1-500).
- Feedback em tempo real sobre o palpite (maior, menor ou correto).
- Contagem do número de tentativas e armazenamento de números palpitados.
- Placar de líderes para cada nível de dificuldade.
- Possibilidade de salvar a pontuação no final do jogo.

## 💻 Tecnologias Utilizadas

- **Backend:**
  - .NET 9
  - ASP.NET Core Web API
  - Entity Framework Core
  - SQLite
- **Frontend:**
  - React
  - Vite
  - SASS
  - Axios
- **Testes:**
  - xUnit (Backend)
  - Vitest (Frontend)

## 🚀 Começando

Para executar o projeto, siga as instruções abaixo.

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/)

###  Clone o repositório
1.  Clone o repositório:
    ```bash
    git clone https://github.com/Arthur-Fialho/Guess-Number.git
    cd Guess-Number
    ```

### Backend

1. Navegue até a pasta `GuessNumber`:
   ```bash
   cd GuessNumber
   ```
2. Restaure as dependências:
   ```bash
   dotnet restore
   ```
3. Crie e aplique as migrações do banco de dados:
   ```bash
   dotnet ef database update
   ```
4. Execute o projeto:
   ```bash
   dotnet run
   ```
O servidor da API estará disponível em `http://localhost:5193`.

### Frontend

1. Em um novo terminal navegue até a pasta `guess-number-frontend`:
   ```bash
   cd guess-number-frontend
   ```
2. Instale as dependências:
   ```bash
   npm install
   ```
3. Inicie o servidor de desenvolvimento:
   ```bash
   npm run dev
   ```
O frontend estará disponível em `http://localhost:5173`.

## 🏗️ Estrutura do Projeto

O projeto está organizado como um monorepo com as seguintes pastas:

- `GuessNumber/`: Contém o projeto do backend em .NET.
- `guess-number-frontend/`: Contém o projeto do frontend em React.
- `GuessNumber.Tests/`: Contém os testes de unidade e integração para o backend.

## ↔️ Endpoints da API

- `POST /api/game/start`: Inicia um novo jogo.
- `POST /api/game/guess`: Envia um palpite.
- `GET /api/leaderboard/top-scores`: Retorna o placar de líderes.
- `POST /api/leaderboard/submit-score`: Salva a pontuação do jogador.

## 🗃️ Banco de Dados

O projeto utiliza o SQLite como banco de dados. O arquivo do banco de dados (`guessnumber.db`) é criado automaticamente na pasta `GuessNumber` na primeira vez que a aplicação é executada.

## 🧪 Testes

### Backend

Para executar os testes do backend, navegue até a pasta `GuessNumber.Tests` e execute o seguinte comando:

```bash
dotnet test
```

### Frontend

Para executar os testes do frontend, navegue até a pasta `guess-number-frontend` e execute o seguinte comando:

```bash
npm test
```

## 🧠 Premissas Assumidas

**Para a execução deste desafio, as seguintes premissas foram assumidas para definir o escopo e guiar as decisões de arquitetura do projeto:**

- **Instância Única de Jogo:** A premissa mais importante foi a de que apenas um jogo estaria ativo por vez em toda a aplicação. Isso permitiu uma abordagem mais simples e eficiente para o gerenciamento de estado no backend, utilizando um serviço **Singleton** em memória, sem a necessidade de gerenciar múltiplas sessões de jogo simultaneamente.

- **Ausência de Autenticação de Usuários:** Assumi que não era necessário um sistema de login ou autenticação de usuários. O nome do jogador é solicitado e inserido livremente apenas no momento de salvar uma pontuação no placar, simplificando o fluxo e o modelo de dados.

- **Persistência de Dados Focada no Placar:** A necessidade de persistência de dados foi assumida como sendo exclusiva para a funcionalidade do placar. Um banco de dados simples e local como o 
 **SQLite** foi considerado suficiente para os requisitos do desafio, sem a necessidade de um servidor de banco de dados mais robusto. 


- **Ambiente de Desenvolvimento Local:** O projeto foi desenvolvido com a premissa de ser executado em um ambiente local. Configurações para deploy em produção, como o uso de variáveis de ambiente para a *connection string* ou outras chaves, não fizeram parte do escopo.

- **Gerenciamento de Estado Simples no Frontend:** Para a interface, a premissa foi que toda a lógica de estado poderia ser centralizada em um único componente (App.jsx), sem a necessidade de introduzir bibliotecas de gerenciamento de estado mais complexas (como Redux ou Zustand), dado o escopo da aplicação.

## 🧠 Decisões de Projeto

- **Arquitetura:** O projeto foi estruturado em um monorepo simples, com separação clara entre o backend e o frontend para facilitar o desenvolvimento e a manutenção.
- **Gerenciamento de Estado do Jogo:** A lógica do jogo é mantida no backend através de um serviço (`GameService`) com tempo de vida Singleton, garantindo um estado de jogo único e consistente para toda a aplicação.
- **Banco de Dados:** Foi utilizado o Entity Framework Core com a abordagem Code-First, permitindo que o modelo de dados em C# seja a fonte da verdade para a estrutura do banco. O SQLite foi escolhido por ser um banco de dados leve e de fácil configuração.
- **Testes:** A estratégia de testes foi abrangente, com testes de unidade e integração no backend e testes de componente e interação no frontend.

## 🤖 Uso de IA

- **Google Gemini:** Utilizado como um Tech Lead e mentor, guiando o projeto através de tarefas divididas em etapas, explicando conceitos complexos e ajudando na depuração de erros.
- **Github Copilot:** Utilizado para auxiliar na escrita e autocompletar o código dentro do VS Code.
- **ChatGPT:** Utilizado para tirar dúvidas específicas de sintaxe e conceitos pontuais.

## 👨‍💻 Autor

- [Arthur Fialho](https://arthurfialho.com.br)
- [GitHub](https://github.com/Arthur-Fialho)
- [LinkedIn](https://www.linkedin.com/in/arthurfialho/)
