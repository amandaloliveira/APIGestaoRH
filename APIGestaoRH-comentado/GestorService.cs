using APIGestaoRH.Models; // Importa os modelos de domínio (como Gestor)[cite: 14]

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços[cite: 14]

/// <summary>
/// O "cozinheiro": conhece as regras de negócio dos gestores.
/// </summary> // Comentário XML descrevendo a responsabilidade do serviço de gestores[cite: 14]
public class GestorService : IGestorService // Implementa a interface IGestorService[cite: 14]
{
    private readonly IGestorRepository _repositorio; // Campo privado para o repositório de gestores[cite: 14]

    public GestorService(IGestorRepository repositorio) // Construtor que recebe IGestorRepository por injeção de dependência[cite: 14]
    {
        _repositorio = repositorio; // Atribui a instância recebida ao campo privado[cite: 14]
    }

    public async Task<List<Gestor>> ListarAsync() // Lista todos os gestores de forma assíncrona[cite: 14]
    {
        return await _repositorio.ListarAsync(); // Retorna a lista obtida do repositório[cite: 14]
    }

    public async Task<Gestor?> ObterAsync(int id) // Busca um gestor específico pelo id[cite: 14]
    {
        return await _repositorio.ObterAsync(id); // Retorna o gestor encontrado ou null[cite: 14]
    }

    public async Task<Gestor> CriarAsync(Gestor gestor) // Valida e cria um novo gestor[cite: 14]
    {
        await ValidarRegrasAsync(gestor, idIgnorado: null); // Valida as regras de negócio sem ignorar nenhum id[cite: 14]

        var novoGestor = new Gestor // Cria uma nova instância limpa de Gestor para evitar persistir dados extras indesejados[cite: 14]
        { // Inicia a inicialização de propriedades de novoGestor[cite: 14]
            Id = 0, // Garante Id zerado para geração automática[cite: 14]
            Nome = gestor.Nome, // Copia o nome informado[cite: 14]
            Setor = gestor.Setor, // Copia o setor informado[cite: 14]
            Email = gestor.Email // Copia o e-mail informado[cite: 14]
            // Não copia/persiste eventuais funcionários vindos no payload de criação do gestor[cite: 14]
        }; // Finaliza a criação de novoGestor[cite: 14]

        _repositorio.Adicionar(novoGestor); // Adiciona o novo gestor ao repositório[cite: 14]
        await _repositorio.SalvarAsync(); // Salva as alterações no repositório/banco[cite: 14]

        return novoGestor; // Retorna o gestor criado[cite: 14]
    }

    public async Task<bool> AtualizarAsync(int id, Gestor gestorAtualizado) // Atualiza os dados de um gestor existente[cite: 14]
    {
        var gestor = await _repositorio.ObterAsync(id); // Busca o gestor existente pelo id[cite: 14]

        if (gestor is null) // Verifica se o gestor existe[cite: 14]
        {
            return false; // Retorna false caso não seja encontrado[cite: 14]
        }

        await ValidarRegrasAsync(gestorAtualizado, idIgnorado: id); // Valida as regras ignorando o próprio gestor na verificação de e-mail[cite: 14]

        gestor.Nome = gestorAtualizado.Nome; // Atualiza o nome do gestor[cite: 14]
        gestor.Setor = gestorAtualizado.Setor; // Atualiza o setor do gestor[cite: 14]
        gestor.Email = gestorAtualizado.Email; // Atualiza o e-mail do gestor[cite: 14]

        await _repositorio.SalvarAsync(); // Persiste as alterações[cite: 14]

        return true; // Retorna true indicando sucesso na atualização[cite: 14]
    }

    public async Task<bool> RemoverAsync(int id) // Remove um gestor caso não tenha funcionários vinculados[cite: 14]
    {
        var gestor = await _repositorio.ObterAsync(id); // Busca o gestor pelo id[cite: 14]

        if (gestor is null) // Se o gestor não existir[cite: 14]
        {
            return false; // Retorna false[cite: 14]
        }

        // Regra de integridade 1:N: impede remover gestor com equipe ativa[cite: 14]
        if (await _repositorio.PossuiFuncionariosAsync(id)) // Verifica se há funcionários associados a este gestor[cite: 14]
        {
            throw new RegraDeNegocioException("Não é possível remover um gestor que ainda possui funcionários vinculados."); // Lança exceção de negócio se houver equipe ativa[cite: 14]
        }

        _repositorio.Remover(gestor); // Marca o gestor para remoção[cite: 14]
        await _repositorio.SalvarAsync(); // Efetiva a exclusão[cite: 14]

        return true; // Retorna true indicando que o gestor foi removido[cite: 14]
    }

    private async Task ValidarRegrasAsync(Gestor gestor, int? idIgnorado) // Valida as regras de negócio de Gestor[cite: 14]
    {
        if (await _repositorio.ExisteEmailAsync(gestor.Email, idIgnorado)) // Verifica se já existe outro gestor com o mesmo e-mail[cite: 14]
        {
            throw new RegraDeNegocioException("Já existe um gestor cadastrado com este e-mail."); // Lança exceção se o e-mail já estiver cadastrado[cite: 14]
        }
    } // Fecha ValidarRegrasAsync[cite: 14]
} // Fecha a classe GestorService[cite: 14]
