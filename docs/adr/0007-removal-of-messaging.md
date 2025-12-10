# ADR-0008 – Revisão Arquitetural: Remoção da Mensageria

## Status

Aceito

---

## 1. Contexto

Na primeira versão arquitetural deste projeto, adotamos um modelo baseado em **comunicação assíncrona via mensageria**, utilizando RabbitMQ como broker de eventos entre:

- `TransactionService` (Write Model)
- `ConsolidationService` (Read Model)

Essa abordagem fazia parte de um design mais elaborado, com padrão **Outbox**, consumidores assíncronos, workers, e dois bancos separados (write/read).

O modelo simulava uma arquitetura orientada a eventos, com foco em escalabilidade e desacoplamento.

Porém, após reavaliar profundamente o domínio e os requisitos do desafio, foi identificado que:

- essa complexidade **não gerava benefícios reais** dentro do escopo atual;
- a mensageria introduzia **sobrecarga operacional e cognitiva** desnecessária;
- o domínio é simples, sincrônico e não exige processamento distribuído.

Assim, tornou-se necessário reavaliar a decisão.

---

## 2. Problema

A mensageria adicionava vários elementos que não resolviam problemas reais do domínio:

- Necessidade de coordenação de dois bancos distintos (write/read)
- Manutenção de workers, filas, consumidores e dispatchers
- Ponto adicional de falha (broker)
- Adoção do padrão Outbox apenas para sustentar a fila
- Custo extra para testes e infraestrutura
- Aumento da complexidade para o MVP, contrariando o tempo limitado do desafio

E, sobretudo:

📌 **Os microsserviços não dependiam um do outro para nenhuma operação crítica.**  
📌 **Não havia necessidade de comunicação indireta.**  
📌 **Não existia processo assíncrono que justificasse mensageria.**  
📌 **O cálculo do consolidado pode ser feito diretamente no banco.**

Ou seja, a arquitetura estava se afastando do princípio:

> *A solução mais simples que resolve o problema é a melhor solução.*

---

## 3. Decisão

**Remover completamente o uso de mensageria (RabbitMQ) na arquitetura do projeto.**

Isso inclui:

- Remoção do RabbitMQ do docker-compose
- Remoção do padrão Outbox
- Remoção do OutboxDispatcher
- Remoção do RabbitMqConsumer e EventProcessor
- Unificação do banco de dados (com schemas ou tabelas separadas)
- Atualização dos diagramas C4
- Ajuste do README e demais documentações
- Ajuste a solução para um modelo sincrônico simples e eficiente

Os dois microsserviços permanecem independentes, mas agora acessam a mesma base de dados (com limitações por contexto), e o consolidado é calculado via SQL diretamente.

---

## 4. Justificativa

A decisão foi tomada pelos seguintes motivos:

### ✔ 4.1. Redução de complexidade

A remoção de mensageria elimina diversos componentes que não agregavam valor:

- Broker
- filas
- workers
- outbox
- reprocessamento
- dead lettering
- sincronização de estados

### ✔ 4.2. Aderência ao domínio real

O domínio do problema é simples:

- O usuário lança transações manualmente.
- O consolidado é apenas uma consulta agregada.
- Não existe necessidade de desacoplamento temporal.

### ✔ 4.3. Escalabilidade horizontal mantida

Cada microsserviço continua podendo escalar individualmente.

### ✔ 4.4. Resiliência garantida de forma mais eficiente

Em vez de mensageria:

- retries → Polly
- circuit breaker → Polly
- fallback → cache Redis
- timeouts → API resiliente

### ✔ 4.5. MVP mais adequado ao prazo e ao desafio

O desafio enfatiza:

- clareza arquitetural
- boas práticas
- justificativas
- documentação

E não complexidade excessiva.

---

## 5. Consequências

### 5.1. Positivas

- Arquitetura mais simples e estável
- Menos pontos de falha
- Infraestrutura menor
- Deploy mais rápido
- Mais tempo para focar em regras de negócio
- Documentação mais clara
- Menos código acoplado a infraestrutura

### 5.2. Negativas

- Perda da possibilidade de futuras integrações event-driven
- Cálculos passam a ser síncronos (embora leves para este domínio)

### 5.3. Mitigações

Caso o domínio evolua no futuro, podemos:

- reintroduzir mensageria
- implementar um modelo CQRS completo
- manter um módulo de publish/subscribe

Mas isso só aconteceria *com motivos reais*.

---

## 6. Alternativas Consideradas

### ❌ Manter mensageria

Rejeitada por adicionar complexidade desnecessária.

### ❌ Usar Outbox sem mensageria

Sem utilidade — Outbox existe apenas para publicar eventos com consistência.

### ✔ Unificar banco e fazer consultas diretas

Aceita como modelo sólido e simples.

---

## 7. Conclusão

A remoção da mensageria alinha o projeto com:

- Simplicidade arquitetural
- Objetividade
- Resiliência
- Declaração explícita dos trade-offs
- Melhores práticas de arquitetura
- Uma solução que realmente corresponde ao domínio apresentado

---
