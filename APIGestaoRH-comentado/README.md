# APIGestaoRH — Arquitetura em Camadas (.NET 10 + EF Core)

## 1. ORM com Entity Framework Core
- **Onde está:** `APIGestaoRH.csproj`, `appsettings.json`, `Program.cs`, `Data/AppDbContext.cs`, `Data/*RepositoryEf.cs` e `Migrations/`.
- **Como funciona:** Mapeia as classes C# (`Gestor` e `Funcionario`) para tabelas no SQLite (`gestaorh.db`) e converte consultas LINQ (`ToListAsync`, `FindAsync`, `Where`, `AnyAsync`, `Include`) em comandos SQL nativos.

## 2. Controller, Services e Repository Pattern
- **Controllers (`Controllers/`):** O "garçom" — recebe requisições HTTP, chama o serviço e devolve códigos de status (`200`, `201`, `204`, `400`, `404`).
- **Services (`Services/`):** O "cozinheiro" — centraliza regras de negócio (CPF único, salário > 0, existência do gestor, bloqueio de exclusão de gestor com equipe).
- **Repositories (`Data/` + interfaces em `Services/`):** O "estoque" — isola o banco por trás de interfaces (`IFuncionarioRepository`, `IGestorRepository`), permitindo alternar entre SQLite (`FuncionarioRepositoryEf`) e memória (`FuncionarioRepositoryMemoria`).

## 3. Injeção de Dependência (DI)
- **Onde está:** `Program.cs` (`AddDbContext`, `AddScoped`, `AddSingleton`) e nos construtores de Controllers, Services e Repositories.
- **Como funciona:** O contêiner nativo do ASP.NET Core instancia e injeta automaticamente as dependências por interface a cada requisição HTTP.

## 4. Relacionamento 1 para Muitos (1:N) entre Models
- **Onde está:** `Models/Gestor.cs` (lado **1**, `List<Funcionario> Funcionarios`), `Models/Funcionario.cs` (lado **N**, `int GestorId` + `[JsonIgnore] Gestor? Gestor`), `Data/AppDbContext.cs` (`HasOne().WithMany().HasForeignKey().OnDelete(DeleteBehavior.Restrict)`), `Data/GestorRepositoryEf.cs` (`.Include(g => g.Funcionarios)`) e `Services/FuncionarioService.cs` / `GestorService.cs`.

## 5. Processamento Assíncrono no ASP.NET Core
- **Onde está:** Em toda a cadeia `Controller -> Service -> Repository` (`async`, `await`, `Task`, `Task<T>`).
- **Como funciona:** Durante operações de I/O com o SQLite (`ToListAsync()`, `FindAsync()`, `SaveChangesAsync()`), a thread do servidor Kestrel é devolvida ao *ThreadPool* para atender outras requisições HTTP simultâneas, retomando a execução apenas quando o banco conclui a operação.
