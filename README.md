Pré-requisitos
## Instalar .NET 8.0
Certifique-se de que o .NET 8 está instalado em sua máquina.

### Este projeto utiliza container Docker para rodar as dependências necessárias. Abaixo estão os passos para configurar e executar a aplicação.

## 

### 1. Postgress

Segue o link para Construindo ambiente PostgreSQL no Docker:
https://medium.com/@nathaliafriederichs/construindo-ambiente-postgresql-no-docker-guia-passo-a-passo-659e9d2a25ef

### 2. Verifique no arquivo 'appsettings.Development' se o Servidor e a porta corresponde as configurações da sua conexão:

### 3. Passos para criar Usuario e Carteira

Acesse a API de autenticação para registrar um usuario:
  * URL: https://localhost:7131/swagger/index.html
  * Método: POST /api/Usuario/registrar
  * Request:
    ```Json
    {
      "email": "usuario1@teste.com",
      "senha": "123"
    }
    ```
Ao registra um Usuário o mesmo será vinculado a uma Carteira.

Copie o token gerado.

### 4. Configurar Autorização na Vendas.API
Acesse a API de vendas configure o token de autorização:
  * URL: https://localhost:7226/swagger/index.html
  * No Swagger, clique no botão "Authorize" e cole o token gerado.

### 4. Realizar Movimentaçoes na carteira do usuario

  1. Saldo da Carteira:
      * Método: GET /api/Carteira/saldo
      * Buscar o saldo da carteira do usuario logado.

  2. Deposito:
      * Método: POST /api/Carteira/deposito
      * Realiza deposito na carteira do usuario logado.

  3. Saque:
      * Método: POST /api/Carteira/saque
      * Realiza saque na carteira do usuario logado.
    
  4. Transferencia:
      * Método: POST /api/Carteira/transferencia
      * Realiza transferencia da carteira do usuario logado para uma carteira de um outro usuario.
    
