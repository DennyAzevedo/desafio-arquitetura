# CashFlow – Sistema de Lançamentos e Consolidação de Fluxo de Caixa

Projeto desenvolvido como parte de um desafio técnico para vaga de Arquiteto de Software.  
A solução foi construída aplicando princípios de **Clean Architecture**, **DDD**, **SOLID** e **C4 Model**, com uma abordagem pragmática voltada para simplicidade, clareza arquitetural, escalabilidade e resiliência.

---

## 1. Visão Geral do Problema

Um comerciante precisa:

- Registrar lançamentos financeiros (crédito/débito).
- Consultar o saldo diário consolidado.

O desafio exige:

- Arquitetura escalável, resiliente e bem documentada.
- Uso de **C# / ASP.NET Core**.
- Boas práticas, testes e documentação arquitetural.
- Repositório público com instruções claras.

---

## 2. Arquitetura da Solução

A solução final utiliza **dois microsserviços independentes**, construídos com ASP.NET Core:

- **TransactionService** → responsável por registrar lançamentos.
- **ConsolidationService** → responsável por consultas de saldo diário consolidado.

Ambos compartilham um **único banco PostgreSQL**, cada qual manipulando apenas suas tabelas conforme seu contexto.

### 2.1. Arquitetura MVP

A arquitetura do MVP foi desenhada com foco em entregar uma solução:

- funcional,
- simples,
- escalável horizontalmente,
- resiliente,
- e extremamente clara para avaliação.

### MVP inclui

- Dois microsserviços independentes
- PostgreSQL em um único banco
- ASP.NET Core minimal APIs ou controllers
- Testes unitários com xUnit, Moq e FluentAssertions
- Docker Compose para levantar todo o ambiente
- Diagramas C4 (Contexto, Containers e Componentes)
- ADRs documentando decisões críticas
- Polly para resiliência (retry, timeout, circuit breaker)

### MVP **não** inclui

- Mensageria
- Outbox
- Processadores assíncronos
- Banco separado write/read
- Redis
- Frontend
- API Gateway

Esses elementos pertencem à **Arquitetura Ideal** (ver seção 2.2).

---

### 2.2. Arquitetura Ideal (Visão Evolutiva)

A arquitetura Ideal apresenta um cenário de evolução natural do sistema, contemplando:

- Frontend (SPA)
- API Gateway
- Identity Provider (OIDC)
- Redis para cache de leitura
- Stack de Observabilidade (Prometheus, Grafana, Loki, OpenTelemetry)

A arquitetura ideal reforça princípios de escalabilidade e separação de responsabilidades, mas não é exigida para o MVP.

### 2.3. Por que removemos a mensageria?

A decisão está documentada no **ADR-0007 – Remoção da Mensageria**.

Resumo:

- Mensageria introduzia complexidade sem benefício real para o domínio.
- Os serviços **não precisam** se comunicar entre si.
- Consolidação pode ser feita diretamente com SQL eficiente.
- MVP tem prazo reduzido e deve focar no essencial.
- Resiliência é garantida com:
  - retries
  - circuit breaker
  - timeouts
  - logging estruturado

A remoção da mensageria tornou o sistema mais simples, mais claro e mais alinhado aos requisitos.

---

## 3. Diagramas C4

Todos os diagramas estão disponíveis em:

```bash
/docs/diagramas/c4/
```

Inclui:

- `c4-context.puml` (ideal + mvp)
- `c4-container.puml` (ideal + mvp)
- `c4-container-mvp.puml`
- `c4-component-transaction-mvp.puml`
- `c4-component-consolidation-mvp.puml`
- `c4-component-ideal.puml`

Os diagramas distinguem elementos do **MVP** e do **Ideal** através de cores.

---

## 4. Estrutura do Repositório

```bash
/services
  /TransactionService
  /ConsolidationService
/tests
  /TransactionService.Tests
  /ConsolidationService.Tests
/deploy
  docker-compose.yml
  Dockerfile (por serviço)
/docs
  /diagramas
  /adr
  /imagens
```

---

## 5. Tecnologias e Princípios Adotados

### Backend

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- Polly (resiliência)
- FluentValidation (opcional)
- Serilog (opcional)

### Testes

- xUnit
- FluentAssertions
- Moq

### Arquitetura

- DDD (na medida adequada ao domínio)
- Clean Architecture
- SOLID
- C4 Model
- ADRs

### Infra

- Docker / Docker Compose

---

## 6. Como Executar o Projeto

Pré-requisitos:

- Docker + Docker Compose
- SDK .NET 8
- Git

### 6.1. Migrations e Inicialização Automática do Banco de Dados

Este projeto utiliza Entity Framework Core Migrations para criar e atualizar automaticamente o esquema do banco PostgreSQL durante a inicialização do sistema.

#### 6.1.1. Como funciona

- O banco PostgreSQL é iniciado via Docker Compose.
- O TransactionService aplica automaticamente suas migrations no momento do startup, criando as tabelas necessárias caso ainda não existam.
- O ConsolidationService realiza apenas leitura e não aplica migrations no MVP.
- Não é necessário executar scripts manuais, comandos de CLI ou preparar o banco previamente.

#### 6.1.2. Gerar os migrations

```bash
cd services/TransactionService
dotnet ef migrations add InitialCreate -o Infrastructure/Persistence/Migrations
```

#### 6.1.3 Para executar o sistema

Basta rodar:

```bash
docker compose up --build
```

O ambiente será iniciado com:

- Banco criado
- Tabelas aplicadas
- Microsserviços funcionando e saudáveis

#### 6.1.4. Por que adotamos este modelo?

- Facilita a execução do projeto pelo avaliador (zero passos manuais)
- Garante consistência entre ambientes
- Evita divergências de schema
- Mantém o processo simples no MVP, mas compatível com pipelines de CI/CD

### 6.2. Rodar testes

```bash
dotnet test
```

### 6.3. Endpoints

- `POST /api/v1/transactions`
- `GET /api/vi/consolidations/daily?merchantId=123&date=2025-12-01`

## 7. Decisões Arquiteturais (ADRs)

local:

```bash
/docs/adr/
```

Principais:

- ADR-0001 — arquitetura Inicial
- ADR-0005 — Estratégia de Autenticação
- ADR-0006 — Gerenciamento de Secredos
- ADR-0008 — Remoção da Mensageria (decisão crítica)

## 8. Roadmap de Evolução

- Adicionar frontend SPA
- Adicionar API Gateway
- Implementar OIDC/OAuth2
- Adicionar Redis para cache
- Implementar observabilidade completa
- Criar pipelines CI/CD
- Dividir banco em schemas conforme amadurecimento do domínio

## 9. Licença MIT

Projeto público para avaliação técnica.
