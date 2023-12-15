# Tutorial de Teste da Aplicação

## Introdução
Este tutorial aborda como realizar testes na aplicação, focando nos métodos de login e cadastro na camada de identidade. Utilize as contas existentes fornecidas para executar os testes.
- [Acesse o Swagger da aplicação](https://bankchallengeapi-app-20231215023.lemonocean-43d2f426.eastus2.azurecontainerapps.io/swagger/index.html)

### Contas para Teste
1. **Titular: José da Silva**
   - CPF: 94064810066
   - Senha: Senha123@
   - Número da Conta: 04778-0

2. **Titular: Matheus de Assis**
   - CPF: 52681717043
   - Senha: Senha123@
   - Número da Conta: 01248-0

3. **Titular: Levi Davi Rezende**
   - CPF: 20346941121
   - Senha: Senha123@
   - Número da Conta: 02444-0
  
### Observações
- Lembre-se de ajustar os dados nas requisições conforme necessário e utilizar as informações das contas fornecidas para validar os resultados dos testes.
- Url base da API: https://bankchallengeapi-app-20231215023.lemonocean-43d2f426.eastus2.azurecontainerapps.io

## Fluxo de Teste

### 1. SignUp
#### Rota: `/v1/identity/sigUn`

1. **Requisição de Exemplo:**
   ```json
   {
       "email": "assis@email.com",
       "password": "Senha123@",
       "cpf": "52681717043",
       "name": "Matheus de Assis",
       "birthdate": "1997-05-06",
       "address": "Rua A, 123",
       "initialDeposit": 20000.00
   }
   ```
2. **Resposta**
<img src="/.github/readme-imgs/unit-tests-img.png">


### 2. SignIn
#### Rota: `/v1/identity/signIn`

1. **Requisição de Exemplo:**
   ```json
   {
      "cpf": "52681717043",
      "password": "Senha123@"
   }
   ```

2. **Resposta**
<img src="/.github/readme-imgs/unit-tests-img.png">
