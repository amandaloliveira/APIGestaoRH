using APIGestaoRH.Models;

namespace APIGestaoRH.Services;

/// <summary>
/// O "cardápio": o que o serviço de gestores sabe fazer.
/// </summary>
public interface IGestorService
{
    Task<List<Gestor>> ListarAsync();
    Task<Gestor?> ObterAsync(int id);
    Task<Gestor> CriarAsync(Gestor gestor);
    Task<bool> AtualizarAsync(int id, Gestor gestorAtualizado);
    Task<bool> RemoverAsync(int id);
}