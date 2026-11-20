// =========================================================================================
// CONTRATO DO SERVIÇO DE GESTORES (Services/IGestorService.cs)
// Conceitos abordados neste arquivo:
// [2] Camada de Serviços (contrato de operações de negócio para Gestores)
// [3] Injeção de Dependência (interface injetada no GestoresController)
// [5] Processamento Assíncrono (métodos retornando Task<T>)
// =========================================================================================

// Importa a entidade Gestor
using APIGestaoRH.Models;

// Define o namespace da camada de serviços
namespace APIGestaoRH.Services;

/// <summary>
/// [2] Define os casos de uso de negócio disponíveis para Gestores.
/// </summary>
public interface IGestorService
{
    // [5] Lista todos os gestores e suas equipes
    Task<List<Gestor>> ListarAsync();

    // [5] Busca um gestor específico pelo Id
    Task<Gestor?> ObterAsync(int id);

    // [5] Valida e cria um novo gestor
    Task<Gestor> CriarAsync(Gestor gestor);

    // [5] Valida e atualiza um gestor existente
    Task<bool> AtualizarAsync(int id, Gestor gestorAtualizado);

    // [5] Valida integridade 1:N e remove um gestor
    Task<bool> RemoverAsync(int id);
}
