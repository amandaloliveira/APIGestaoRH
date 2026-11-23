using APIGestaoRH.Models; // Importa o modelo Funcionario

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços

/// <summary>
/// O que o serviço precisa saber sobre os funcionários guardados.
/// </summary> // Comentário XML explicando o contrato do repositório de funcionários
public interface IFuncionarioRepository // Declara a interface do repositório de funcionários
{
    Task<List<Funcionario>> ListarAsync(); // Contrato para listar todos os funcionários
    Task<Funcionario?> ObterAsync(int id); // Contrato para obter um funcionário por id (ou null)
    Task<List<Funcionario>> ListarPorSetorAsync(string setor); // Contrato para listar funcionários de um setor
    Task<bool> ExisteCpfAsync(string cpf, int? idIgnorado); // Contrato para verificar duplicidade de CPF, ignorando opcionalmente um id

    void Adicionar(Funcionario funcionario); // Contrato para adicionar um funcionário
    void Remover(Funcionario funcionario); // Contrato para remover um funcionário
    Task SalvarAsync(); // Contrato para persistir as alterações de forma assíncrona
} // Fecha a interface IFuncionarioRepository
