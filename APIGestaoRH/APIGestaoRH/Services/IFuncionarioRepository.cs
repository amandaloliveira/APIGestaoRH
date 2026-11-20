using APIGestaoRH.Models;

namespace APIGestaoRH.Services;

/// <summary>
/// O que o serviço precisa saber sobre os funcionários guardados.
/// </summary>
public interface IFuncionarioRepository
{
    Task<List<Funcionario>> ListarAsync();
    Task<Funcionario?> ObterAsync(int id);
    Task<List<Funcionario>> ListarPorSetorAsync(string setor);
    Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado);

    void Adicionar(Funcionario funcionario);
    void Remover(Funcionario funcionario);
    Task SalvarAsync();
}