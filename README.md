# Developer Evaluation Project

## Instruções para Executar o Projeto

> **Observação**: Recomenda-se o uso do **Visual Studio Community** para executar o projeto.

### 1. Variáveis de Ambiente

- As variáveis de ambiente são carregadas automaticamente a partir das **GitHub Actions** para ambiente prod.
- O arquivo de configuração já está devidamente preparado para uso local.

### 2. Configuração do Ambiente Local

- O projeto utiliza **Docker Compose** para rodar localmente, garantindo que todas as dependências, incluindo o banco de dados **SQL Server** com migrations e seed aplicados, sejam configuradas automaticamente.
- Defina o **docker-compose** como o projeto de inicialização (`Startup Item`) no Visual Studio.

### 3. Execução do Projeto

- Rode o projeto localmente via **Docker Compose** para testes e desenvolvimento.
- Certifique-se que o **docker-compose** está configurado como arquivo de inicialização no Visual Studio.
- Após a execução, a API estará disponível em:  
  [https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/swagger/index.html#/Auth/FereiraCostaServerControllersAuthenticationSignInEndpoint](https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/swagger/index.html#/Auth/FereiraCostaServerControllersAuthenticationSignInEndpoint)
- O frontend estará disponível em:

  O frontend URI:  
  [https://witty-dune-044b72a0f.2.azurestaticapps.net/](https://witty-dune-044b72a0f.2.azurestaticapps.net/)

### 4. Deploy em Produção

- OBS: Azure SQL Server está em um ambiente serverless por se tratar de um instancia free tier, isso implica que a primeira request pode demorar mais do que o normal por causa do cold start.

- O projeto já está configurado com pipeline de **CI/CD** via **Azure DevOps**, garantindo deploys automáticos, testes e entrega contínua em ambiente de produção.

## Detalhes do Projeto

### Frontend

- **Tecnologias Utilizadas**:
  - **React** com **Vite** (Single Page Application - SPA)
  - **Shadcn** como biblioteca de componentes
  - **Tailwind CSS** para estilização
  - **React Query** e **TanStack Query** para gerenciamento de estado e requisições do lado do cliente
  - **Zod** para validação de formulários
- **Autorização**:
  - Foi implementada uma estratégia de autorização no frontend com a função `Authorize`, que pode ser facilmente utilizada para proteger rotas. Exemplo:
    ```typescript
    export const Route = createFileRoute(
      "/_authenticated/_authenticated/users/"
    )({
      validateSearch: userSearchSchema,
      component: Authorize(RouteComponent, [IUserRole.USER]),
    });
    ```
  - Apesar de implementada, a funcionalidade de autorização não está sendo utilizada no momento.

### Backend

- **Padrões e Bibliotecas**:
  - Por segurança os erros não estão sendo expostos no client ( Enxergar exceptions somente no Azure Monitor/Application Insights )
  - Padrão Mediator para comunicação entre camadas
  - Fluent Validations para validação de dados
  - Entity Framework como ORM para interação com o banco de dados
  - Identity para autenticação e autorização
  - Estratégia de refresh token armazenada no banco de dados
- **Testes**:
  - Testes unitários implementados exclusivamente na camada de negócio (domain).

### Banco de Dados

- SQL Server (relacional) utilizado como banco de dados principal.

### DevOps

- **Docker e Docker Compose**:
  - Usados para rodar o ambiente local de forma automatizada.
- **CI/CD**:
  - Pipeline de integração e entrega contínua implementado com Azure DevOps para deploy em produção.

### Swagger

- A documentação da API está disponível publicamente no Swagger:  
  [https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/swagger/index.html#/Auth/FereiraCostaServerControllersAuthenticationSignInEndpoint](https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/swagger/index.html#/Auth/FereiraCostaServerControllersAuthenticationSignInEndpoint)

## Notas Adicionais

- Certifique-se de que o ambiente Docker está corretamente configurado antes de executar o projeto.
- Para dúvidas ou suporte, entre em contato com o recrutador via e-mail (gabrielk6.mobile@gmail.com) ou WhatsApp ((35) 99196-9303).
