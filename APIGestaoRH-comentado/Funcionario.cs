using System.ComponentModel.DataAnnotations; // Importa atributos de validação como [Required] e [Range]
using System.Text.Json.Serialization; // Importa atributos de serialização JSON como [JsonIgnore]

namespace APIGestaoRH.Models; // Define o namespace dos modelos de domínio

/// <summary>
/// Representa um funcionário cadastrado no sistema de RH (lado N do relacionamento 1:N).
/// </summary> // Comentário XML descrevendo a entidade no relacionamento 1:N
public class Funcionario // Declara a classe de modelo Funcionario
{
    public int Id { get; set; } // Identificador único (chave primária) do funcionário

    [Required(ErrorMessage = "O nome do funcionário é obrigatório.")] // Valida que Nome não seja nulo ou vazio na requisição
    public string Nome { get; set; } = string.Empty; // Nome do funcionário, inicializado com string vazia

    [Required(ErrorMessage = "O CPF é obrigatório.")] // Valida que Cpf seja informado
    public string Cpf { get; set; } = string.Empty; // CPF do funcionário, inicializado com string vazia

    [Required(ErrorMessage = "O cargo é obrigatório.")] // Valida que Cargo seja informado
    public string Cargo { get; set; } = string.Empty; // Cargo ocupado pelo funcionário, inicializado com string vazia

    [Required(ErrorMessage = "O setor é obrigatório.")] // Valida que Setor seja informado
    public string Setor { get; set; } = string.Empty; // Setor do funcionário, inicializado com string vazia

    [Range(0.01, 1000000, ErrorMessage = "O salário deve ser maior que zero.")] // Valida que o salário esteja entre 0,01 e 1.000.000
    public double Salario { get; set; } // Valor do salário do funcionário

    /// <summary>
    /// Chave estrangeira explícita: diversos funcionários apontam para um único gestor.
    /// </summary> // Comentário XML explicando a chave estrangeira GestorId
    [Required(ErrorMessage = "O Id do gestor é obrigatório.")] // Exige que GestorId seja fornecido
    public int GestorId { get; set; } // Identificador do gestor responsável por este funcionário

    /// <summary>
    /// Propriedade de navegação para o gestor responsável.
    /// JsonIgnore evita ciclos de serialização e dispensa enviar o objeto Gestor inteiro no POST/PUT.
    /// </summary> // Comentário XML explicando o uso de [JsonIgnore] na navegação
    [JsonIgnore] // Impede que a propriedade Gestor seja serializada/desserializada no JSON
    public Gestor? Gestor { get; set; } // Referência de navegação para o objeto Gestor associado (pode ser nula)
} // Fecha a classe Funcionario
