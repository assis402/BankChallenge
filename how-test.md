# Tutorial de Teste da Aplicação

## Introdução
Este tutorial aborda como realizar testes na aplicação, focando nos métodos de login e cadastro na camada de identidade. Utilize as contas existentes fornecidas para executar os testes.
- [Acesse o Swagger da aplicação](https://bankchallengeapi-app-20231215023.lemonocean-43d2f426.eastus2.azurecontainerapps.io/swagger/index.html)

### Contas para Teste
1. **Titular: José da Silva**
   - **CPF**: 94064810066
   - **Senha**: Senha123@
   - **Número da Conta**: 04778-0

2. **Titular: Matheus de Assis**
   - **CPF**: 52681717043
   - **Senha**: Senha123@
   - **Número da Conta**: 05949-0

3. **Titular: Levi Davi Rezende**
   - **CPF**: 20346941121
   - **Senha**: Senha123@
   - **Número da Conta**: 02444-0
  
### Observações
- Lembre-se de ajustar os dados nas requisições conforme necessário e utilizar as informações das contas fornecidas para validar os resultados dos testes.
- Url base da API: https://bankchallengeapi-app-20231215023.lemonocean-43d2f426.eastus2.azurecontainerapps.io

## Fluxo de Teste

- Exemplo de como realizar uma requisição pelo Swagger: 
<img src="/.github/swagger.gif">

### 1. SignUp
#### POST: `/v1/identity/sigUn`

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
   
   ![signup-response](https://github.com/assis402/BankChallenge/assets/72348081/fcb7c818-0f96-41cd-a619-465cb80d621e)

### 2. SignIn
#### POST: `/v1/identity/signIn`

1. **Requisição de Exemplo:**
   ```json
   {
      "cpf": "52681717043",
      "password": "Senha123@"
   }
   ```

2. **Resposta**

   ![signin-response](https://github.com/assis402/BankChallenge/assets/72348081/3ed655e5-43a8-47d9-ae2d-dbf0f0a9e6f1)

### 3. Adicionar Token JWT
1. Copie o `accesstoken` recebido da responsta do SignIn
 
2. Coloque na modal mostrado após clicar no botão "Authorize"
   
   ![image](https://github.com/assis402/BankChallenge/assets/72348081/c910ea68-eeb6-4f37-98b2-31832166126d)

   ![image](https://github.com/assis402/BankChallenge/assets/72348081/a4ac908c-4b1b-4911-9ff2-1129f6d3de89)


### 4. Buscar as contas vinculadas ao titular
#### GET: `/v1/account`

1. **Esta requisição não necessida de nenhum parâmetro**
2. **Resposta**
   
   ![image](https://github.com/assis402/BankChallenge/assets/72348081/402b964e-cf35-4d17-a4c4-9af347f7b982)

### 5. Depósito
#### POST: `/v1/financialTransaction/deposit`

1. **Requisição de Exemplo:**
   ```json
   {
      "accountNumber": "05949-0",
      "amount": 1000.00
   }
   ```

2. **Resposta**
   
   ![image](https://github.com/assis402/BankChallenge/assets/72348081/b3e93acf-296e-40a2-bee0-d6f3c43aa0fe)

### 6. Saque
#### POST: `/v1/financialTransaction/withdraw`

1. **Requisição de Exemplo:**
   ```json
   {
      "accountNumber": "05949-0",
      "amount": 50.00
   }
   ```

2. **Resposta**
   
   ![image](https://github.com/assis402/BankChallenge/assets/72348081/e7c4ad4c-4d70-4274-929f-52922c388bc4)

### 7. TED Interna
#### POST: `/v1/financialTransaction/tedInTransfer`

1. **Requisição de Exemplo de transferência para a conta do Levi Davi Rezende:** 
   ```json
   {
      "accountNumber": "05949-0",
      "destinationAccountNumber": "02444-0",
      "amount": 1000.00
   }
   ```

2. **Resposta**
   
   ![image](https://github.com/assis402/BankChallenge/assets/72348081/53ab353e-665a-42cf-b9e0-47607ece3a7d)

### 8. Solicitar Empréstimo
#### POST: `/v1/financialTransaction/requestLoan`

1. **Requisição de Exemplo:** 
   ```json
   {
     "accountNumber": "05949-0",
     "amount": 5500
   }
   ```

2. **Resposta**
   
   ![image](https://github.com/assis402/BankChallenge/assets/72348081/65144688-97de-4bd9-a457-53b01c547653)

### 9. Quitar parcialmente uma dívida bancária
#### POST: `/v1/financialTransaction/payOffDebt`

1. Copiar para esta requisição o `debtId` retornado na resposta da solicitação de empréstimo
2. Existe a possibilidade de pagar parcialmente ou pagar o valor total, neste exemplo irei pagar parcialmente para depois realizar outra requisição e quitar a dívida
3. **Requisição de Exemplo:** 
   ```json
   {
     "accountNumber": "05949-0",
     "amount": 4000,
     "debtId": "657c9d3749070336b5e2cf2a"
   }
   ```

4. **Resposta**
   
   ![image](https://github.com/assis402/BankChallenge/assets/72348081/d41dfed3-9657-45dc-8e2a-a7ec4532e29b)

### 10. Quitar totalmente uma dívida bancária
#### POST: `/v1/financialTransaction/payOffDebt`

1. Copiar para esta requisição o `debtId` retornado na resposta da solicitação de empréstimo
2. **Requisição de Exemplo:** 
   ```json
   {
     "accountNumber": "05949-0",
     "amount": 1775,
     "debtId": "657c9d3749070336b5e2cf2a"
   }
   ```

3. **Resposta**
   
   ![image](https://github.com/assis402/BankChallenge/assets/72348081/bf61b5fe-6fba-41f5-9ac2-2713bc3f83db)

### 11. Buscar todas as dívidas bancárias de uma conta
#### GET: `/v1/financialTransaction/getAllDebtsByAccountNumber/{accountNumber}`

1. **Requisição de Exemplo:** => GET /getAllDebtsByAccountNumber/05949-0
2. **Resposta**
   
   ![image](https://github.com/assis402/BankChallenge/assets/72348081/240b39bc-ccd9-446c-a835-a21e039823fd)

### 12. Buscar todas as transações bancárias realizadas por uma conta
#### GET: `/v1/financialTransaction/{accountNumber}`

1. **Requisição de Exemplo:** => GET /05949-0
2. **Resposta**
   
![image](https://github.com/assis402/BankChallenge/assets/72348081/1806ad6a-1537-4cbb-8e4b-9bd4ecb5bdcd)
