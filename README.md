# Desafio de Arquitetura de Software

## Objetivos do Desafio

Desenvolver uma arquitetura de software escalável e resiliente, garantindo alta disponibilidade, segurança e desempenho. Isso inclui a seleção adequada de padrões arquiteturais, integração de tecnologias e frameworks, além de otimização de requisitos não-funcionais. Deve abranger aspectos importantes, como design, integração, segurança e desempenho.

- **Escalabilidade:** Garanta que a arquitetura possa lidar com o aumento da carga de trabalho sem degradação significativa do desempenho. Con-
sidere dimensionamento horizontal, balanceamento de carga e estratégias de cache.

- **Resiliência:** Projete para a recuperação de falhas. Isso inclui redundância, failover, monitoramento proativo e estratégias de recuperação.

- **Segurança:** Proteja os dados e sistemas contra ameaças. Implemente autenticação, autorização, criptografia e mecanismos de proteção contra ataques.

- **Padrões Arquiteturais:** Escolha padrões adequados, como microsserviços, monolitos, SOA ou serverless. Considere trade-offs entre simplicidade e flexibilidade.

- **Integração:** Defina como os componentes se comunicarão. Avalie protocolos, formatos de mensagem e ferramentas de integração.

- **Requisitos Não-Funcionais:** Otimize para desempenho, disponibilidade e confiabilidade. Defina métricas e metas claras.

- **Documentação:** Registre decisões arquiteturais, diagramas e fluxos de dados. Isso facilita a comunicação e a manutenção.

## Estrutura do Repositório

- **Pasta database:** Contém os arquivos do Liquibase para a geração das tabelas.
- **Pasta deploy:** Contém os arquivos yaml dos dockers utilizados.
- **Pasta diagramas:** Contém os arquivos do C4 Model dos diagramas de arquitetura (Contexto, Containers e Componentes) e BPMN.
- **Pasta docs:** Contém as documentações bases do desafio, ADRs e demais orientações.
- **Pasta imagens:** Contém as imagens usadas nas documentações.
- **Pasta src:** Contém os arquivos do MVP.

## Ferramentas Utilizadas

- Visual Studio Code: geração das documentações
- Visual Studio: desenvolvimento e testes do codigo do MVP.

Autor: **Denny Paulista Azevedo Filho**
