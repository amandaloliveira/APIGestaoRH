using APIGestaoRH.Models; // Importa o modelo Funcionario
using APIGestaoRH.Services; // Importa a interface IFuncionarioRepository

namespace APIGestaoRH.Data; // Define o namespace da camada de dados

/// <summary>
/// Guarda os funcionários numa lista em memória.
/// Útil para testes e protótipos sem banco de dados.
/// </summary> // Comentário XML explicando o uso de dados em memória
public class FuncionarioRepositoryMemoria : IFuncionarioRepository // Implementa IFuncionarioRepository usando uma lista em memória
{
    private readonly List<Funcionario> _funcionarios = // Lista privada que simula a tabela de funcionários em memória
    [ // Início da coleção com dados iniciais (collection expression do C#)
        new Funcionario { Id = 1, Nome = "Ana Souza", Cpf = "111.111.111-11", Cargo = "Desenvolvedora Backend", Setor = "TI", Salario = 6500.00, GestorId = 1 }, // Funcionário inicial #1
        new Funcionario { Id = 2, Nome = "Bruno Lima", Cpf = "222.222.222-22", Cargo = "Analista de Recrutamento", Setor = "RH", Salario = 4800.00, GestorId = 2 }, // Funcionário inicial #2
        new Funcionario { Id = 3, Nome = "Carla Mendes", Cpf = "333.333.333-33", Cargo = "Engenheira de Dados", Setor = "TI", Salario = 8200.00, GestorId = 1 } // Funcionário inicial #3
    ]; // Fim da lista inicial

    private int _proximoId = 4; // Controla manualmente o próximo ID auto-incrementado para novos registros

    public Task<List<Funcionario>> ListarAsync() // Retorna a lista de funcionários encapsulada em Task
    {
        return Task.FromResult(_funcionarios.ToList()); // Retorna uma cópia da lista para evitar mutação externa direta
    }

    public Task<Funcionario?> ObterAsync(int id) // Busca um funcionário pelo id na lista em memória
    {
        return Task.FromResult<Funcionario?>(_funcionarios.FirstOrDefault(f => f.Id == id)); // Retorna o primeiro correspondente ou null
    }

    public Task<List<Funcionario>> ListarPorSetorAsync(string setor) // Filtra funcionários por setor ignorando maiúsculas/minúsculas
    {
        var lista = _funcionarios // Parte da lista em memória
            .Where(f => f.Setor.Equals(setor, StringComparison.OrdinalIgnoreCase)) // Compara o nome do setor sem diferenciar caixa
            .ToList(); // Materializa a consulta em uma lista

        return Task.FromResult(lista); // Retorna a lista filtrada como Task concluída
    }

    public Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado) // Verifica duplicidade de CPF na lista em memória
    {
        var existe = _funcionarios.Any(f => // Verifica se há algum funcionário que cumpra os critérios
            f.Cpf == cpf && // Mesmo CPF
            (idIgnorado == null || f.Id != idIgnorado)); // Desconsidera o registro com idIgnorado (útil na edição)

        return Task.FromResult(existe); // Retorna o booleano dentro de uma Task
    }

    public void Adicionar(Funcionario funcionario) // Adiciona um novo funcionário à lista em memória
    {
        funcionario.Id = _proximoId++; // Atribui o próximo ID disponível e incrementa o contador
        _funcionarios.Add(funcionario); // Insere o funcionário na lista interna
    }

    public void Remover(Funcionario funcionario) // Remove um funcionário da lista em memória
    {
        _funcionarios.Remove(funcionario); // Remove a instância da lista _funcionarios
    }

    public Task SalvarAsync() // Simula o salvamento assíncrono em banco de dados
    {
        return Task.CompletedTask; // Como os dados já estão na lista em memória, retorna uma Task já concluída
    }
} // Fecha a classe FuncionarioRepositoryMemoria
