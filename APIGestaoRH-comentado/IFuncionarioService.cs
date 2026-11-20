using APIGestaoRH.Models; // Importa o modelo Funcionario[cite: 16]

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços[cite: 16]

/// <summary>
/// O "cardápio": o que o serviço de funcionários sabe fazer.
/// </summary> // Comentário XML descrevendo as operações expostas pelo serviço[cite: 16]
public interface IFuncionarioService // Declara a interface do serviço de funcionários[cite: 16]
{
    Task<List<Funcionario>> ListarAsync(); // Contrato para listar funcionários[cite: 16]
    Task<Funcionario?> ObterAsync(int id); // Contrato para buscar funcionário por id[cite: 16]
    Task<List<Funcionario>> ListarPorSetorAsync(string setor); // Contrato para buscar funcionários por setor[cite: 16]
    Task<Funcionario> CriarAsync(Funcionario funcionario); // Contrato para criar um funcionário[cite: 16]
    Task<bool> AtualizarAsync(int id, Funcionario funcionarioAtualizado); // Contrato para atualizar um funcionário existente[cite: 16]
    Task<bool> RemoverAsync(int id); // Contrato para remover um funcionário por id[cite: 16]
} // Fecha a interface IFuncionarioService[cite: 16]
