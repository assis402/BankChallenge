# BankChallenge API ![status](https://img.shields.io/static/v1?label=status&message=ready&color=blue)
<br>

## Resumo do Projeto

A **BankChallenge API** é uma aplicação em .NET 8 que simula funcionalidades de um banco digital. A API permite realizar operações como abertura de contas, transações financeiras, solicitação de empréstimos e quitação de dívidas. O projeto é desenvolvido utilizando as tecnologias mais recentes, seguindo boas práticas de arquitetura e design.

<br>

## Como Testar via Swagger
O projeto contém uma documentação própria gerada em Swagger para facilitar os testes e auxiliar no entendimento de cada endpoint da API.
- [Link do tutorial de teste](how-test.md)

O Swagger é uma ferramenta de código aberto que simplifica o design, a documentação e o consumo de APIs REST, facilitando a comunicação entre desenvolvedores.

<br>

## Tecnologias Utilizadas

- **C# 12**
- **.NET 8**: Versão mais recente da plataforma de desenvolvimento da Microsoft.
- **Autenticação com JWT**
- **XUnit**: Framework para testes unitários e integrados para C#.
- **MongoDB**
- **Docker**
- **Azure Container Apps**: Serviço da Microsoft Azure que permite implantar e gerenciar aplicativos em contêineres de maneira simplificada na nuvem.
<br>
<div>
    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/b/bd/Logo_C_sharp.svg/1200px-Logo_C_sharp.svg.png" height="70">
    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/7/7d/Microsoft_.NET_logo.svg/2048px-Microsoft_.NET_logo.svg.png" height="70">
    <img src="https://jwt.io/img/logo-asset.svg" height="70">
    <img src="https://media.licdn.com/dms/image/D4E12AQE4AmAdWfL3sQ/article-cover_image-shrink_600_2000/0/1695127505668?e=2147483647&v=beta&t=ziuXm_riVZkSTXxDED73oH62D_VLoupVaZKdeF9oTxQ" height="50">
    <img src="https://logowik.com/content/uploads/images/mongodb9740.logowik.com.webp" height="70">
    <img src="https://www.docker.com/wp-content/uploads/2023/08/logo-dont-reverse.svg" height="70">
    <img src="https://ms-azuretools.gallerycdn.vsassets.io/extensions/ms-azuretools/vscode-azurecontainerapps/0.6.1/1699409688312/Microsoft.VisualStudio.Services.Icons.Default" height="70">
</div>
<br>

## Estrutura do Projeto
```
└── Solution
    ├── BankChallenge.API              // Camada de apresentação
    │   ├── Configurations             // Configurações da API, como injeção de dependência, swagger e etc
    │   └── Controllers                // Classes dos Controladores
    │
    ├── BankChallenge.Business         // Camada de regras de negócio
    │   ├── Entities                   // Entidades com suas respectivas lógicas de negócios
    │   ├── Enums                      // Enumerações relacionadas a entidades e mensagens de erro e informação da API
    │   ├── Helpers                    // Classes utilitárias para a camada de negócios
    │   ├── Interfaces                 // Interfaces de serviços e repositórios
    │   ├── Services                   // Implementações dos serviços
    │   └── Validators                 // Fluent validations dos DTOs de request e response
    │    
    ├── BankChallenge.Infrastructure   // Camada de infraestrutura
    │    ├── Helpers                   // Classes de utilidade para a camada de infraestrutura
    │    └── Repositories              // Implementações dos repositórios
    │
    │── BankChallenge.Shared           // Camada com classes genéricas e sem regras de negócios que são compartilhadas com os outros projetos
    │    ├── Dtos                      // Data Transfer Objects
    │    └── Helpers                   // Classes de utilitários genéricos para compartilhar com outras camadas
    │
    └── BankChallenge.UnitTests        // Camada de Testes unitários
         ├── Builders                  // Classes utilizando o padão Builder para constuir os Requests e os Mocks para os testes unitários 
         ├── Mocks                     // Classes com Mocks de repositórios
         └── Services                  // Classes de Testes dos Serviços 
```
<br>

## Padrões de Design e Arquitetura

- API Rest
- Clean Architecture
- Repository
- Builders
- Mock <br>
  - Utilizado para construção de respostas simuladas de repositorio nos testes unitários
- Unit of Work <br>
  - Design Pattern que agrupa operações relacionadas a banco de dados em uma única transação, garantindo consistência e facilitando o controle sobre as alterações, especialmente quando associado ao padrão Repository
<br>

## Execução de Testes Unitários
Para demonstrar proficiência em testes unitários, foram desenvolvidos nove casos de teste que abrangem os fluxos de três métodos na camada de serviço.

![image](https://github.com/assis402/BankChallenge/assets/72348081/bf1fa172-8978-4539-bf4e-38e7e1185f67)

<br>

## Bibliotecas Externas
Algumas das principais bibliotecas do NuGet utilizadas no projeto:

- [Mongo.Driver](https://docs.mongodb.com/drivers/csharp): Biblioteca base para manipulação de dados no banco de dados MongoDB.
- [FluentValidation](https://fluentvalidation.net/): Biblioteca para validação de DTOs.
- [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer): Middleware que permite que um aplicativo receba um bearer token OpenID Connect (JWT).
<br>

## Bibliotecas Próprias

Durante o decorrer deste ano, desenvolvi duas bibliotecas que tenho aplicado em meus projetos pessoais. Neste projeto específico, aproveitei a oportunidade para aprimorar o desempenho e funcionalidades de ambas.

- **Matsoft.MongoDB:** Pacote que facilita a implementação de bancos de dados MongoDB no padrão de design Repository.
  - [Link Nuget](https://www.nuget.org/packages/Matsoft.MongoDB)
  - [Link GitHub](https://github.com/assis402/Matsoft.MongoDB)
- **Matsoft.ApiResults:** Pacote projetado para facilitar o retorno de dados em APIs, cobrindo casos de sucesso e erro. Pode ser utilizado em conjunto com a biblioteca FluentValidation, mas também pode ser usado de forma independente.
  - [Link Nuget](https://www.nuget.org/packages/Matsoft.ApiResults)
  - [Link GitHub](https://github.com/assis402/Matsoft.ApiResults)
 <br>

## Environments

- As envs do projeto são controladas por meio de um arquivo .env, lido diretamente pelo código do projeto. Embora o arquivo esteja no repositório do GitHub para facilitar o deploy no Azure, a prática mais segura seria isolar as variáveis de ambiente utilizando recursos de segurança, como o Key Vault da Azure.
<br>

## CI/CD com GitHub Actions

- O fluxo de integração contínua é gerenciado pelo GitHub Actions, realizando deploy automaticamente no Azure Container App ao realizar um commit na branch master ou aprovar um Pull Request.
<br>

## Banco de Dados Hospedado

- O banco de dados está hospedado no serviço de nuvem do próprio MongoDB Atlas (Plano Gratuito).

    ![image](https://github.com/assis402/BankChallenge/assets/72348081/88e2ed74-5c2f-4dd5-8aa2-62fa2d8104b5)
<br>

## Deploy na Azure com Azure Container App

- A aplicação é implantada na Azure utilizando Azure Container Apps, um serviço da Microsoft Azure projetado para simplificar a implementação e o gerenciamento de aplicativos baseados em contêineres na nuvem. Ele oferece uma abordagem serverless para contêineres, permitindo que os desenvolvedores se concentrem no código e na lógica de aplicativos, enquanto a plataforma cuida da infraestrutura subjacente.

    ![image](https://github.com/assis402/BankChallenge/assets/72348081/4f143181-3f1a-44bf-b884-274798f47b0f)

<br>

## Links para realização de testes

- **Link da API na Azure**: https://bankchallengeapi-app-20231215023.lemonocean-43d2f426.eastus2.azurecontainerapps.io
- **Link do Swagger**: https://bankchallengeapi-app-20231215023.lemonocean-43d2f426.eastus2.azurecontainerapps.io/swagger/index.html
