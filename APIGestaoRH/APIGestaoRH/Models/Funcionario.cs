// =========================================================================================
// MODELO DE DOMÍNIO: FUNCIONÁRIO (Models/Funcionario.cs)
// Conceitos abordados neste arquivo:
// [1] ORM com Entity Framework Core (classe mapeada para a tabela "Funcionarios" no SQLite)
// [4] Relacionamento 1 para Muitos (1:N) — Lado "N" (diversos Funcionários para 1 Gestor)
// =========================================================================================

// Importa atributos de validação automática ([Required], [Range]) avaliados pelo [ApiController]
using System.ComponentModel.DataAnnotations;
// Importa o atributo [JsonIgnore] usado para controlar a serialização JSON
using System.Text.Json.Serialization;

// Define o namespace da camada de modelos da aplicação
namespace APIGestaoRH.Models;

/// <summary>
/// Representa um funcionário cadastrado no sistema de RH.
/// No relacionamento 1:N, esta classe representa o lado "N" (entidade dependente).
/// </summary>
public class Funcionario
{
    // Propriedade Id: Chave Primária (PK) gerada automaticamente pelo banco SQLite
    public int Id { get; set; }

    // Torna o preenchimento do nome obrigatório no JSON enviado à API
    [Required(ErrorMessage = "O nome do funcionário é obrigatório.")]
    // Nome completo do funcionário
    public string Nome { get; set; } = string.Empty;

    // Torna o CPF obrigatório (também possui regra de unicidade no FuncionarioService)
    [Required(ErrorMessage = "O CPF é obrigatório.")]
    // CPF do funcionário
    public string Cpf { get; set; } = string.Empty;

    // Torna o cargo obrigatório
    [Required(ErrorMessage = "O cargo é obrigatório.")]
    // Cargo ocupado pelo funcionário (ex.: "Desenvolvedora Pleno", "Tech Lead")
    public string Cargo { get; set; } = string.Empty;

    // Torna o setor obrigatório (usado também no filtro GET /api/Funcionarios/setor/{setor})
    [Required(ErrorMessage = "O setor é obrigatório.")]
    // Setor em que o funcionário trabalha
    public string Setor { get; set; } = string.Empty;

    // Garante na camada de entrada que o salário esteja dentro de uma faixa positiva válida
    [Range(0.01, 1000000, ErrorMessage = "O salário deve ser maior que zero.")]
    // Valor numérico do salário do funcionário (mapeado como REAL no SQLite)
    public double Salario { get; set; }

    /// <summary>
    /// [4] Chave Estrangeira (Foreign Key) explícita do relacionamento 1:N:
    /// Guarda o Id do Gestor responsável por este funcionário.
    /// Diversos registros de Funcionarios podem apontar para o mesmo GestorId.
    /// </summary>
    [Required(ErrorMessage = "O Id do gestor é obrigatório.")]
    public int GestorId { get; set; }

    /// <summary>
    /// [4] Propriedade de Navegação de Referência (lado "1" visto a partir do lado "N"):
    /// Permite ao Entity Framework navegar de Funcionario -> Gestor.
    /// [JsonIgnore] impede referência circular infinita (Gestor -> Funcionarios -> Gestor...)
    /// durante a serialização JSON e dispensa enviar o objeto Gestor inteiro no POST/PUT.
    /// </summary>
    [JsonIgnore]
    public Gestor? Gestor { get; set; }
}
