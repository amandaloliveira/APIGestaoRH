using System.ComponentModel.DataAnnotations;

namespace APIGestaoRH.Models;

/// <summary>
/// Representa um gestor responsável por um ou mais funcionários (lado 1 do relacionamento 1:N).
/// </summary>
public class Gestor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do gestor é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O setor do gestor é obrigatório.")]
    public string Setor { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail corporativo é obrigatório.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Propriedade de navegação: um gestor possui diversos funcionários.
    /// </summary>
    public List<Funcionario> Funcionarios { get; set; } = new();
}