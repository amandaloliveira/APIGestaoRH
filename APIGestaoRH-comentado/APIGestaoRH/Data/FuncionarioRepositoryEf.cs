// =========================================================================================
// REPOSITÓRIO EF CORE DE FUNCIONÁRIOS (Data/FuncionarioRepositoryEf.cs)
// Conceitos abordados neste arquivo:
// [1] ORM com Entity Framework Core (consultas LINQ traduzidas em SQL pelo EF Core)
// [2] Repository Pattern (implementa o contrato IFuncionarioRepository isolando o banco)
// [3] Injeção de Dependência (recebe AppDbContext pelo construtor)
// [5] Processamento Assíncrono (operações de I/O não bloqueantes com async/await e Task)
// =========================================================================================

// Importa a entidade Funcionario
using APIGestaoRH.Models;
// Importa a interface IFuncionarioRepository definida na camada Services
using APIGestaoRH.Services;
// Importa os métodos assíncronos do EF Core (ToListAsync, AnyAsync, etc.)
using Microsoft.EntityFrameworkCore;

// Define o namespace da camada de dados
namespace APIGestaoRH.Data;

/// <summary>
/// [2] Implementação concreta do contrato IFuncionarioRepository usando Entity Framework Core.
/// É o único lugar da aplicação de funcionários (além do AppDbContext) que conhece o EF Core.
/// </summary>
public class FuncionarioRepositoryEf : IFuncionarioRepository
{
    // Campo privado somente leitura que guarda a referência ao contexto do banco
    private readonly AppDbContext _context;

    // [3] Injeção de Dependência: o contêiner do ASP.NET Core entrega automaticamente o AppDbContext
    public FuncionarioRepositoryEf(AppDbContext context)
    {
        _context = context;
    }

    // [1] [5] Busca todos os funcionários no banco de forma assíncrona.
    // ToListAsync() dispara "SELECT * FROM Funcionarios" no SQLite e libera a thread enquanto aguarda o disco.
    public async Task<List<Funcionario>> ListarAsync()
    {
        return await _context.Funcionarios.ToListAsync();
    }

    // [1] [5] Busca um funcionário pela chave primária (Id) de forma assíncrona.
    // FindAsync(id) verifica primeiro o cache rastreado pelo DbContext e, se necessário, executa SELECT por Id.
    public async Task<Funcionario?> ObterAsync(int id)
    {
        return await _context.Funcionarios.FindAsync(id);
    }

    // [1] [5] Filtra funcionários por setor ignorando diferença entre maiúsculas e minúsculas.
    // O EF Core traduz o .Where(...) para uma cláusula SQL WHERE antes de trazer os dados para a memória.
    public async Task<List<Funcionario>> ListarPorSetorAsync(string setor)
    {
        return await _context.Funcionarios
            .Where(f => f.Setor.ToLower() == setor.ToLower())
            .ToListAsync();
    }

    // [1] [5] Verifica assincronamente se já existe algum funcionário com o mesmo CPF.
    // O parâmetro idIgnorado permite que, no PUT, o próprio registro atualizado não acuse duplicidade consigo mesmo.
    public async Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado)
    {
        return await _context.Funcionarios.AnyAsync(f =>
            f.Cpf == cpf &&
            (idIgnorado == null || f.Id != idIgnorado));
    }

    // [1] Marca o objeto funcionario como "Added" no ChangeTracker do EF Core (ainda não executa INSERT no banco)
    public void Adicionar(Funcionario funcionario)
    {
        _context.Funcionarios.Add(funcionario);
    }

    // [1] Marca o objeto funcionario como "Deleted" no ChangeTracker do EF Core (ainda não executa DELETE no banco)
    public void Remover(Funcionario funcionario)
    {
        _context.Funcionarios.Remove(funcionario);
    }

    // [1] [5] Persiste de uma só vez todas as alterações pendentes (INSERT, UPDATE ou DELETE) no SQLite
    // de forma assíncrona, liberando a thread da requisição durante a escrita em disco.
    public async Task SalvarAsync()
    {
        await _context.SaveChangesAsync();
    }
}
