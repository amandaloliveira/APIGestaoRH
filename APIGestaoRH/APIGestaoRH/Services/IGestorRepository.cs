using APIGestaoRH.Models;

namespace APIGestaoRH.Services;

/// <summary>
/// O que o serviço precisa saber sobre os gestores guardados.
/// </summary>
public interface IGestorRepository
{
    Task<List<Gestor>> ListarAsync();
    Task<Gestor?> ObterAsync(int id);
    Task<bool> ExisteAsync(int id);
    Task<bool> ExisteEmailAsync(string email, int? idIgnorado);
    Task<bool> PossuiFuncionariosAsync(int id);

    void Adicionar(Gestor gestor);
    void Remover(Gestor gestor);
    Task SalvarAsync();
}