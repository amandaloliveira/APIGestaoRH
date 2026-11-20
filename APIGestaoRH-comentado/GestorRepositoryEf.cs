using APIGestaoRH.Models; // Importa o modelo Gestor[cite: 7]
using APIGestaoRH.Services; // Importa a interface IGestorRepository[cite: 7]
using Microsoft.EntityFrameworkCore; // Importa métodos assíncronos e Include do EF Core[cite: 7]

namespace APIGestaoRH.Data; // Define o namespace da camada de dados[cite: 7]

/// <summary>
/// Repositório EF Core para a entidade Gestor (lado 1 do relacionamento 1:N).
/// </summary> // Comentário XML explicando o papel do repositório no relacionamento 1:N[cite: 7]
public class GestorRepositoryEf : IGestorRepository // Implementa IGestorRepository usando Entity Framework Core[cite: 7]
{
    private readonly AppDbContext _context; // Campo privado para o DbContext[cite: 7]

    public GestorRepositoryEf(AppDbContext context) // Construtor com injeção de AppDbContext[cite: 7]
    {
        _context = context; // Atribui o contexto ao campo privado[cite: 7]
    }

    public async Task<List<Gestor>> ListarAsync() // Lista todos os gestores incluindo seus funcionários vinculados[cite: 7]
    {
        return await _context.Gestores // Acessa o DbSet de Gestores[cite: 7]
            .Include(g => g.Funcionarios) // Faz Eager Loading da coleção de Funcionarios de cada Gestor[cite: 7]
            .ToListAsync(); // Executa a consulta e retorna a lista de gestores[cite: 7]
    }

    public async Task<Gestor?> ObterAsync(int id) // Busca um gestor específico pelo id incluindo sua equipe[cite: 7]
    {
        return await _context.Gestores // Acessa o DbSet de Gestores[cite: 7]
            .Include(g => g.Funcionarios) // Carrega junto os funcionários vinculados ao gestor[cite: 7]
            .FirstOrDefaultAsync(g => g.Id == id); // Retorna o gestor com o id informado ou null se não existir[cite: 7]
    }

    public async Task<bool> ExisteAsync(int id) // Verifica se existe algum gestor com o id informado[cite: 7]
    {
        return await _context.Gestores.AnyAsync(g => g.Id == id); // Consulta no banco se há algum registro com esse Id[cite: 7]
    }

    public async Task<bool> ExisteEmailAsync(string email, int? idIgnorado) // Verifica se já existe um gestor com o mesmo e-mail[cite: 7]
    {
        return await _context.Gestores.AnyAsync(g => // Retorna true se encontrar correspondência[cite: 7]
            g.Email.ToLower() == email.ToLower() && // Compara o e-mail em minúsculas para evitar duplicidade por caixa[cite: 7]
            (idIgnorado == null || g.Id != idIgnorado)); // Ignora o id especificado (usado ao atualizar um gestor)[cite: 7]
    }

    public async Task<bool> PossuiFuncionariosAsync(int id) // Verifica se um gestor possui funcionários associados[cite: 7]
    {
        return await _context.Funcionarios.AnyAsync(f => f.GestorId == id); // Retorna true se existir ao menos um funcionário com GestorId igual a id[cite: 7]
    }

    public void Adicionar(Gestor gestor) // Adiciona um novo gestor ao contexto[cite: 7]
    {
        _context.Gestores.Add(gestor); // Registra o novo gestor para ser inserido no banco[cite: 7]
    }

    public void Remover(Gestor gestor) // Remove um gestor do contexto[cite: 7]
    {
        _context.Gestores.Remove(gestor); // Marca o gestor para exclusão no banco[cite: 7]
    }

    public async Task SalvarAsync() // Persiste as alterações no banco de dados[cite: 7]
    {
        await _context.SaveChangesAsync(); // Grava de forma assíncrona as mudanças pendentes no contexto[cite: 7]
    }
} // Fecha a classe GestorRepositoryEf[cite: 7]
