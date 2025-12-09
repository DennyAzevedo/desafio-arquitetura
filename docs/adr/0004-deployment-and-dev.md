# ADR 0004 — Estratégia de Deployment e Ambiente de Desenvolvimento

## Status

Aceito

## Contexto

Prazo curto exige ambiente que facilite execução local e demonstração. Também é importante demonstrar como a solução deve ser operada em produção.

## Decisão

- **Desenvolvimento local:** `docker compose` com containers:
  - transaction-service (imagem dev)
  - consolidation-service (imagem dev)
  - postgres (1 ou 2 instâncias/schemas)
  - rabbitmq (MVP)
  - redis (opcional)

- **CI:** GitHub Actions com steps: build, unit tests, integration tests (pelo menos um e2e)

- **Produção (documentado):** Kubernetes com:
  - Deployments/StatefulSets
  - Kafka cluster (ou RabbitMQ cluster)
  - Vault/Secrets Manager para segredos
  - Prometheus + Grafana + tracing

## Justificativa

Docker Compose permite rodar stack completa localmente e facilita validação. K8s recomendado para produção.

## Consequências

- Manter `Dockerfile.dev` e `docker-compose.yml` atualizados.
- Documentar steps para deploy em k8s (manifestos/Helm charts).
