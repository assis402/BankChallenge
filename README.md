# BankChallenge API

## Resumo do Projeto

A **BankChallenge API** é uma aplicação em .NET 8 que simula funcionalidades de um banco digital. A API permite realizar operações como abertura de contas, transações financeiras, solicitação de empréstimos e quitação de dívidas. O projeto é desenvolvido utilizando as tecnologias mais recentes, seguindo boas práticas de arquitetura e design.

## Tecnologias Utilizadas

- C# 12
- .NET 8
- MongoDB
- Docker
- Azure Container Apps

## Padrões de Design e Arquitetura

- API Rest
- Clean Architecture
- Repository
- Builders
- Mock
- Unit of Work

## Bibliotecas Externas

- [Mongo.Driver](https://docs.mongodb.com/drivers/csharp): Biblioteca para manipulação de dados no banco de dados MongoDB.
- [FluentValidation](https://fluentvalidation.net/): Biblioteca para validação de DTOs.

## Bibliotecas Próprias Utilizadas

- [Matsoft.MongoDB](https://www.nuget.org/packages/Matsoft.MongoDB): Pacote que facilita a implementação de bancos de dados MongoDB no padrão de design Repository.
- [Matsoft.ApiResults](https://www.nuget.org/packages/Matsoft.ApiResults): Pacote projetado para facilitar o retorno de dados em APIs, cobrindo casos de sucesso e erro. Pode ser utilizado em conjunto com a biblioteca FluentValidation, mas também pode ser usado de forma independente.

## Banco de Dados e Transações

- MongoDB é utilizado como banco de dados.
- Padrão Unit of Work é implementado para suportar transações.

## Environments

- As configurações do projeto podem ser gerenciadas utilizando um arquivo `.env`.

## CI/CD com GitHub Actions

- O fluxo de integração contínua/desdobramento contínuo é gerenciado pelo GitHub Actions.

## Banco de Dados Hospedado

- O banco de dados está hospedado no serviço de nuvem MongoDB Atlas (Plano Gratuito).

## Deploy na Azure com Azure Container App

- A aplicação é implantada na Azure utilizando Azure Container Apps, uma plataforma de hospedagem de contêineres.

## Links Úteis

- [Link da API na Azure](https://bankchallengeapi-app-20231215023.lemonocean-43d2f426.eastus2.azurecontainerapps.io)
- [Link do Swagger](https://bankchallengeapi-app-20231215023.lemonocean-43d2f426.eastus2.azurecontainerapps.io/swagger/index.html)

## Como Testar

### Pré-requisitos

- Certifique-se de ter o .NET 8 instalado em sua máquina.
- Clone o repositório do projeto: `git clone https://github.com/assis402/BankChallenge.git`
- Navegue até o diretório do projeto: `cd BankChallenge`
