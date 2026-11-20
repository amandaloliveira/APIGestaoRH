using APIGestaoRH.Models;
using APIGestaoRH.Services;
using Microsoft.EntityFrameworkCore;

namespace APIGestaoRH.Data;

/// <summary>
/// Repositório EF Core para a entidade Gestor (lado 1 do relacionamento 1:N).
/// </summary>
public class GestorRepositoryEf : IGestorRepository
{
    private readonly AppDbContext _context;

    public GestorRepositoryEf(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Gestor>> ListarAsync()
    {
        return await _context.Gestores
            .Include(g => g.Funcionarios)
            .ToListAsync();
    }

    public async Task<Gestor?> ObterAsync(int id)
    {
        return await _context.Gestores
            .Include(g => g.Funcionarios)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Gestores.AnyAsync(g => g.Id == id);
    }

    public async Task<bool> ExisteEmailAsync(string email, int? idIgnorado)
    {
        return await _context.Gestores.AnyAsync(g =>
            g.Email.ToLower() == email.ToLower() &&
            (idIgnorado == null || g.Id != idIgnorado));
    }

    public async Task<bool> PossuiFuncionariosAsync(int id)
    {
        return await _context.Funcionarios.AnyAsync(f => f.GestorId == id);
    }

    public void Adicionar(Gestor gestor)
    {
        _context.Gestores.Add(gestor);
    }

    public void Remover(Gestor gestor)
    {
        _context.Gestores.Remove(gestor);
    }

    public async Task SalvarAsync()
    {
        await _context.SaveChangesAsync();
    }
}