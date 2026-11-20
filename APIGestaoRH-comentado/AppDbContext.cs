using APIGestaoRH.Models; // Importa as entidades do modelo (Gestor e Funcionario)[cite: 4]
using Microsoft.EntityFrameworkCore; // Importa os recursos do Entity Framework Core[cite: 4]

namespace APIGestaoRH.Data; // Define o namespace da camada de acesso a dados[cite: 4]

/// <summary>
/// A "porta" da API para o banco de dados.
/// </summary> // Comentário XML explicando o papel do DbContext[cite: 4]
public class AppDbContext : DbContext // Define a classe de contexto herdando de DbContext do EF Core[cite: 4]
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // Construtor que repassa as opções de configuração ao DbContext base[cite: 4]
    {
    } // Corpo vazio do construtor[cite: 4]

    // Cada DbSet vira uma tabela no banco de dados[cite: 4]
    public DbSet<Gestor> Gestores => Set<Gestor>(); // Propriedade que representa a tabela de Gestores no contexto[cite: 4]
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>(); // Propriedade que representa a tabela de Funcionarios no contexto[cite: 4]

    protected override void OnModelCreating(ModelBuilder modelBuilder) // Sobrescreve o método para configurar o modelo relacional via Fluent API[cite: 4]
    {
        // Configuração explícita do relacionamento 1:N:[cite: 4]
        // Um Gestor possui muitos Funcionários, e cada Funcionário tem um único Gestor.[cite: 4]
        modelBuilder.Entity<Funcionario>() // Inicia a configuração da entidade Funcionario[cite: 4]
            .HasOne(f => f.Gestor) // Indica que cada Funcionario possui um Gestor associado (propriedade de navegação Gestor)[cite: 4]
            .WithMany(g => g.Funcionarios) // Indica que o Gestor possui vários Funcionarios (coleção Funcionarios)[cite: 4]
            .HasForeignKey(f => f.GestorId) // Define GestorId como a chave estrangeira em Funcionario[cite: 4]
            .OnDelete(DeleteBehavior.Restrict); // Impede excluir um Gestor caso existam Funcionarios vinculados a ele[cite: 4]
    } // Fecha OnModelCreating[cite: 4]
} // Fecha a classe AppDbContext[cite: 4]
