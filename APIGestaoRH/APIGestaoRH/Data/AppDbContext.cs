// =========================================================================================
// CONTEXTO DO BANCO DE DADOS (Data/AppDbContext.cs)
// Conceitos abordados neste arquivo:
// [1] ORM com Entity Framework Core (herança de DbContext e mapeamento de tabelas via DbSet)
// [3] Injeção de Dependência (recebe DbContextOptions<AppDbContext> configurado no Program.cs)
// [4] Relacionamento 1 para Muitos (1:N) — configuração explícita via Fluent API
// =========================================================================================

// Importa as classes de modelo (Gestor e Funcionario)
using APIGestaoRH.Models;
// Importa as classes principais do Entity Framework Core (DbContext, DbSet, ModelBuilder)
using Microsoft.EntityFrameworkCore;

// Define o namespace da camada de acesso a dados
namespace APIGestaoRH.Data;

/// <summary>
/// A "porta" da API para o banco de dados.
/// Gerencia a sessão com o SQLite, rastreia alterações nos objetos e traduz LINQ para SQL.
/// </summary>
public class AppDbContext : DbContext
{
    // [3] Construtor que recebe via Injeção de Dependência as opções configuradas no Program.cs
    // (como o provedor UseSqlite e a string de conexão) e as repassa para a classe base DbContext.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // [1] Cada propriedade DbSet<T> representa uma tabela no banco de dados SQLite:
    // Mapeia a entidade Gestor para a tabela "Gestores"
    public DbSet<Gestor> Gestores => Set<Gestor>();
    // Mapeia a entidade Funcionario para a tabela "Funcionarios"
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();

    // [1] [4] Método executado pelo EF Core ao construir o modelo relacional do banco.
    // Aqui definimos explicitamente as regras de chave estrangeira e cardinalidade via Fluent API.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // [4] Configuração explícita do relacionamento 1:N (Um para Muitos):
        modelBuilder.Entity<Funcionario>()          // Partindo da entidade Funcionario (lado N)...
            .HasOne(f => f.Gestor)                  // ...cada Funcionário possui UM Gestor (lado 1)
            .WithMany(g => g.Funcionarios)          // ...e cada Gestor possui MUITOS Funcionários (coleção)
            .HasForeignKey(f => f.GestorId)         // ...ligados pela chave estrangeira GestorId na tabela Funcionarios
            .OnDelete(DeleteBehavior.Restrict);     // ...impede apagar em cascata um Gestor que ainda tenha funcionários
    }
}
