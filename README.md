

# Mini Banco Virtual

**Mini Banco Virtual** é uma aplicação de banco digital desenvolvida em **C#** com **ASP.NET Core**, que permite o gerenciamento de carteiras, autenticação de usuários e realização de transações financeiras. O projeto utiliza tecnologias como **Dapper**, **FluentMigrator** e **PostgreSQL**.

---

## 🔧 Funcionalidades

### 🔐 Autenticação de Usuários
- Registro e login de usuários
- Geração e validação de tokens JWT

### 💼 Gerenciamento de Carteiras
- Criação automática de carteiras para novos usuários
- Consulta de carteiras por ID ou usuário
- Depósito de fundos em carteiras

### 💸 Transações
- Transferência de fundos entre carteiras
- Validação e rollback em caso de falha

---

## 🛠️ Tecnologias Utilizadas

- **ASP.NET Core** – Framework para construção de APIs REST
- **Docker** – Para subir o banco de dados
- **Dapper** – Micro ORM leve e performático
- **FluentMigrator** – Controle de versionamento do banco de dados
- **PostgreSQL** – Banco de dados relacional
- **JWT** – Autenticação via token

---

## 🗂️ Estrutura do Projeto

```text
MiniBancoVirtual/
├── API/               # Controladores e serviços
├── Identity/          # Autenticação e usuários
├── Core/              # Componentes compartilhados e migrações
├── IntegrationTests/  # Testes de integração
```

---

## ⚙️ Configuração

### ✅ Pré-requisitos

- [.NET 8.0](https://dotnet.microsoft.com/)
- Docker

---

## 🚀 Executando o Projeto

### 1. Clone o repositório

```bash
HTTPS:
git clone https://github.com/matheuskieling/MiniBank.git

ou SSH:
git clone git@github.com:matheuskieling/MiniBank.git

cd MiniBancoVirtual
```

### 2. Execute a aplicação

### 3. Acesse a documentação Swagger


---
## 📦 Migrações do Banco de Dados

As migrações são aplicadas automaticamente ao iniciar o projeto no ambiente de desenvolvimento. Certifique-se de que o banco esteja ativo e acessível.

---

## 🧪 Testes

Os testes de integração estão na pasta `IntegrationTests`.

Para executá-los:

```bash
dotnet test
```
