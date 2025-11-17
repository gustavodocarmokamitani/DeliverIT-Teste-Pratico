#  DeliverIT - Pagamento API
## Visão Geral da Solução

A solução adota o estilo **Arquitetura Limpa (Clean Architecture)** com organização em **Camadas**, aplicando o **Princípio da Inversão de Dependência (DIP)**. Isso garante que o **Domain** defina as interfaces do Repositório e permaneça completamente independente da Infraestrutura.

##  Estilo Arquitetural

### 1. Camadas da Arquitetura

| Projeto | Camada | Responsabilidade |
| :--- | :--- | :--- |
| **DeliverIT.Pagamento.Domain** | Core | Regras de Negócio, Entidades e Abstrações. |
| **DeliverIT.Pagamento.Application** | Orquestração | Orquestração do fluxo de dados e mapeamento de DTOs. |
| **DeliverIT.Pagamento.Infrastructure** | Detalhes | Implementação do Repository Pattern e persistência de dados via ORM. |
| **DeliverIT.Pagamento.API** | Apresentação | Ponto de entrada **REST**, Controllers, Roteamento e Injeção de Dependência. |

---

### 2. Princípios e Decisões de Design

| Decisão | Princípio / Padrão Aplicado | Objetivo |
| :--- | :--- | :--- |
| **Separação de Projetos** | Separação de Preocupações (SoC) | Código dividido em Domain, Application, Infrastructure e API para isolar responsabilidades e facilitar a manutenção. |
| **Lógica da Entidade** | Encapsulamento, SRP ( Single Responsability Principle ) | O cálculo de ValorCorrigido, DiasEmAtraso e RegraAplicada é encapsulado na Entidade, garantindo a integridade do Domínio. |
| **Repository Pattern** | Isolamento de Dados | Isolar a lógica de acesso a dados (EF Core) dos Serviços de Aplicação. |
| **Testes Automatizados** | TDD / Qualidade | Aplicação de testes unitários e de integração em todas as camadas. |
| **DTO de Listagem** | Princípio do Menor Conhecimento | Expor apenas os campos essenciais da Entidade na resposta da API. |

----

### 3. Contêineres e Tecnologias (Stack)

| Categoria | Tecnologia | Uso |
| :--- | :--- | :--- |
| **Framework** | **ASP.NET Core / .NET Core 3.0+** | Ambiente de execução para a API RESTful. |
| **Persistência** | **Entity Framework Core** | ORM e acesso a dados. |
| **Documentação** | **Swagger / OpenAPI** | Documentação e testes interativos da API. |
| **Testes** | **xUnit / Moq** | Testes Unitários (TDD) e Simulação de dependências. |
