using APIGestaoRH.Models; // Importa o modelo Funcionario[cite: 6]
using APIGestaoRH.Services; // Importa a interface IFuncionarioRepository[cite: 6]

namespace APIGestaoRH.Data; // Define o namespace da camada de dados[cite: 6]

/// <summary>
/// Guarda os funcionários numa lista em memória.
/// Útil para testes e protótipos sem banco de dados.
/// </summary> // Comentário XML explicando o uso de dados em memória[cite: 6]
public class FuncionarioRepositoryMemoria : IFuncionarioRepository // Implementa IFuncionarioRepository usando uma lista em memória[cite: 6]
{
    private readonly List<Funcionario> _funcionarios = // Lista privada que simula a tabela de funcionários em memória[cite: 6]
    [ // Início da coleção com dados iniciais (collection expression do C#)[cite: 6]
        new Funcionario { Id = 1, Nome = "Ana Souza", Cpf = "111.111.111-11", Cargo = "Desenvolvedora Backend", Setor = "TI", Salario = 6500.00, GestorId = 1 }, // Funcionário inicial #1[cite: 6]
        new Funcionario { Id = 2, Nome = "Bruno Lima", Cpf = "222.222.222-22", Cargo = "Analista de Recrutamento", Setor = "RH", Salario = 4800.00, GestorId = 2 }, // Funcionário inicial #2[cite: 6]
        new Funcionario { Id = 3, Nome = "Carla Mendes", Cpf = "333.333.333-33", Cargo = "Engenheira de Dados", Setor = "TI", Salario = 8200.00, GestorId = 1 } // Funcionário inicial #3[cite: 6]
    ]; // Fim da lista inicial[cite: 6]

    private int _proximoId = 4; // Controla manualmente o próximo ID auto-incrementado para novos registros[cite: 6]

    public Task<List<Funcionario>> ListarAsync() // Retorna a lista de funcionários encapsulada em Task[cite: 6]
    {
        return Task.FromResult(_funcionarios.ToList()); // Retorna uma cópia da lista para evitar mutação externa direta[cite: 6]
    }

    public Task<Funcionario?> ObterAsync(int id) // Busca um funcionário pelo id na lista em memória[cite: 6]
    {
        return Task.FromResult<Funcionario?>(_funcionarios.FirstOrDefault(f => f.Id == id)); // Retorna o primeiro correspondente ou null[cite: 6]
    }

    public Task<List<Funcionario>> ListarPorSetorAsync(string setor) // Filtra funcionários por setor ignorando maiúsculas/minúsculas[cite: 6]
    {
        var lista = _funcionarios // Parte da lista em memória[cite: 6]
            .Where(f => f.Setor.Equals(setor, StringComparison.OrdinalIgnoreCase)) // Compara o nome do setor sem diferenciar caixa[cite: 6]
            .ToList(); // Materializa a consulta em uma lista[cite: 6]

        return Task.FromResult(lista); // Retorna a lista filtrada como Task concluída[cite: 6]
    }

    public Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado) // Verifica duplicidade de CPF na lista em memória[cite: 6]
    {
        var existe = _funcionarios.Any(f => // Verifica se há algum funcionário que cumpra os critérios[cite: 6]
            f.Cpf == cpf && // Mesmo CPF[cite: 6]
            (idIgnorado == null || f.Id != idIgnorado)); // Desconsidera o registro com idIgnorado (útil na edição)[cite: 6]

        return Task.FromResult(existe); // Retorna o booleano dentro de uma Task[cite: 6]
    }

    public void Adicionar(Funcionario funcionario) // Adiciona um novo funcionário à lista em memória[cite: 6]
    {
        funcionario.Id = _proximoId++; // Atribui o próximo ID disponível e incrementa o contador[cite: 6]
        _funcionarios.Add(funcionario); // Insere o funcionário na lista interna[cite: 6]
    }

    public void Remover(Funcionario funcionario) // Remove um funcionário da lista em memória[cite: 6]
    {
        _funcionarios.Remove(funcionario); // Remove a instância da lista _funcionarios[cite: 6]
    }

    public Task SalvarAsync() // Simula o salvamento assíncrono em banco de dados[cite: 6]
    {
        return Task.CompletedTask; // Como os dados já estão na lista em memória, retorna uma Task já concluída[cite: 6]
    }
} // Fecha a classe FuncionarioRepositoryMemoria[cite: 6]
