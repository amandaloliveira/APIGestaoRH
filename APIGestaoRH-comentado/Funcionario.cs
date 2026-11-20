using System.ComponentModel.DataAnnotations; // Importa atributos de validação como [Required] e [Range][cite: 11]
using System.Text.Json.Serialization; // Importa atributos de serialização JSON como [JsonIgnore][cite: 11]

namespace APIGestaoRH.Models; // Define o namespace dos modelos de domínio[cite: 11]

/// <summary>
/// Representa um funcionário cadastrado no sistema de RH (lado N do relacionamento 1:N).
/// </summary> // Comentário XML descrevendo a entidade no relacionamento 1:N[cite: 11]
public class Funcionario // Declara a classe de modelo Funcionario[cite: 11]
{
    public int Id { get; set; } // Identificador único (chave primária) do funcionário[cite: 11]

    [Required(ErrorMessage = "O nome do funcionário é obrigatório.")] // Valida que Nome não seja nulo ou vazio na requisição[cite: 11]
    public string Nome { get; set; } = string.Empty; // Nome do funcionário, inicializado com string vazia[cite: 11]

    [Required(ErrorMessage = "O CPF é obrigatório.")] // Valida que Cpf seja informado[cite: 11]
    public string Cpf { get; set; } = string.Empty; // CPF do funcionário, inicializado com string vazia[cite: 11]

    [Required(ErrorMessage = "O cargo é obrigatório.")] // Valida que Cargo seja informado[cite: 11]
    public string Cargo { get; set; } = string.Empty; // Cargo ocupado pelo funcionário, inicializado com string vazia[cite: 11]

    [Required(ErrorMessage = "O setor é obrigatório.")] // Valida que Setor seja informado[cite: 11]
    public string Setor { get; set; } = string.Empty; // Setor do funcionário, inicializado com string vazia[cite: 11]

    [Range(0.01, 1000000, ErrorMessage = "O salário deve ser maior que zero.")] // Valida que o salário esteja entre 0,01 e 1.000.000[cite: 11]
    public double Salario { get; set; } // Valor do salário do funcionário[cite: 11]

    /// <summary>
    /// Chave estrangeira explícita: diversos funcionários apontam para um único gestor.
    /// </summary> // Comentário XML explicando a chave estrangeira GestorId[cite: 11]
    [Required(ErrorMessage = "O Id do gestor é obrigatório.")] // Exige que GestorId seja fornecido[cite: 11]
    public int GestorId { get; set; } // Identificador do gestor responsável por este funcionário[cite: 11]

    /// <summary>
    /// Propriedade de navegação para o gestor responsável.
    /// JsonIgnore evita ciclos de serialização e dispensa enviar o objeto Gestor inteiro no POST/PUT.
    /// </summary> // Comentário XML explicando o uso de [JsonIgnore] na navegação[cite: 11]
    [JsonIgnore] // Impede que a propriedade Gestor seja serializada/desserializada no JSON[cite: 11]
    public Gestor? Gestor { get; set; } // Referência de navegação para o objeto Gestor associado (pode ser nula)[cite: 11]
} // Fecha a classe Funcionario[cite: 11]
