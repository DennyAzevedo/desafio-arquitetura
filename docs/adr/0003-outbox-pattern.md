# ADR 0003 — Uso do Outbox Pattern

## Status

Aceito

## Contexto

Para garantir atomicidade entre persistência de transação e emissão do evento (evitar situações onde a transação é gravada mas o evento não é publicado), adotaremos o Outbox Pattern.

## Decisão

- Implementar uma tabela `outbox_events` no mesmo banco da Transaction Service.
- Durante a mesma transação que grava `transactions`, inserir o evento serializado em `outbox_events`.
- Background worker (Outbox Dispatcher) lê registros não publicados e publica no broker; marca como publicado.

## Justificativa

Garante confiabilidade sem depender de transações distribuídas (2PC). Evita perda de mensagens quando broker estiver temporariamente indisponível.

## Consequências

- Complexidade de implementação do dispatcher.
- Necessidade de monitorar lag/outbox queue e alertar.
