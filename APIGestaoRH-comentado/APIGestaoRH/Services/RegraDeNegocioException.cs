// =========================================================================================
// EXCEÇÃO DE DOMÍNIO / REGRA DE NEGÓCIO (Services/RegraDeNegocioException.cs)
// Conceitos abordados neste arquivo:
// [2] Camada de Serviços (comunicação limpa entre regras de negócio e Controllers HTTP)
// =========================================================================================

// Define o namespace da camada de serviços
namespace APIGestaoRH.Services;

/// <summary>
/// Exceção personalizada lançada pelos Services quando uma regra de negócio do RH é violada
/// (ex.: CPF duplicado, salário inválido, GestorId inexistente ou remoção de gestor com equipe).
/// Os Controllers capturam esta exceção e a convertem em resposta HTTP 400 (BadRequest).
/// </summary>
public class RegraDeNegocioException : Exception
{
    // Construtor que recebe a mensagem amigável de erro e a repassa para a classe base Exception
    public RegraDeNegocioException(string mensagem) : base(mensagem)
    {
    }
}
