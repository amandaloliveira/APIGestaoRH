using APIGestaoRH.Models; // Importa as entidades do modelo (Gestor e Funcionario)
using Microsoft.EntityFrameworkCore; // Importa os recursos do Entity Framework Core

namespace APIGestaoRH.Data; // Define o namespace da camada de acesso a dados

/// <summary>
/// A "porta" da API para o banco de dados.
/// </summary> // Comentário XML explicando o papel do DbContext
public class AppDbContext : DbContext // Define a classe de contexto herdando de DbContext do EF Core
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // Construtor que repassa as opções de configuração ao DbContext base
    {
    } // Corpo vazio do construtor

    // Cada DbSet vira uma tabela no banco de dados
    public DbSet<Gestor> Gestores => Set<Gestor>(); // Propriedade que representa a tabela de Gestores no contexto
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>(); // Propriedade que representa a tabela de Funcionarios no contexto

    protected override void OnModelCreating(ModelBuilder modelBuilder) // Sobrescreve o método para configurar o modelo relacional via Fluent API
    {
        // Configuração explícita do relacionamento 1:N:
        // Um Gestor possui muitos Funcionários, e cada Funcionário tem um único Gestor.
        modelBuilder.Entity<Funcionario>() // Inicia a configuração da entidade Funcionario
            .HasOne(f => f.Gestor) // Indica que cada Funcionario possui um Gestor associado (propriedade de navegação Gestor)
            .WithMany(g => g.Funcionarios) // Indica que o Gestor possui vários Funcionarios (coleção Funcionarios)
            .HasForeignKey(f => f.GestorId) // Define GestorId como a chave estrangeira em Funcionario
            .OnDelete(DeleteBehavior.Restrict); // Impede excluir um Gestor caso existam Funcionarios vinculados a ele
    } // Fecha OnModelCreating
} // Fecha a classe AppDbContext
