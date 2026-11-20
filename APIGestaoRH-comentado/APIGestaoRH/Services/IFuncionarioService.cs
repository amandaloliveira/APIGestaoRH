// =========================================================================================
// CONTRATO DO SERVIÇO DE FUNCIONÁRIOS (Services/IFuncionarioService.cs)
// Conceitos abordados neste arquivo:
// [2] Camada de Serviços (define o "cardápio" de casos de uso expostos aos Controllers)
// [3] Injeção de Dependência (interface injetada no FuncionariosController)
// [5] Processamento Assíncrono (operações retornando Task<T>)
// =========================================================================================

// Importa a entidade Funcionario
using APIGestaoRH.Models;

// Define o namespace da camada de serviços
namespace APIGestaoRH.Services;

/// <summary>
/// [2] Interface que define todas as operações de negócio disponíveis para Funcionários.
/// </summary>
public interface IFuncionarioService
{
    // [5] Lista todos os funcionários cadastrados
    Task<List<Funcionario>> ListarAsync();

    // [5] Obtém um funcionário específico pelo Id
    Task<Funcionario?> ObterAsync(int id);

    // [5] Lista apenas os funcionários pertencentes a um determinado setor
    Task<List<Funcionario>> ListarPorSetorAsync(string setor);

    // [5] Valida regras de negócio e cadastra um novo funcionário
    Task<Funcionario> CriarAsync(Funcionario funcionario);

    // [5] Valida regras de negócio e atualiza os dados de um funcionário existente
    Task<bool> AtualizarAsync(int id, Funcionario funcionarioAtualizado);

    // [5] Remove um funcionário pelo Id
    Task<bool> RemoverAsync(int id);
}
