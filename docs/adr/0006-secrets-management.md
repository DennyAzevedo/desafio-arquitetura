# ADR 0006 — Gestão de Segredos (Variáveis de Ambiente no MVP, Vault na Arquitetura Ideal)

## Status

Aceito

## Contexto

Os serviços do sistema Cash Flow precisam de acesso a segredos sensíveis, como:

- Connection strings de bancos PostgreSQL
- Credenciais de acesso à mensageria
- Chaves de assinatura de tokens (JWT)
- Eventuais tokens de acesso a serviços externos

Armazenar esses valores diretamente no código-fonte ou em arquivos versionados é uma prática insegura.
Ao mesmo tempo, o desafio possui prazo limitado e ambiente simplificado (Docker Compose para MVP).

## Decisão

1. **MVP**
   - Utilizar **variáveis de ambiente** para configuração de segredos.
   - Docker Compose definirá as variáveis necessárias.
   - Nenhum segredo deveria ser commitado no repositório, mas por questões de avaliações ficarão disponíveis.

2. **Arquitetura Ideal**
   - Utilizar **Hashicorp Vault** (ou solução equivalente fornecida pela nuvem) como fonte de segredos:
     - Armazenar connection strings, credenciais, chaves de criptografia.
     - Definir políticas por serviço (least privilege).
     - Utilizar tokens temporários ou métodos de autenticação adequados (Kubernetes Auth, AppRole, etc.).
   - Configurar os microsserviços para buscar segredos no Vault em tempo de inicialização ou sob demanda.

## Justificativa

- **MVP**
  - Simples de operar em ambiente local.
  - Compatível com Docker Compose.
  - Atende ao requisito mínimo de não expor segredos em código-fonte, mas por questões de avaliações ficarão disponíveis.

- **Ideal**
  - Atende requisitos de segurança corporativa.
  - Permite rotação de segredos sem necessidade de redeploy completo.
  - Facilita auditoria de acesso a segredos.
  - Reduz riscos em caso de vazamento do repositório.

## Consequências

- **MVP**
  - Não oferece rotação automática de segredos.
  - Não possui trilha de auditoria de quem acessou quais valores.
  - Adequado apenas para ambientes de desenvolvimento / testes controlados.

- **Ideal**
  - Introduz dependência operacional adicional (Vault).
  - Requer configuração de políticas, autenticação de serviços e infraestrutura adicional.
  - Em contrapartida, aumenta significativamente a segurança e a governança.

## Integração com a Arquitetura

- Transaction Service e Consolidation Service passam a ler:
  - Connection strings
  - Chaves de JWT
  - Outros segredos
  a partir do Vault em produção.

- O API Gateway também utilizará o Vault para:
  - Certificados TLS
  - Segredos de integração com o Identity Provider (OIDC).
