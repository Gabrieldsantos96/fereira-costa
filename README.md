# Developer Evaluation Project

## Instruções para Executar o Projeto

> **Observação**: Recomenda-se o uso do **Visual Studio Community** para executar o projeto.

### 1. Configuração do Arquivo de Variáveis de Ambiente
- As variáveis de ambiente são carregadas automaticamente a partir das **GitHub Actions** para o ambiente de desenvolvimento.
- Caso precise configurar variáveis localmente, solicite o arquivo de configuração ao recrutador por e-mail (`gabrielk6.mobile@gmail.com`) ou WhatsApp (`(35) 99196-9303`).
- Cole o conteúdo do arquivo em `Fereira.Costa.Server/appsettings.Development.json`.

### 2. Configuração do Ambiente Local
- Defina o **docker-compose** como o projeto de inicialização (`Startup Item`) no Visual Studio.
- Ao executar o projeto com `docker-compose`, todas as dependências serão configuradas automaticamente, incluindo:
  - Banco de dados **SQL Server** com migrations e seed aplicados.

### 3. Execução do Projeto
- O projeto roda localmente utilizando **Docker Compose** para testes.
- Certifique-se de que o **docker-compose** está configurado como o arquivo de inicialização no Visual Studio.
- Após a execução, a API estará disponível em:  
  [https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/swagger/index.html#/Auth/FereiraCostaServerControllersAuthenticationSignInEndpoint](https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/swagger/index.html#/Auth/FereiraCostaServerControllersAuthenticationSignInEndpoint)
- O frontend estará disponível em:  

  O frontend está ocultando a URI do backend como medida de boas práticas usando proxy:  
  [https://witty-dune-044b72a0f.2.azurestaticapps.net/](https://witty-dune-044b72a0f.2.azurestaticapps.net/)

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
    export const Route = createFileRoute("/_authenticated/_authenticated/users/")({
      validateSearch: userSearchSchema,
      component: Authorize(RouteComponent, [IUserRole.USER]),
    });
    ```
  - Apesar de implementada, a funcionalidade de autorização não está sendo utilizada no momento.

### Backend
- **Padrões e Bibliotecas**:
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
  - Utilizados para configurar e subir o ambiente local automaticamente.
- **CI/CD**:
  - Pipeline de integração e entrega contínua implementado com Azure DevOps.
- **Proxy Reverso**:
  - Configurado no React Vite para redirecionar requisições ao backend, garantindo que não sejam expostas diretamente ao cliente e sejam processadas pelo Node.js.

### Swagger
- A documentação da API está disponível publicamente no Swagger:  
  [https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/swagger/index.html#/Auth/FereiraCostaServerControllersAuthenticationSignInEndpoint](https://fereira-costa-api-grhde5avgnd6ecck.eastus2-01.azurewebsites.net/swagger/index.html#/Auth/FereiraCostaServerControllersAuthenticationSignInEndpoint)

## Notas Adicionais
- Certifique-se de que o ambiente Docker está corretamente configurado antes de executar o projeto.
- Para dúvidas ou suporte, entre em contato com o recrutador via e-mail (gabrielk6.mobile@gmail.com) ou WhatsApp ((35) 99196-9303).

---

## Instruções para Uso no Git

1. **Salvar o Arquivo**:
   - Copie o conteúdo acima.
   - Crie (ou substitua) um arquivo chamado `README.md` na raiz do seu repositório.
   - Cole o conteúdo no arquivo e salve.

2. **Commit no Git**:
   ```bash
   git add README.md
   git commit -m "Atualiza README com instruções revisadas e organizadas"
   git push origin main
