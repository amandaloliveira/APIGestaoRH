using APIGestaoRH.Models; // Importa o modelo Gestor[cite: 18]

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços[cite: 18]

/// <summary>
/// O "cardápio": o que o serviço de gestores sabe fazer.
/// </summary> // Comentário XML descrevendo as operações do serviço de gestores[cite: 18]
public interface IGestorService // Declara a interface do serviço de gestores[cite: 18]
{
    Task<List<Gestor>> ListarAsync(); // Contrato para listar gestores[cite: 18]
    Task<Gestor?> ObterAsync(int id); // Contrato para buscar um gestor por id[cite: 18]
    Task<Gestor> CriarAsync(Gestor gestor); // Contrato para criar um novo gestor[cite: 18]
    Task<bool> AtualizarAsync(int id, Gestor gestorAtualizado); // Contrato para atualizar um gestor existente[cite: 18]
    Task<bool> RemoverAsync(int id); // Contrato para remover um gestor por id[cite: 18]
} // Fecha a interface IGestorService[cite: 18]
