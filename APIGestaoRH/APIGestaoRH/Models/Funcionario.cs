using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIGestaoRH.Models;

/// <summary>
/// Representa um funcionário cadastrado no sistema de RH (lado N do relacionamento 1:N).
/// </summary>
public class Funcionario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do funcionário é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "O cargo é obrigatório.")]
    public string Cargo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O setor é obrigatório.")]
    public string Setor { get; set; } = string.Empty;

    [Range(0.01, 1000000, ErrorMessage = "O salário deve ser maior que zero.")]
    public double Salario { get; set; }

    /// <summary>
    /// Chave estrangeira explícita: diversos funcionários apontam para um único gestor.
    /// </summary>
    [Required(ErrorMessage = "O Id do gestor é obrigatório.")]
    public int GestorId { get; set; }

    /// <summary>
    /// Propriedade de navegação para o gestor responsável.
    /// JsonIgnore evita ciclos de serialização e dispensa enviar o objeto Gestor inteiro no POST/PUT.
    /// </summary>
    [JsonIgnore]
    public Gestor? Gestor { get; set; }
}