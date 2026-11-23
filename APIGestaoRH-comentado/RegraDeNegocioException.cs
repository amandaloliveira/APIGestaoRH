namespace APIGestaoRH.Services; // Define o namespace da camada de serviços

/// <summary>
/// Lançada quando uma regra de negócio é quebrada.
/// A mensagem é escrita para quem usa a API.
/// </summary> // Comentário XML explicando o propósito da exceção customizada
public class RegraDeNegocioException : Exception // Define a exceção de regra de negócio herdando de System.Exception
{
    public RegraDeNegocioException(string mensagem) : base(mensagem) // Construtor que recebe a mensagem de erro e a repassa ao construtor base de Exception
    {
    } // Corpo vazio do construtor
} // Fecha a classe RegraDeNegocioException
