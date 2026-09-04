# IntegraSUS API 🏥

O **IntegraSUS** é uma API RESTful desenvolvida em ASP.NET Core para o gerenciamento de registros de saúde de pacientes. O sistema permite cadastrar pacientes, vincular responsáveis (para menores de idade ou incapazes), associar o usuário do sistema que realizou o cadastro e gerenciar dados clínicos básicos.

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C# (.NET Core)
* **Framework Web:** ASP.NET Core Web API
* **Banco de Dados:** SQL Server
* **Acesso a Dados:** ADO.NET puro (`Microsoft.Data.SqlClient`)
* **Arquitetura:** MVC (Models, Controllers)

## 🗄️ Estrutura do Banco de Dados

O banco de dados (`IntegraSUS_DB`) foi modelado com foco na integridade referencial, utilizando chaves estrangeiras (Foreign Keys) e validações robustas (Check Constraints).

### Tabelas principais:
1. **`usuarios`**: Funcionários do sistema (Atendentes e Administradores).
2. **`responsaveis`**: Pais, mães ou responsáveis legais pelos pacientes.
3. **`pacientes`**: Cadastro principal do paciente (com vínculos opcionais para `responsaveis` e `usuarios`).
4. **`enderecos`**: Dados de localização do paciente.
5. **`dados_clinicos`**: Informações de saúde (tipo sanguíneo, alergias, doenças crônicas).

## 🚀 Como Executar o Projeto

### Pré-requisitos
* [.NET 6.0 SDK](https://dotnet.microsoft.com/download) (ou superior)
* SQL Server (ou LocalDB/SQLExpress)
* Visual Studio, VS Code ou outra IDE compatível

### 1. Configurar o Banco de Dados
1. Abra o SQL Server Management Studio (SSMS) ou Azure Data Studio.
2. Execute o script de criação do banco de dados (disponível na documentação ou arquivos `.sql` do projeto).
3. Popule as tabelas `usuarios` e `responsaveis` para evitar erros de restrição de Chave Estrangeira.

### 2. Configurar a Connection String
No arquivo `appsettings.json`, certifique-se de configurar a sua conexão com o banco de dados SQL Server no bloco `ConnectionStrings`:

```json
{
  "ConnectionStrings": {
    "IntegraSUSConnection": "Server=SEU_SERVIDOR\\SQLEXPRESS;Database=IntegraSUS_DB;Trusted_Connection=True;Encrypt=False;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
