using APIGestaoRH.Models; // Importa o modelo Funcionario[cite: 15]

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços[cite: 15]

/// <summary>
/// O que o serviço precisa saber sobre os funcionários guardados.
/// </summary> // Comentário XML explicando o contrato do repositório de funcionários[cite: 15]
public interface IFuncionarioRepository // Declara a interface do repositório de funcionários[cite: 15]
{
    Task<List<Funcionario>> ListarAsync(); // Contrato para listar todos os funcionários[cite: 15]
    Task<Funcionario?> ObterAsync(int id); // Contrato para obter um funcionário por id (ou null)[cite: 15]
    Task<List<Funcionario>> ListarPorSetorAsync(string setor); // Contrato para listar funcionários de um setor[cite: 15]
    Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado); // Contrato para verificar duplicidade de CPF, ignorando opcionalmente um id[cite: 15]

    void Adicionar(Funcionario funcionario); // Contrato para adicionar um funcionário[cite: 15]
    void Remover(Funcionario funcionario); // Contrato para remover um funcionário[cite: 15]
    Task SalvarAsync(); // Contrato para persistir as alterações de forma assíncrona[cite: 15]
} // Fecha a interface IFuncionarioRepository[cite: 15]
