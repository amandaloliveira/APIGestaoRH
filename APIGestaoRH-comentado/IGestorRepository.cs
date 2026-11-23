using APIGestaoRH.Models; // Importa o modelo Gestor

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços

/// <summary>
/// O que o serviço precisa saber sobre os gestores guardados.
/// </summary> // Comentário XML explicando o contrato do repositório de gestores
public interface IGestorRepository // Declara a interface do repositório de gestores
{
    Task<List<Gestor>> ListarAsync(); // Contrato para listar todos os gestores
    Task<Gestor?> ObterAsync(int id); // Contrato para obter um gestor por id (ou null)
    Task<bool> ExisteAsync(int id); // Contrato para verificar se um gestor existe pelo id
    Task<bool> ExisteEmailAsync(string email, int? idIgnorado); // Contrato para verificar duplicidade de e-mail, ignorando um id opcional
    Task<bool> PossuiFuncionariosAsync(int id); // Contrato para verificar se o gestor possui funcionários vinculados

    void Adicionar(Gestor gestor); // Contrato para adicionar um novo gestor
    void Remover(Gestor gestor); // Contrato para remover um gestor
    Task SalvarAsync(); // Contrato para persistir alterações de forma assíncrona
} // Fecha a interface IGestorRepository
