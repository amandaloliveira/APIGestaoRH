using APIGestaoRH.Models;
using APIGestaoRH.Services;

namespace APIGestaoRH.Data;

/// <summary>
/// Guarda os funcionários numa lista em memória.
/// Útil para testes e protótipos sem banco de dados.
/// </summary>
public class FuncionarioRepositoryMemoria : IFuncionarioRepository
{
    private readonly List<Funcionario> _funcionarios =
    [
        new Funcionario { Id = 1, Nome = "Ana Souza", Cpf = "111.111.111-11", Cargo = "Desenvolvedora Backend", Setor = "TI", Salario = 6500.00, GestorId = 1 },
        new Funcionario { Id = 2, Nome = "Bruno Lima", Cpf = "222.222.222-22", Cargo = "Analista de Recrutamento", Setor = "RH", Salario = 4800.00, GestorId = 2 },
        new Funcionario { Id = 3, Nome = "Carla Mendes", Cpf = "333.333.333-33", Cargo = "Engenheira de Dados", Setor = "TI", Salario = 8200.00, GestorId = 1 }
    ];

    private int _proximoId = 4;

    public Task<List<Funcionario>> ListarAsync()
    {
        return Task.FromResult(_funcionarios.ToList());
    }

    public Task<Funcionario?> ObterAsync(int id)
    {
        return Task.FromResult<Funcionario?>(_funcionarios.FirstOrDefault(f => f.Id == id));
    }

    public Task<List<Funcionario>> ListarPorSetorAsync(string setor)
    {
        var lista = _funcionarios
            .Where(f => f.Setor.Equals(setor, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult(lista);
    }

    public Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado)
    {
        var existe = _funcionarios.Any(f =>
            f.Cpf == cpf &&
            (idIgnorado == null || f.Id != idIgnorado));

        return Task.FromResult(existe);
    }

    public void Adicionar(Funcionario funcionario)
    {
        funcionario.Id = _proximoId++;
        _funcionarios.Add(funcionario);
    }

    public void Remover(Funcionario funcionario)
    {
        _funcionarios.Remove(funcionario);
    }

    public Task SalvarAsync()
    {
        return Task.CompletedTask;
    }
}