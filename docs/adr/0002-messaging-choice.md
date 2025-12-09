# ADR 0002 — Escolha de Mensageria (RabbitMQ para MVP; Kafka ideal)

## Status

Aceito (MVP: RabbitMQ; Ideal: Kafka)

## Contexto

A comunicação entre Transaction e Consolidation deve ser assíncrona, durável e escalável. Para o MVP precisamos de solução simples para rodar localmente e demonstrar o comportamento exigido pelo teste.

## Decisão

- **MVP:** RabbitMQ
  - Vantagens: configuração simples,  (Advanced Message Queuing Protocol), rápido para testar localmente com Docker.
  - Suficiente para volumes médios e demonstração do padrão Outbox/consumer group.

- **Ideal (produção):** Kafka
  - Vantagens: particionamento, escalabilidade horizontal, retenção de eventos, fácil reprocessamento histórico.

## Justificativa

RabbitMQ permite desenvolvimento com menor overhead; Kafka é recomendado para cenários de alto throughput e necessidade de retenção histórica.

## Consequências

- Projetar schemas de mensagem e versionamento (JSON v1 ou Avro/Protobuf).
- Implementar DLQ e reprocessamento.
