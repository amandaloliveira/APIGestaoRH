using APIGestaoRH.Models; // Importa o modelo Funcionario[cite: 5]
using APIGestaoRH.Services; // Importa a interface IFuncionarioRepository[cite: 5]
using Microsoft.EntityFrameworkCore; // Importa extensões assíncronas e LINQ do EF Core[cite: 5]

namespace APIGestaoRH.Data; // Define o namespace da camada de dados[cite: 5]

/// <summary>
/// Implementação do contrato usando Entity Framework Core.
/// É o único lugar, fora do AppDbContext, que conhece o EF.
/// </summary> // Comentário XML explicando o encapsulamento do EF Core[cite: 5]
public class FuncionarioRepositoryEf : IFuncionarioRepository // Implementa a interface IFuncionarioRepository usando EF Core[cite: 5]
{
    private readonly AppDbContext _context; // Campo privado para o contexto do banco de dados[cite: 5]

    public FuncionarioRepositoryEf(AppDbContext context) // Construtor que recebe AppDbContext via injeção de dependência[cite: 5]
    {
        _context = context; // Armazena a instância do contexto em _context[cite: 5]
    }

    public async Task<List<Funcionario>> ListarAsync() // Retorna de forma assíncrona todos os funcionários[cite: 5]
    {
        return await _context.Funcionarios.ToListAsync(); // Executa consulta SQL para trazer todos os registros da tabela Funcionarios[cite: 5]
    }

    public async Task<Funcionario?> ObterAsync(int id) // Busca um funcionário pelo id (pode retornar null)[cite: 5]
    {
        return await _context.Funcionarios.FindAsync(id); // Busca pelo id usando o rastreamento/chave primária do EF Core[cite: 5]
    }

    public async Task<List<Funcionario>> ListarPorSetorAsync(string setor) // Lista funcionários pertencentes a um setor específico[cite: 5]
    {
        return await _context.Funcionarios // Acessa o DbSet de Funcionarios[cite: 5]
            .Where(f => f.Setor.ToLower() == setor.ToLower()) // Filtra comparando o setor em minúsculas[cite: 5]
            .ToListAsync(); // Converte o resultado da consulta em uma List<Funcionario> de forma assíncrona[cite: 5]
    }

    public async Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado) // Verifica se já existe um CPF cadastrado, opcionalmente ignorando um id[cite: 5]
    {
        return await _context.Funcionarios.AnyAsync(f => // Retorna true se algum registro atender às condições[cite: 5]
            f.Cpf == cpf && // O CPF deve ser igual ao informado[cite: 5]
            (idIgnorado == null || f.Id != idIgnorado)); // Ignora o próprio registro em edições quando idIgnorado for informado[cite: 5]
    }

    public void Adicionar(Funcionario funcionario) // Adiciona um novo funcionário ao contexto[cite: 5]
    {
        _context.Funcionarios.Add(funcionario); // Marca a entidade para inserção no banco ao salvar[cite: 5]
    }

    public void Remover(Funcionario funcionario) // Remove um funcionário existente[cite: 5]
    {
        _context.Funcionarios.Remove(funcionario); // Marca a entidade para exclusão no banco ao salvar[cite: 5]
    }

    public async Task SalvarAsync() // Persiste as alterações pendentes no banco de dados[cite: 5]
    {
        await _context.SaveChangesAsync(); // Executa os comandos INSERT/UPDATE/DELETE pendentes de forma assíncrona[cite: 5]
    }
} // Fecha a classe FuncionarioRepositoryEf[cite: 5]
