namespace APIGestaoRH.Services; // Define o namespace da camada de serviços[cite: 19]

/// <summary>
/// Lançada quando uma regra de negócio é quebrada.
/// A mensagem é escrita para quem usa a API.
/// </summary> // Comentário XML explicando o propósito da exceção customizada[cite: 19]
public class RegraDeNegocioException : Exception // Define a exceção de regra de negócio herdando de System.Exception[cite: 19]
{
    public RegraDeNegocioException(string mensagem) : base(mensagem) // Construtor que recebe a mensagem de erro e a repassa ao construtor base de Exception[cite: 19]
    {
    } // Corpo vazio do construtor[cite: 19]
} // Fecha a classe RegraDeNegocioException[cite: 19]
