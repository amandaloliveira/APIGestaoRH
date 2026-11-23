# APIGestaoRH — Sistema de Gestão de RH (.NET 10 + Entity Framework Core)

API REST desenvolvida em **ASP.NET Core (.NET 10)** para o gerenciamento de **Gestores** e **Funcionários**, estruturada sob o padrão arquitetural em camadas (**Controllers → Services → Repositories → ORM / SQLite**) baseado na arquitetura `v3-camada-servico`.

---

## 1. Tecnologias e Pacotes Utilizados (`APIGestaoRH.csproj`)

* **Framework:** `.NET 10.0` (`net10.0`) com Nullable Reference Types e Implicit Usings habilitados.
* **ORM e Banco de Dados:** `Microsoft.EntityFrameworkCore.Sqlite` (`10.0.14`) — driver que permite ao Entity Framework Core persistir dados no arquivo local SQLite (`gestaorh.db`), configurado via `ConnectionStrings:DefaultConnection` no `appsettings.json`.
* **Ferramentas de Migração:** `Microsoft.EntityFrameworkCore.Tools` (`10.0.14`) — suporte à geração e aplicação de *Migrations* para versionamento do esquema relacional.
* **Documentação Interativa:** `Swashbuckle.AspNetCore` (`10.2.3`) — gera a especificação OpenAPI e a interface web do **Swagger UI** em `/swagger` (configurada como rota inicial em `Properties/launchSettings.json`).

---

## 2. Estrutura de Pastas e Papel de Cada Arquivo do Projeto

```text
APIGestaoRH/
├── APIGestaoRH.slnx                      # Arquivo de solução do Visual Studio
└── APIGestaoRH/
    ├── Controllers/                      # Camada de entrada HTTP (endpoints REST)
    │   ├── FuncionariosController.cs     # CRUD de Funcionários + filtro por setor
    │   └── GestoresController.cs         # CRUD completo de Gestores
    ├── Services/                         # Camada de domínio, contratos e regras de negócio
    │   ├── IFuncionarioService.cs        # Contrato das operações de negócio de Funcionários
    │   ├── FuncionarioService.cs         # Validações de CPF, salário e existência de Gestor
    │   ├── IGestorService.cs             # Contrato das operações de negócio de Gestores
    │   ├── GestorService.cs              # Validações de e-mail único e bloqueio de exclusão 1:N
    │   ├── IFuncionarioRepository.cs     # Contrato de persistência de Funcionários
    │   ├── IGestorRepository.cs          # Contrato de persistência de Gestores
    │   └── RegraDeNegocioException.cs    # Exceção customizada convertida em HTTP 400
    ├── Data/                             # Camada de acesso a dados (ORM e alternativa em RAM)
    │   ├── AppDbContext.cs               # Sessão do EF Core e configuração Fluent API 1:N
    │   ├── FuncionarioRepositoryEf.cs    # Implementação EF Core/SQLite para Funcionários
    │   ├── FuncionarioRepositoryMemoria.cs # Implementação em memória (lista estática pré-populada)
    │   └── GestorRepositoryEf.cs         # Implementação EF Core/SQLite para Gestores (com Include)
    ├── Models/                           # Entidades de domínio e validações de entrada
    │   ├── Gestor.cs                     # Entidade lado "1" (um gestor lidera vários funcionários)
    │   └── Funcionario.cs                # Entidade lado "N" (vários funcionários por gestor)
    ├── Migrations/                       # Histórico versionado do esquema do banco de dados
    │   ├── 20261120034145_CriacaoInicial.cs # Script C# de criação das tabelas e FK
    │   └── AppDbContextModelSnapshot.cs  # Fotografia atual do modelo para futuras migrations
    ├── Properties/
    │   └── launchSettings.json           # Portas locais (http://localhost:5135 e https://localhost:7296)
    ├── Program.cs                        # Ponto de entrada, registro de DI e pipeline HTTP
    ├── appsettings.json                  # Configuração da string de conexão ("Data Source=gestaorh.db")
    ├── gestaorh.db                       # Banco de dados SQLite gerado pelo EF Core
    └── APIGestaoRH.http                  # Coleção de requisições HTTP prontas para teste
```

---

## 3. Os 5 Pilares Arquiteturais Aplicados no Código

### 3.1. ORM com Entity Framework Core
O **Entity Framework Core (EF Core)** atua como tradutor objeto-relacional (*Object-Relational Mapper*), convertendo classes e consultas C# em comandos SQL executados no SQLite sem necessidade de escrever SQL manualmente:

1. **Mapeamento de Tabelas (`Data/AppDbContext.cs`):** A classe `AppDbContext` herda de `DbContext` e expõe duas propriedades `DbSet<T>`:
   * `public DbSet<Gestor> Gestores => Set<Gestor>();` → mapeada para a tabela **`Gestores`**.
   * `public DbSet<Funcionario> Funcionarios => Set<Funcionario>();` → mapeada para a tabela **`Funcionarios`**.
2. **Tradução de Consultas LINQ para SQL (`Data/*RepositoryEf.cs`):**
   * `_context.Funcionarios.ToListAsync()` → traduzido para `SELECT * FROM Funcionarios`.
   * `_context.Funcionarios.FindAsync(id)` → busca otimizada pela chave primária (`Id`).
   * `_context.Funcionarios.Where(f => f.Setor.ToLower() == setor.ToLower()).ToListAsync()` → gera filtro `WHERE` insensível a maiúsculas/minúsculas.
   * `_context.Funcionarios.AnyAsync(...)` → gera consulta booleana (`SELECT EXISTS(...)`) usada para checar duplicidade de CPF ou existência de vínculos sem carregar registros inteiros na memória.
   * `_context.SaveChangesAsync()` → rastreia entidades adicionadas (`Add`), modificadas ou removidas (`Remove`) pelo *Change Tracker* do EF Core e executa os comandos `INSERT`, `UPDATE` ou `DELETE` dentro de uma transação atômica.
3. **Versionamento do Banco (`Migrations/20261120034145_CriacaoInicial.cs`):** Registra de forma reproduzível a criação das colunas (`Id` autoincremento, `Nome`, `Cpf`, `Cargo`, `Setor`, `Salario`, `GestorId`, `Email`), chaves primárias e índices de chave estrangeira.

---

### 3.2. Arquitetura em Camadas: Controller, Services e Repository Pattern
A aplicação separa claramente as responsabilidades em três camadas desacopladas, ilustradas pela analogia de um restaurante:

#### A) Controllers — O "Garçom" (`Controllers/`)
Recebem a requisição HTTP, validam automaticamente os atributos das Models (graças ao atributo `[ApiController]`), acionam o serviço correspondente e traduzem o resultado para códigos de status HTTP padronizados:
* `200 OK`: Listagens (`GET`) e consultas por `Id` ou `Setor` bem-sucedidas.
* `201 Created`: Cadastro (`POST`) concluído, retornando o objeto criado e o cabeçalho `Location` apontando para o `GET` individual (`CreatedAtAction`).
* `204 No Content`: Atualização (`PUT`) ou exclusão (`DELETE`) concluída com sucesso.
* `400 Bad Request`: Violação de validação de entrada (`[Required]`, `[Range]`) ou captura de `RegraDeNegocioException`.
* `404 Not Found`: Quando o serviço retorna `null` ou `false` indicando que o `Id` solicitado não existe no banco.

#### B) Services — O "Cozinheiro" (`Services/`)
Concentram toda a lógica de domínio e regras de negócio do RH, sem conhecer detalhes de HTTP ou de banco de dados:
* **`FuncionarioService.cs`:**
  1. **Unicidade de CPF:** Impede cadastrar dois funcionários com o mesmo CPF; no `PUT`, ignora o próprio `Id` sendo editado (`idIgnorado: id`) para permitir atualizar outros dados mantendo o mesmo CPF.
  2. **Salário Válido:** Exige que `Salario > 0`.
  3. **Integridade Referencial:** Verifica via `IGestorRepository.ExisteAsync(funcionario.GestorId)` se o gestor informado realmente existe antes de salvar.
* **`GestorService.cs`:**
  1. **Unicidade de E-mail:** Impede cadastrar dois gestores com o mesmo e-mail (comparação case-insensitive), ignorando o próprio registro durante o `PUT`.
  2. **Proteção de Integridade 1:N na Exclusão:** Antes de remover um gestor no `DELETE`, verifica via `IGestorRepository.PossuiFuncionariosAsync(id)` se há subordinados vinculados. Caso haja, lança `RegraDeNegocioException`, evitando erro de banco (`500`) e devolvendo uma mensagem amigável (`400 Bad Request`).

#### C) Repository Pattern — O "Estoque" (`Data/` + Interfaces em `Services/`)
Abstrai o mecanismo de persistência atrás de contratos (`IFuncionarioRepository` e `IGestorRepository`):
* **Persistência Real (`FuncionarioRepositoryEf` e `GestorRepositoryEf`):** Utilizam o `AppDbContext` para gravar no SQLite.
* **Persistência Alternativa em Memória (`FuncionarioRepositoryMemoria`):** Mantém uma lista estática em RAM com 3 funcionários pré-cadastrados (`Ana Souza`, `Bruno Lima`, `Carla Mendes`), demonstrando que é possível trocar toda a tecnologia de armazenamento sem alterar uma única linha dos `Controllers` ou `Services`.

---

### 3.3. Injeção de Dependência (Dependency Injection — DI)
Nenhuma classe instancia suas dependências diretamente com `new` (evitando acoplamento forte). Em vez disso, cada classe declara no construtor as **interfaces** de que precisa, e o contêiner nativo de Inversão de Controle (IoC) do ASP.NET Core resolve e injeta as implementações automaticamente.

**Onde está configurado (`Program.cs`):**
* `builder.Services.AddDbContext<AppDbContext>(...)`: Registra o contexto do banco com tempo de vida *Scoped*.
* `builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepositoryEf>()`: Cria uma instância do repositório por requisição HTTP, compartilhando o mesmo `AppDbContext` durante toda a requisição.
* `builder.Services.AddScoped<IGestorRepository, GestorRepositoryEf>()`: Registra o repositório de gestores no mesmo escopo.
* `builder.Services.AddScoped<IFuncionarioService, FuncionarioService>()` e `AddScoped<IGestorService, GestorService>()`: Registram os serviços de negócio.
* *(Alternativa comentada em `Program.cs`)*: `builder.Services.AddSingleton<IFuncionarioRepository, FuncionarioRepositoryMemoria>()` — usa `AddSingleton` porque a lista em memória precisa persistir entre diferentes requisições HTTP enquanto a aplicação estiver rodando.

**Fluxo de Injeção em uma Requisição:**
```text
HTTP Request → FuncionariosController(IFuncionarioService)
                    └── FuncionarioService(IFuncionarioRepository, IGestorRepository)
                             ├── FuncionarioRepositoryEf(AppDbContext)
                             └── GestorRepositoryEf(AppDbContext)
```

---

### 3.4. Relacionamento 1 para Muitos (1:N) entre Models com EF Core
O sistema modela a hierarquia organizacional em que **1 Gestor** gerencia **N Funcionários**, e **cada Funcionário** está subordinado a **1 único Gestor**:

1. **Lado "1" (`Models/Gestor.cs`):**
   * Contém a propriedade de navegação de coleção:
     ```csharp
     public List<Funcionario> Funcionarios { get; set; } = new();
     ```
2. **Lado "N" (`Models/Funcionario.cs`):**
   * Contém a chave estrangeira explícita:
     ```csharp
     [Required(ErrorMessage = "O Id do gestor é obrigatório.")]
     public int GestorId { get; set; }
     ```
   * Contém a propriedade de navegação inversa decorada com `[JsonIgnore]`:
     ```csharp
     [JsonIgnore]
     public Gestor? Gestor { get; set; }
     ```
   * **Por que usar `[JsonIgnore]`?** Evita referência circular infinita durante a serialização JSON (`Gestor → Funcionarios → Gestor → ...`) e dispensa o cliente da API de enviar o objeto `Gestor` aninhado no corpo do `POST` ou `PUT` (basta enviar `"gestorId": 1`).
3. **Configuração Explícita via Fluent API (`Data/AppDbContext.cs`):**
   ```csharp
   modelBuilder.Entity<Funcionario>()
       .HasOne(f => f.Gestor)
       .WithMany(g => g.Funcionarios)
       .HasForeignKey(f => f.GestorId)
       .OnDelete(DeleteBehavior.Restrict);
   ```
   O `DeleteBehavior.Restrict` cria a restrição `ON DELETE RESTRICT` no SQLite, impedindo exclusão em cascata acidental.
4. **Carregamento Antecipado / Eager Loading (`Data/GestorRepositoryEf.cs`):**
   * Nos métodos `ListarAsync()` e `ObterAsync(int id)`, o repositório utiliza `.Include(g => g.Funcionarios)`. Isso instrui o EF Core a gerar um `LEFT JOIN` para retornar cada gestor já acompanhado da lista completa de seus funcionários subordinados.

---

### 3.5. Processamento Assíncrono no ASP.NET Core
#### Como funciona internamente
O servidor web do ASP.NET Core (**Kestrel**) atende às requisições HTTP utilizando um conjunto finito de *threads* gerenciado pelo **.NET ThreadPool**. Operações de entrada/saída (I/O) — como leitura e escrita em disco no banco SQLite — são ordens de magnitude mais lentas do que operações em CPU.

Quando um método assíncrono (`async`) executa uma operação de I/O com `await`:
1. A execução inicia na *thread* atribuída à requisição HTTP.
2. Ao atingir o `await` de uma operação real de I/O, o método é suspenso e a *thread* **não fica bloqueada esperando o disco**: ela é devolvida imediatamente ao *ThreadPool* para atender outras requisições simultâneas.
3. Assim que o SQLite conclui a leitura ou gravação, o sistema operacional notifica o *runtime* do .NET, que aloca uma *thread* disponível do *ThreadPool* para retomar o método exatamente do ponto onde parou.

#### Onde ocorre na aplicação (de ponta a ponta)
* **Camada Web (`Controllers/`):** Todos os endpoints retornam `Task<ActionResult<T>>` ou `Task<IActionResult>` (ex.: `public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()`), aguardando o serviço com `await`.
* **Camada de Serviço (`Services/`):** Encadeia chamadas assíncronas (ex.: em `FuncionarioService.CriarAsync`, aguarda primeiro `await ValidarRegrasAsync(...)` e depois `await _repositorio.SalvarAsync()`).
* **Camada de Dados EF Core (`Data/*RepositoryEf.cs`):** É onde a liberação real da *thread* para I/O acontece, através dos métodos assíncronos do Entity Framework Core (`ToListAsync()`, `FindAsync()`, `FirstOrDefaultAsync()`, `AnyAsync()` e `SaveChangesAsync()`).
* **Contraste com o Repositório em Memória (`Data/FuncionarioRepositoryMemoria.cs`):** Como consultar uma `List<Funcionario>` na memória RAM não envolve I/O externo, os métodos executam de forma síncrona e utilizam `Task.FromResult(...)` e `Task.CompletedTask` apenas para cumprir o contrato assíncrono definido em `IFuncionarioRepository`.

---

## 4. Mapa de Endpoints Disponíveis na API

### Gestores (`/api/Gestores`)
| Método | Rota | Descrição | Retornos Possíveis |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Gestores` | Lista todos os gestores e suas respectivas listas de funcionários (`Include`) | `200 OK` |
| `GET` | `/api/Gestores/{id}` | Busca um gestor específico pelo `Id` com seus subordinados | `200 OK`, `404 Not Found` |
| `POST` | `/api/Gestores` | Cadastra um novo gestor (valida nome, setor, e-mail obrigatório e e-mail único) | `201 Created`, `400 Bad Request` |
| `PUT` | `/api/Gestores/{id}` | Atualiza nome, setor e e-mail de um gestor existente | `204 No Content`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/Gestores/{id}` | Remove um gestor (bloqueado se houver funcionários vinculados a ele) | `204 No Content`, `400 Bad Request`, `404 Not Found` |

### Funcionários (`/api/Funcionarios`)
| Método | Rota | Descrição | Retornos Possíveis |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Funcionarios` | Lista todos os funcionários cadastrados | `200 OK` |
| `GET` | `/api/Funcionarios/{id}` | Busca um funcionário específico pelo `Id` | `200 OK`, `404 Not Found` |
| `GET` | `/api/Funcionarios/setor/{setor}` | Filtra funcionários pertencentes a um determinado setor | `200 OK` |
| `POST` | `/api/Funcionarios` | Cadastra um funcionário vinculado a um `GestorId` existente | `201 Created`, `400 Bad Request` |
| `PUT` | `/api/Funcionarios/{id}` | Atualiza os dados do funcionário (cargo, salário, setor, gestor, etc.) | `204 No Content`, `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/Funcionarios/{id}` | Remove o funcionário do banco de dados | `204 No Content`, `404 Not Found` |

---

## 5. Como Executar e Testar Localmente

1. **Requisitos:** SDK do **.NET 10** instalado.
2. **Iniciar a API:**
   * **Pelo Visual Studio:** Abra `APIGestaoRH.slnx` e pressione `F5` (o navegador abrirá automaticamente em `https://localhost:7296/swagger`).
   * **Pelo Terminal:** Acesse a pasta interna `APIGestaoRH/APIGestaoRH` e execute:
     ```bash
     dotnet run
     ```
     Em seguida, acesse `http://localhost:5135/swagger` ou utilize o arquivo `APIGestaoRH.http`.
3. **Ordem Recomendada de Teste:**
   1. Crie um gestor em `POST /api/Gestores` (ex.: `id: 1`).
   2. Crie funcionários em `POST /api/Funcionarios` informando `"gestorId": 1`.
   3. Consulte `GET /api/Gestores` para visualizar o relacionamento **1:N** preenchido na propriedade `funcionarios`.