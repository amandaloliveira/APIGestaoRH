using APIGestaoRH.Models;
using Microsoft.EntityFrameworkCore;

namespace APIGestaoRH.Data;

/// <summary>
/// A "porta" da API para o banco de dados.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Cada DbSet vira uma tabela no banco de dados
    public DbSet<Gestor> Gestores => Set<Gestor>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração explícita do relacionamento 1:N:
        // Um Gestor possui muitos Funcionários, e cada Funcionário tem um único Gestor.
        modelBuilder.Entity<Funcionario>()
            .HasOne(f => f.Gestor)
            .WithMany(g => g.Funcionarios)
            .HasForeignKey(f => f.GestorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}