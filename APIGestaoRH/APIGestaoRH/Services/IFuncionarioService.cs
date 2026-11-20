using APIGestaoRH.Models;

namespace APIGestaoRH.Services;

/// <summary>
/// O "cardápio": o que o serviço de funcionários sabe fazer.
/// </summary>
public interface IFuncionarioService
{
    Task<List<Funcionario>> ListarAsync();
    Task<Funcionario?> ObterAsync(int id);
    Task<List<Funcionario>> ListarPorSetorAsync(string setor);
    Task<Funcionario> CriarAsync(Funcionario funcionario);
    Task<bool> AtualizarAsync(int id, Funcionario funcionarioAtualizado);
    Task<bool> RemoverAsync(int id);
}