// =========================================================================================
// CONTRATO DO REPOSITÓRIO DE GESTORES (Services/IGestorRepository.cs)
// Conceitos abordados neste arquivo:
// [2] Repository Pattern (abstração para persistência da entidade Gestor)
// [3] Injeção de Dependência (interface injetada em GestorService e FuncionarioService)
// [4] Relacionamento 1 para Muitos (1:N) — consultas de existência e vínculo com funcionários
// [5] Processamento Assíncrono (assinaturas Task e Task<T>)
// =========================================================================================

// Importa a entidade Gestor
using APIGestaoRH.Models;

// Define o namespace da camada de serviços
namespace APIGestaoRH.Services;

/// <summary>
/// [2] Contrato de acesso a dados para a entidade Gestor (lado 1 do relacionamento 1:N).
/// </summary>
public interface IGestorRepository
{
    // [5] Lista todos os gestores com seus respectivos funcionários vinculados
    Task<List<Gestor>> ListarAsync();

    // [5] Obtém um gestor pelo Id
    Task<Gestor?> ObterAsync(int id);

    // [4] [5] Verifica se um gestor existe pelo Id (usado para validar GestorId em Funcionarios)
    Task<bool> ExisteAsync(int id);

    // [5] Verifica se já existe gestor cadastrado com o e-mail informado
    Task<bool> ExisteEmailAsync(string email, int? idIgnorado);

    // [4] [5] Verifica se o gestor possui funcionários vinculados antes de permitir sua exclusão
    Task<bool> PossuiFuncionariosAsync(int id);

    // Adiciona um novo gestor
    void Adicionar(Gestor gestor);

    // Remove um gestor existente
    void Remover(Gestor gestor);

    // [5] Persiste as alterações no banco de dados
    Task SalvarAsync();
}
