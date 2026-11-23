using APIGestaoRH.Models; // Importa o modelo Gestor

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços

/// <summary>
/// O "cardápio": o que o serviço de gestores sabe fazer.
/// </summary> // Comentário XML descrevendo as operações do serviço de gestores
public interface IGestorService // Declara a interface do serviço de gestores
{
    Task<List<Gestor>> ListarAsync(); // Contrato para listar gestores
    Task<Gestor?> ObterAsync(int id); // Contrato para buscar um gestor por id
    Task<Gestor> CriarAsync(Gestor gestor); // Contrato para criar um novo gestor
    Task<bool> AtualizarAsync(int id, Gestor gestorAtualizado); // Contrato para atualizar um gestor existente
    Task<bool> RemoverAsync(int id); // Contrato para remover um gestor por id
} // Fecha a interface IGestorService
