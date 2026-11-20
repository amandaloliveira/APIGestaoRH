// =========================================================================================
// REPOSITÓRIO EM MEMÓRIA DE FUNCIONÁRIOS (Data/FuncionarioRepositoryMemoria.cs)
// Conceitos abordados neste arquivo:
// [2] Repository Pattern (implementação alternativa de IFuncionarioRepository sem banco)
// [5] Processamento Assíncrono (uso de Task.FromResult e Task.CompletedTask para cumprir contratos async)
// =========================================================================================

// Importa a entidade Funcionario
using APIGestaoRH.Models;
// Importa o contrato IFuncionarioRepository
using APIGestaoRH.Services;

// Define o namespace da camada de dados
namespace APIGestaoRH.Data;

/// <summary>
/// [2] Implementação alternativa do repositório que guarda os funcionários em uma lista na RAM.
/// Prova que a camada de Serviço e os Controllers não dependem do Entity Framework nem do SQLite.
/// </summary>
public class FuncionarioRepositoryMemoria : IFuncionarioRepository
{
    // Lista privada em memória pré-populada com 3 funcionários iniciais para testes rápidos
    private readonly List<Funcionario> _funcionarios =
    [
        new Funcionario { Id = 1, Nome = "Ana Souza", Cpf = "111.111.111-11", Cargo = "Desenvolvedora Backend", Setor = "TI", Salario = 6500.00, GestorId = 1 },
        new Funcionario { Id = 2, Nome = "Bruno Lima", Cpf = "222.222.222-22", Cargo = "Analista de Recrutamento", Setor = "RH", Salario = 4800.00, GestorId = 2 },
        new Funcionario { Id = 3, Nome = "Carla Mendes", Cpf = "333.333.333-33", Cargo = "Engenheira de Dados", Setor = "TI", Salario = 8200.00, GestorId = 1 }
    ];

    // Contador manual para simular o autoincremento que o banco SQLite faria
    private int _proximoId = 4;

    // [5] Como acessar uma List<> na memória é instantâneo (sem I/O de rede/disco),
    // Task.FromResult "embrulha" o resultado já pronto dentro de uma Task<List<Funcionario>>
    // para respeitar a assinatura assíncrona definida na interface IFuncionarioRepository.
    public Task<List<Funcionario>> ListarAsync()
    {
        return Task.FromResult(_funcionarios.ToList());
    }

    // [5] Busca o funcionário na lista em memória e devolve o resultado embrulhado em Task<Funcionario?>
    public Task<Funcionario?> ObterAsync(int id)
    {
        return Task.FromResult<Funcionario?>(_funcionarios.FirstOrDefault(f => f.Id == id));
    }

    // [5] Filtra a lista em memória pelo setor (ignorando maiúsculas/minúsculas) e retorna como Task
    public Task<List<Funcionario>> ListarPorSetorAsync(string setor)
    {
        var lista = _funcionarios
            .Where(f => f.Setor.Equals(setor, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Task.FromResult(lista);
    }

    // [5] Verifica na lista em memória se o CPF já está em uso por outro registro
    public Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado)
    {
        var existe = _funcionarios.Any(f =>
            f.Cpf == cpf &&
            (idIgnorado == null || f.Id != idIgnorado));

        return Task.FromResult(existe);
    }

    // Atribui o próximo Id sequencial e insere o funcionário na lista em memória
    public void Adicionar(Funcionario funcionario)
    {
        funcionario.Id = _proximoId++;
        _funcionarios.Add(funcionario);
    }

    // Remove o funcionário da lista em memória
    public void Remover(Funcionario funcionario)
    {
        _funcionarios.Remove(funcionario);
    }

    // [5] Como não há transação externa a confirmar, retorna Task.CompletedTask (tarefa já concluída)
    public Task SalvarAsync()
    {
        return Task.CompletedTask;
    }
}
