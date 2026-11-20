// =========================================================================================
// MODELO DE DOMÍNIO: GESTOR (Models/Gestor.cs)
// Conceitos abordados neste arquivo:
// [1] ORM com Entity Framework Core (classe mapeada para a tabela "Gestores" no SQLite)
// [4] Relacionamento 1 para Muitos (1:N) — Lado "1" (um Gestor lidera diversos Funcionários)
// =========================================================================================

// Importa atributos de validação automática de dados ([Required], [Range], etc.)
using System.ComponentModel.DataAnnotations;

// Define o namespace da camada de modelos (entidades) da aplicação
namespace APIGestaoRH.Models;

/// <summary>
/// Representa um gestor responsável por um ou mais funcionários.
/// No relacionamento 1:N, esta classe representa o lado "1" (entidade principal).
/// </summary>
public class Gestor
{
    // Propriedade Id: por convenção do Entity Framework Core, torna-se a Chave Primária (PK)
    // com autoincremento (INTEGER PRIMARY KEY AUTOINCREMENT) na tabela "Gestores".
    public int Id { get; set; }

    // Valida automaticamente na entrada da API (HTTP 400 se vazio) e gera coluna NOT NULL no banco
    [Required(ErrorMessage = "O nome do gestor é obrigatório.")]
    // Armazena o nome completo do gestor (inicializado com string.Empty para evitar valor nulo)
    public string Nome { get; set; } = string.Empty;

    // Valida que o setor de atuação do gestor é obrigatório
    [Required(ErrorMessage = "O setor do gestor é obrigatório.")]
    // Armazena o nome do setor gerenciado (ex.: "Tecnologia", "RH")
    public string Setor { get; set; } = string.Empty;

    // Valida que o e-mail corporativo é obrigatório
    [Required(ErrorMessage = "O e-mail corporativo é obrigatório.")]
    // Armazena o e-mail único do gestor (validado no GestorService)
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// [4] Propriedade de Navegação de Coleção (Relacionamento 1:N):
    /// Representa todos os funcionários subordinados a este gestor.
    /// O Entity Framework utiliza esta lista para carregar os funcionários via .Include(g => g.Funcionarios).
    /// </summary>
    public List<Funcionario> Funcionarios { get; set; } = new();
}
