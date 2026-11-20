using System.ComponentModel.DataAnnotations; // Importa atributos de validação de dados, como [Required][cite: 24]

namespace APIGestaoRH.Models; // Define o namespace dos modelos de domínio do projeto[cite: 24]

/// <summary>
/// Representa um gestor responsável por um ou mais funcionários (lado 1 do relacionamento 1:N).
/// </summary> // Comentário XML que descreve o papel da entidade Gestor no relacionamento 1:N[cite: 24]
public class Gestor // Declara a classe de modelo Gestor[cite: 24]
{
    public int Id { get; set; } // Identificador único (chave primária) do gestor[cite: 24]

    [Required(ErrorMessage = "O nome do gestor é obrigatório.")] // Exige que Nome seja informado e define a mensagem de erro de validação[cite: 24]
    public string Nome { get; set; } = string.Empty; // Nome do gestor, inicializado com string vazia[cite: 24]

    [Required(ErrorMessage = "O setor do gestor é obrigatório.")] // Exige que Setor seja informado[cite: 24]
    public string Setor { get; set; } = string.Empty; // Setor do gestor, inicializado com string vazia[cite: 24]

    [Required(ErrorMessage = "O e-mail corporativo é obrigatório.")] // Exige que Email seja informado[cite: 24]
    public string Email { get; set; } = string.Empty; // E-mail corporativo do gestor, inicializado com string vazia[cite: 24]

    /// <summary>
    /// Propriedade de navegação: um gestor possui diversos funcionários.
    /// </summary> // Comentário XML que explica a coleção de navegação Funcionarios[cite: 24]
    public List<Funcionario> Funcionarios { get; set; } = new(); // Lista de funcionários vinculados ao gestor, inicializada vazia[cite: 24]
} // Fecha a classe Gestor[cite: 24]
