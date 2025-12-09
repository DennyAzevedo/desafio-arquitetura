# ADR 0005 — Estratégia de Autenticação (JWT no MVP, OIDC na Arquitetura Ideal)

## Status

Aceito

## Contexto

O sistema Cash Flow será exposto via APIs HTTP e, na visão ideal, também via um FrontEnd Web (SPA) para interação com o comerciante.
A autenticação e autorização são requisitos fundamentais para produção, mas o desafio possui prazo curto e foco principal em arquitetura,
resiliência e integração entre serviços.

São opções avaliadas:

- **JWT manual/simples** (chave simétrica, emissão controlada pela própria aplicação ou por um emissor simples).
- **OIDC (OpenID Connect)**, com provedor dedicado (Keycloak, Auth0, Azure AD, etc.).

## Decisão

1. **MVP**
   - Utilizar **JWT simples**, validado pelos serviços.
   - A chave de assinatura será configurada via variável de ambiente.
   - As APIs podem, inclusive, ser expostas sem autenticação obrigatória, desde que a estratégia JWT esteja documentada
     e com ganchos prontos para ser ativada.

2. **Arquitetura Ideal**
   - Adotar **OIDC como padrão de autenticação e autorização**, via provider externo:
     - Ex.: Keycloak, Auth0, Azure AD.
   - O FrontEnd realizará o fluxo OAuth2/OIDC (Authorization Code Flow).
   - O API Gateway validará o token recebido do FrontEnd e encaminhará somente requisições autenticadas aos microsserviços.
   - Microsserviços validam o token de forma leve (claims, roles, escopos).

## Justificativa

- **MVP**:
  - Prazo curto de desenvolvimento.
  - Evita implementar e configurar um Identity Provider completo.
  - Permite demonstrar claramente a intenção arquitetural, sem elevar a complexidade operacional.

- **Ideal**:
  - OIDC é padrão amplamente aceito e compatível com ambientes corporativos.
  - Separa claramente responsabilidades: Identity Provider cuida de autenticação, microsserviços focam em lógica de negócio.
  - Facilita integração futura com outros sistemas, SSO e controle centralizado de usuários.

## Consequências

- **MVP**
  - Menos segurança e governança em comparação com OIDC completo.
  - A responsabilidade de emissão/validação de tokens ainda pode estar acoplada à aplicação.
  - Importante documentar que é um passo provisório, não a solução final de produção.

- **Ideal**
  - Exige provisionamento de um Identity Provider.
  - Aumenta a complexidade de infraestrutura, mas oferece segurança, observabilidade e padronização muito superiores.
  - Facilita auditoria, revogação de acesso, MFA e requisitos de compliance.

## Referências

- OAuth 2.1 / OpenID Connect Core
- Documentação de Keycloak, Auth0 e Azure AD
