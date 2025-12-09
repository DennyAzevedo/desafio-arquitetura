# ADR 0001 — Arquitetura Inicial (Transaction + Consolidation)

## Status

Aceito

## Contexto

O requisito principal é controlar lançamentos (débito/crédito) e disponibilizar um relatório de saldo diário consolidado.
Requisitos não-funcionais importantes:

- O serviço de controle de lançamentos **não deve ficar indisponível** se o sistema de consolidado diário falhar.
- Em picos, o consolidado recebe 50 req/s com no máximo 5% de perda.

O tempo para execução é limitado (entrega parcial e documentação arquitetural aceita).

## Decisão

Adotar arquitetura baseada em **dois serviços desacoplados** em C# (.NET 8 / ASP.NET Core):

- **Transaction Service** (escrita)
  - Expor REST API para receber lançamentos.
  - Persistir transações em PostgreSQL.
  - Implementar **Outbox Pattern** (tabela `outbox_events`) para garantir entrega de eventos.
  - Background worker publica eventos no broker.

- **Consolidation Service** (leitura/agregação)
  - Consumir eventos (`TransactionCreated`) do broker.
  - Atualizar read model `daily_balances` (Postgres).
  - Expor endpoint de leitura `/consolidations/daily`.

- **Mensageria**
  - **MVP:** RabbitMQ.
  - **Ideal:** Kafka.

- **Infra**
  - docker-compose para dev local; documentar deploy em k8s para produção.

## Justificativa

- Desacoplamento garante que falhas no consolidado não impactem gravação de transações.
- Outbox evita perda de eventos quando broker estiver indisponível.
- RabbitMQ facilita entrega do MVP no prazo.

## Consequências

- Consistência eventual entre gravação e consolidado.
- Necessidade de monitorar e reprocessar outbox em caso de falhas.
- Dispatcher adicional para publicar eventos, aumentando complexidade mas melhorando robustez.
