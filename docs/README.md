# Cash Flow System — Arquitetura, MVP e Visão Ideal

Este projeto implementa uma arquitetura moderna, escalável e resiliente para o problema de **controle de lançamentos financeiros** e **consolidação diária de saldo**.  
A solução foi estruturada com base em princípios de _DDD_, _SOLID_, _Clean Architecture_, _Microsserviços_,
_resiliência_, _observabilidade_ e _segurança_, considerando:

- prazo de entrega (MVP funcional e realista);
- visão arquitetural completa (Ideal).

## Índice

- [Cash Flow System — Arquitetura, MVP e Visão Ideal](#cash-flow-system--arquitetura-mvp-e-visão-ideal)
  - [Índice](#índice)
  - [📌 1. Objetivo do Sistema](#-1-objetivo-do-sistema)
  - [🚀 2. Arquitetura Geral](#-2-arquitetura-geral)
    - [🔵 **2.1 MVP (Minimum Viable Product)**](#-21-mvp-minimum-viable-product)
      - [🔧 Tecnologias do MVP](#-tecnologias-do-mvp)
      - [🏛 Serviços no MVP](#-serviços-no-mvp)
    - [🔵 **2.2 Arquitetura Ideal (Visão Alvo)**](#-22-arquitetura-ideal-visão-alvo)
      - [🌐 FrontEnd (Ideal)](#-frontend-ideal)
      - [🔐 Segurança](#-segurança)
      - [🔑 Gestão de Segredos](#-gestão-de-segredos)
      - [📊 Observabilidade](#-observabilidade)
      - [🕸 Mensageria Ideal](#-mensageria-ideal)
      - [⚡ Escalabilidade](#-escalabilidade)
  - [🗂️ 3. Diagramas](#️-3-diagramas)
  - [🏗️ 4. Estrutura dos Projetos](#️-4-estrutura-dos-projetos)
  - [⚙️ 5. Como Executar o MVP Localmente](#️-5-como-executar-o-mvp-localmente)
    - [5.1 Subir a infraestrutura](#51-subir-a-infraestrutura)
    - [5.2 Endpoints](#52-endpoints)
  - [🧩 6. Decisões Arquiteturais (ADRs)](#-6-decisões-arquiteturais-adrs)
  - [🔹 6.1 Objetivos dos Testes no MVP](#-61-objetivos-dos-testes-no-mvp)
  - [🔹 6.2 Tipos de Testes Implementados](#-62-tipos-de-testes-implementados)
    - [✔ **Testes Unitários**](#-testes-unitários)
    - [✔ **Testes de Integração** (MVP)](#-testes-de-integração-mvp)
  - [🔹 6.3 Plano de Evolução dos Testes (Arquitetura Ideal)](#-63-plano-de-evolução-dos-testes-arquitetura-ideal)
    - [🧱 Testes de Contrato (Ideal)](#-testes-de-contrato-ideal)
    - [🧱 Testes End-to-End](#-testes-end-to-end)
    - [🧱 Testes de Performance](#-testes-de-performance)
    - [🧱 Testes de Resiliência](#-testes-de-resiliência)
  - [🔹 6.4 Motivação Arquitetural](#-64-motivação-arquitetural)
  - [🔹 6.5 Como Executar os Testes](#-65-como-executar-os-testes)
    - [Rodar apenas os testes:](#rodar-apenas-os-testes)
  - [🔹 6.6 Status Atual do MVP](#-66-status-atual-do-mvp)
  - [🔹 6.7 Conclusão](#-67-conclusão)
  - [🧩 7. Decisões Arquiteturais (ADRs)](#-7-decisões-arquiteturais-adrs)
    - [Lista de ADRs](#lista-de-adrs)
  - [🛣️ 8. Roadmap de Evolução](#️-8-roadmap-de-evolução)
    - [🟢 MVP](#-mvp)
    - [🔵 Ideal](#-ideal)
  - [✔️ 9. Conclusão](#️-9-conclusão)

---

## 📌 1. Objetivo do Sistema

O sistema permite:

- Registrar lançamentos de **crédito** e **débito** diariamente.
- Consolidar automaticamente o saldo diário.
- Expor consultas rápidas para que o usuário (comerciante) acompanhe seu fluxo de caixa.

---

## 🚀 2. Arquitetura Geral

A arquitetura foi modelada em duas versões:

---

### 🔵 **2.1 MVP (Minimum Viable Product)**

O MVP é focado no essencial, garantindo:

- funcionalidade ponta a ponta,
- resiliência via mensageria,
- escalabilidade horizontal,
- documentação completa.

#### 🔧 Tecnologias do MVP

| Camada | Tecnologia |
|--------|------------|
| Linguagem | C# / .NET 8 |
| API Services | ASP.NET Core |
| Mensageria | RabbitMQ |
| Banco Write | PostgreSQL |
| Banco Read | PostgreSQL |
| Migração de DB | Liquibase (changelog básico) |
| Observabilidade | Serilog |
| Containerização | Docker Compose |
| Testes | xUnit |

#### 🏛 Serviços no MVP

- **Transaction Service**  
  Recebe lançamentos, grava no banco e adiciona eventos no Outbox.

- **Outbox Dispatcher Worker**  
  Publica eventos `TransactionCreated` no RabbitMQ de forma resiliente.

- **Consolidation Service**  
  Consome eventos, atualiza o modelo de leitura e expõe consultas de saldo diário.

---

### 🔵 **2.2 Arquitetura Ideal (Visão Alvo)**

#### 🌐 FrontEnd (Ideal)

- Aplicação Web (React/Angular/Vue/Blazor)
- Autenticação via **OIDC**
- Comunicação apenas com o **API Gateway**

#### 🔐 Segurança

- OIDC + JWT assinados
- OAuth2 Authorization Code Flow

#### 🔑 Gestão de Segredos

- **Vault** (Hashicorp)

#### 📊 Observabilidade

- OpenTelemetry, Prometheus, Grafana, Loki

#### 🕸 Mensageria Ideal

- Kafka

#### ⚡ Escalabilidade

- Kubernetes

---

## 🗂️ 3. Diagramas

Os diagramas (C4 e BPMN) estão na pasta:

```bash
/diagramas
  /c4
  /bpmn
```

- Context
- Containers (MVP e Ideal)
- Componentes (MVP e Ideal)

---

## 🏗️ 4. Estrutura dos Projetos

```bash
/services
  /TransactionService
  /ConsolidationService
/docs
  /diagramas
  /adr
  /imagens
/database
  /transaction
  /consolidation
/deploy
```

---

## ⚙️ 5. Como Executar o MVP Localmente

### 5.1 Subir a infraestrutura

```bash
docker compose up -d
```

### 5.2 Endpoints

| Serviço | URL |
|---------|------|
| Transaction API | `http://localhost:5001/swagger` |
| Consolidation API | `http://localhost:5002/swagger` |
| RabbitMQ UI | `http://localhost:15672` |

---

## 🧩 6. Decisões Arquiteturais (ADRs)

A estratégia de testes do MVP foi definida para garantir qualidade mínima, validar o comportamento essencial dos serviços e permitir evolução segura da arquitetura.

Os testes estão localizados em:

```bash
/tests
  /TransactionService.Tests
  /ConsolidationService.Tests
```

---

## 🔹 6.1 Objetivos dos Testes no MVP

- Validar regras essenciais de domínio.
- Garantir que um lançamento seja armazenado corretamente.
- Garantir que um evento `TransactionCreated` seja gravado no Outbox.
- Confirmar que o Consolidation Service atualiza o saldo diário corretamente.
- Reduzir regressões durante evolução do MVP para a arquitetura ideal.

---

## 🔹 6.2 Tipos de Testes Implementados

### ✔ **Testes Unitários**

Local: `TransactionService.Tests` e `ConsolidationService.Tests`

Cobrem:

- Validação de dados de entrada
- Regra de negócio de lançamento (crédito/débito)
- Cálculo de saldos consolidados
- Processamento de eventos sintéticos

Ferramentas:

- **xUnit**
- **FluentAssertions** (opcional)
- **Moq** (para mocks simples)

---

### ✔ **Testes de Integração** (MVP)

Local: `TransactionService.Tests/Integration`

Incluem:

- Teste de gravação real no banco PostgreSQL usando container (ou DB em memória)
- Teste do Outbox Pattern:
  - gravação do evento
  - leitura pelo dispatcher
- Teste do fluxo completo de consolidação:
  - inserção de evento → atualização do Read Model

Ferramentas:

- **Testcontainers** (opcional, recomendável)
- **Docker Compose** (infra real)

---

## 🔹 6.3 Plano de Evolução dos Testes (Arquitetura Ideal)

Na visão ideal, os testes serão expandidos para:

### 🧱 Testes de Contrato (Ideal)

- Pact ou Postman Collections versionadas
- Garantem compatibilidade entre microsserviços

### 🧱 Testes End-to-End

- Simulação real: FrontEnd → Gateway → Transaction → Kafka → Consolidation → API de leitura

### 🧱 Testes de Performance

- Gatling ou k6
- Validam SLA de 50 req/s com perda < 5%

### 🧱 Testes de Resiliência

- Chaos engineering (chaos-mesh/chaos-monkey)
- Failover de mensageria
- Queda temporária de serviços

---

## 🔹 6.4 Motivação Arquitetural

Testes são estruturados para:

- reforçar o isolamento de responsabilidade entre serviços,
- garantir resiliência da comunicação assíncrona,
- proteger regras essenciais do domínio financeiro,
- validar integridade do Outbox Pattern.

---

## 🔹 6.5 Como Executar os Testes

### Rodar apenas os testes:

```bash
dotnet test

docker compose up -d
dotnet test

```

## 🔹 6.6 Status Atual do MVP

| Tipo de Teste | Status                       |
| ------------- | ---------------------------- |
| Unitários     | ✔ Implementados              |
| Integração    | ✔ Parcialmente implementados |
| Contrato      | ❌ Futuro                     |
| E2E           | ❌ Futuro                     |
| Performance   | ❌ Futuro                     |
| Resiliência   | ❌ Futuro                     |

## 🔹 6.7 Conclusão

A camada de testes do MVP cobre o essencial para garantir que o comportamento crítico dos microsserviços funcione corretamente e que a arquitetura possa evoluir de forma segura para o modelo ideal.

## 🧩 7. Decisões Arquiteturais (ADRs)

As decisões arquiteturais que fundamentam o projeto estão documentadas no diretório:

```bash
/docs/adr
```

### Lista de ADRs

| ADR | Título | Descrição |
|-----|--------|-----------|
| `0001` | Arquitetura Inicial | Estrutura fundamental baseada em dois microsserviços. |
| `0002` | Escolha da Mensageria | RabbitMQ no MVP, Kafka no Ideal. |
| `0003` | Outbox Pattern | Garantia de entrega de eventos. |
| `0004` | Estratégia de Deploy & Ambientes | Docker Compose no MVP, visão futura com Kubernetes. |
| `0005` | Estratégia de Autenticação | JWT no MVP; OIDC na arquitetura ideal. |
| `0006` | Gestão de Segredos | Variáveis de ambiente no MVP; Vault na arquitetura ideal. |

Cada ADR apresenta:

- o contexto da decisão,  
- a solução adotada,  
- justificativa,  
- consequências no MVP e na visão ideal.  

Esses documentos articulam claramente os trade-offs técnicos do projeto e mostram o caminho de evolução arquitetural.

---

## 🛣️ 8. Roadmap de Evolução

### 🟢 MVP

- Transaction Service
- Consolidation Service
- RabbitMQ
- Outbox
- Liquibase inicial
- Read Model diário
- Testes essenciais

### 🔵 Ideal

- API Gateway
- FrontEnd SPA
- OIDC + OAuth2
- Kafka
- Observabilidade completa
- Vault
- Redis Cache
- Kubernetes

---

## ✔️ 9. Conclusão

Solução moderna, escalável e aderente ao desafio técnico, equilibrando o que é possível entregar no prazo com uma visão arquitetural robusta de longo prazo.
