// =========================================================================================
// REPOSITÓRIO EF CORE DE GESTORES (Data/GestorRepositoryEf.cs)
// Conceitos abordados neste arquivo:
// [1] ORM com Entity Framework Core (consultas LINQ e Eager Loading com .Include)
// [2] Repository Pattern (implementa IGestorRepository isolando o acesso à tabela Gestores)
// [3] Injeção de Dependência (recebe AppDbContext pelo construtor)
// [4] Relacionamento 1 para Muitos (1:N) — carregamento dos funcionários vinculados (.Include)
// [5] Processamento Assíncrono (ToListAsync, FirstOrDefaultAsync, AnyAsync, SaveChangesAsync)
// =========================================================================================

// Importa a entidade Gestor
using APIGestaoRH.Models;
// Importa o contrato IGestorRepository
using APIGestaoRH.Services;
// Importa extensões assíncronas e relacionais do EF Core (.Include, .ToListAsync, etc.)
using Microsoft.EntityFrameworkCore;

// Define o namespace da camada de dados
namespace APIGestaoRH.Data;

/// <summary>
/// [2] Repositório Entity Framework Core para a entidade Gestor (lado 1 do relacionamento 1:N).
/// </summary>
public class GestorRepositoryEf : IGestorRepository
{
    // Referência privada ao contexto do banco de dados
    private readonly AppDbContext _context;

    // [3] Construtor com Injeção de Dependência do AppDbContext
    public GestorRepositoryEf(AppDbContext context)
    {
        _context = context;
    }

    // [1] [4] [5] Lista todos os gestores de forma assíncrona.
    // O método .Include(g => g.Funcionarios) instrui o EF Core a fazer um LEFT JOIN com a tabela
    // "Funcionarios", trazendo cada gestor já com a lista completa de seus subordinados preenchida.
    public async Task<List<Gestor>> ListarAsync()
    {
        return await _context.Gestores
            .Include(g => g.Funcionarios)
            .ToListAsync();
    }

    // [1] [4] [5] Obtém um gestor específico pelo Id, incluindo sua lista de funcionários vinculados (1:N)
    public async Task<Gestor?> ObterAsync(int id)
    {
        return await _context.Gestores
            .Include(g => g.Funcionarios)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    // [1] [4] [5] Verifica assincronamente se um Gestor existe pelo Id (usado pelo FuncionarioService
    // para validar a chave estrangeira GestorId antes de salvar um funcionário).
    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Gestores.AnyAsync(g => g.Id == id);
    }

    // [1] [5] Verifica assincronamente se já existe outro gestor cadastrado com o mesmo e-mail
    public async Task<bool> ExisteEmailAsync(string email, int? idIgnorado)
    {
        return await _context.Gestores.AnyAsync(g =>
            g.Email.ToLower() == email.ToLower() &&
            (idIgnorado == null || g.Id != idIgnorado));
    }

    // [1] [4] [5] Verifica na tabela Funcionarios se há algum registro apontando para este GestorId.
    // Usado antes de excluir um gestor para preservar a integridade do relacionamento 1:N.
    public async Task<bool> PossuiFuncionariosAsync(int id)
    {
        return await _context.Funcionarios.AnyAsync(f => f.GestorId == id);
    }

    // [1] Prepara a inserção do gestor no ChangeTracker do EF Core
    public void Adicionar(Gestor gestor)
    {
        _context.Gestores.Add(gestor);
    }

    // [1] Prepara a exclusão do gestor no ChangeTracker do EF Core
    public void Remover(Gestor gestor)
    {
        _context.Gestores.Remove(gestor);
    }

    // [1] [5] Executa assincronamente o commit das alterações no banco SQLite
    public async Task SalvarAsync()
    {
        await _context.SaveChangesAsync();
    }
}
