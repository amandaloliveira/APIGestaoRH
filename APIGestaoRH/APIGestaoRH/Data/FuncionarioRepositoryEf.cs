using APIGestaoRH.Models;
using APIGestaoRH.Services;
using Microsoft.EntityFrameworkCore;

namespace APIGestaoRH.Data;

/// <summary>
/// Implementação do contrato usando Entity Framework Core.
/// É o único lugar, fora do AppDbContext, que conhece o EF.
/// </summary>
public class FuncionarioRepositoryEf : IFuncionarioRepository
{
    private readonly AppDbContext _context;

    public FuncionarioRepositoryEf(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Funcionario>> ListarAsync()
    {
        return await _context.Funcionarios.ToListAsync();
    }

    public async Task<Funcionario?> ObterAsync(int id)
    {
        return await _context.Funcionarios.FindAsync(id);
    }

    public async Task<List<Funcionario>> ListarPorSetorAsync(string setor)
    {
        return await _context.Funcionarios
            .Where(f => f.Setor.ToLower() == setor.ToLower())
            .ToListAsync();
    }

    public async Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado)
    {
        return await _context.Funcionarios.AnyAsync(f =>
            f.Cpf == cpf &&
            (idIgnorado == null || f.Id != idIgnorado));
    }

    public void Adicionar(Funcionario funcionario)
    {
        _context.Funcionarios.Add(funcionario);
    }

    public void Remover(Funcionario funcionario)
    {
        _context.Funcionarios.Remove(funcionario);
    }

    public async Task SalvarAsync()
    {
        await _context.SaveChangesAsync();
    }
}