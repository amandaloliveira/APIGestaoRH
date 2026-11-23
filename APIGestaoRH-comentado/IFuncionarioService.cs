using APIGestaoRH.Models; // Importa o modelo Funcionario

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços

/// <summary>
/// O "cardápio": o que o serviço de funcionários sabe fazer.
/// </summary> // Comentário XML descrevendo as operações expostas pelo serviço
public interface IFuncionarioService // Declara a interface do serviço de funcionários
{
    Task<List<Funcionario>> ListarAsync(); // Contrato para listar funcionários
    Task<Funcionario?> ObterAsync(int id); // Contrato para buscar funcionário por id
    Task<List<Funcionario>> ListarPorSetorAsync(string setor); // Contrato para buscar funcionários por setor
    Task<Funcionario> CriarAsync(Funcionario funcionario); // Contrato para criar um funcionário
    Task<bool> AtualizarAsync(int id, Funcionario funcionarioAtualizado); // Contrato para atualizar um funcionário existente
    Task<bool> RemoverAsync(int id); // Contrato para remover um funcionário por id
} // Fecha a interface IFuncionarioService
