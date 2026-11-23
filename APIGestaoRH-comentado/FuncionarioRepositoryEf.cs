using APIGestaoRH.Models; // Importa o modelo Funcionario
using APIGestaoRH.Services; // Importa a interface IFuncionarioRepository
using Microsoft.EntityFrameworkCore; // Importa extensões assíncronas e LINQ do EF Core

namespace APIGestaoRH.Data; // Define o namespace da camada de dados

/// <summary>
/// Implementação do contrato usando Entity Framework Core.
/// É o único lugar, fora do AppDbContext, que conhece o EF.
/// </summary> // Comentário XML explicando o encapsulamento do EF Core
public class FuncionarioRepositoryEf : IFuncionarioRepository // Implementa a interface IFuncionarioRepository usando EF Core
{
    private readonly AppDbContext _context; // Campo privado para o contexto do banco de dados

    public FuncionarioRepositoryEf(AppDbContext context) // Construtor que recebe AppDbContext via injeção de dependência
    {
        _context = context; // Armazena a instância do contexto em _context
    }

    public async Task<List<Funcionario>> ListarAsync() // Retorna de forma assíncrona todos os funcionários
    {
        return await _context.Funcionarios.ToListAsync(); // Executa consulta SQL para trazer todos os registros da tabela Funcionarios
    }

    public async Task<Funcionario?> ObterAsync(int id) // Busca um funcionário pelo id (pode retornar null)
    {
        return await _context.Funcionarios.FindAsync(id); // Busca pelo id usando o rastreamento/chave primária do EF Core
    }

    public async Task<List<Funcionario>> ListarPorSetorAsync(string setor) // Lista funcionários pertencentes a um setor específico
    {
        return await _context.Funcionarios // Acessa o DbSet de Funcionarios
            .Where(f => f.Setor.ToLower() == setor.ToLower()) // Filtra comparando o setor em minúsculas
            .ToListAsync(); // Converte o resultado da consulta em uma List<Funcionario> de forma assíncrona
    }

    public async Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado) // Verifica se já existe um CPF cadastrado, opcionalmente ignorando um id
    {
        return await _context.Funcionarios.AnyAsync(f => // Retorna true se algum registro atender às condições
            f.Cpf == cpf && // O CPF deve ser igual ao informado
            (idIgnorado == null || f.Id != idIgnorado)); // Ignora o próprio registro em edições quando idIgnorado for informado
    }

    public void Adicionar(Funcionario funcionario) // Adiciona um novo funcionário ao contexto
    {
        _context.Funcionarios.Add(funcionario); // Marca a entidade para inserção no banco ao salvar
    }

    public void Remover(Funcionario funcionario) // Remove um funcionário existente
    {
        _context.Funcionarios.Remove(funcionario); // Marca a entidade para exclusão no banco ao salvar
    }

    public async Task SalvarAsync() // Persiste as alterações pendentes no banco de dados
    {
        await _context.SaveChangesAsync(); // Executa os comandos INSERT/UPDATE/DELETE pendentes de forma assíncrona
    }
} // Fecha a classe FuncionarioRepositoryEf
