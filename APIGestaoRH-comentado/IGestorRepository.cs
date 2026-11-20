using APIGestaoRH.Models; // Importa o modelo Gestor[cite: 17]

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços[cite: 17]

/// <summary>
/// O que o serviço precisa saber sobre os gestores guardados.
/// </summary> // Comentário XML explicando o contrato do repositório de gestores[cite: 17]
public interface IGestorRepository // Declara a interface do repositório de gestores[cite: 17]
{
    Task<List<Gestor>> ListarAsync(); // Contrato para listar todos os gestores[cite: 17]
    Task<Gestor?> ObterAsync(int id); // Contrato para obter um gestor por id (ou null)[cite: 17]
    Task<bool> ExisteAsync(int id); // Contrato para verificar se um gestor existe pelo id[cite: 17]
    Task<bool> ExisteEmailAsync(string email, int? idIgnorado); // Contrato para verificar duplicidade de e-mail, ignorando um id opcional[cite: 17]
    Task<bool> PossuiFuncionariosAsync(int id); // Contrato para verificar se o gestor possui funcionários vinculados[cite: 17]

    void Adicionar(Gestor gestor); // Contrato para adicionar um novo gestor[cite: 17]
    void Remover(Gestor gestor); // Contrato para remover um gestor[cite: 17]
    Task SalvarAsync(); // Contrato para persistir alterações de forma assíncrona[cite: 17]
} // Fecha a interface IGestorRepository[cite: 17]
