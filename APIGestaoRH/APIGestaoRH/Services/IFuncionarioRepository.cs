// =========================================================================================
// CONTRATO DO REPOSITÓRIO DE FUNCIONÁRIOS (Services/IFuncionarioRepository.cs)
// Conceitos abordados neste arquivo:
// [2] Repository Pattern (abstração que desacopla a camada de Serviço do banco de dados)
// [3] Injeção de Dependência (interface registrada no Program.cs e injetada no FuncionarioService)
// [5] Processamento Assíncrono (assinaturas retornando Task e Task<T>)
// =========================================================================================

// Importa a entidade Funcionario
using APIGestaoRH.Models;

// Fica no namespace Services porque quem define o contrato é a camada que precisa dele (Inversão de Dependência)
namespace APIGestaoRH.Services;

/// <summary>
/// [2] Define o que o serviço precisa saber fazer com os dados de funcionários.
/// Repare que não há nenhuma menção a SQL, SQLite ou Entity Framework neste contrato.
/// </summary>
public interface IFuncionarioRepository
{
    // [5] Contrato assíncrono para listar todos os funcionários
    Task<List<Funcionario>> ListarAsync();

    // [5] Contrato assíncrono para buscar um funcionário por Id (pode retornar null se não existir)
    Task<Funcionario?> ObterAsync(int id);

    // [5] Contrato assíncrono para listar funcionários filtrando pelo setor
    Task<List<Funcionario>> ListarPorSetorAsync(string setor);

    // [5] Contrato assíncrono para verificar unicidade de CPF
    Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado);

    // Marca um novo funcionário para inclusão
    void Adicionar(Funcionario funcionario);

    // Marca um funcionário existente para remoção
    void Remover(Funcionario funcionario);

    // [5] Contrato assíncrono para gravar definitivamente as alterações pendentes
    Task SalvarAsync();
}
