using APIGestaoRH.Models; // Importa os modelos de domínio (como Funcionario)[cite: 13]

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços[cite: 13]

/// <summary>
/// O "cozinheiro": conhece as regras de negócio do RH.
/// Não sabe nada de HTTP nem de Entity Framework.
/// </summary> // Comentário XML explicando o isolamento das regras de negócio[cite: 13]
public class FuncionarioService : IFuncionarioService // Implementa a interface IFuncionarioService[cite: 13]
{
    private readonly IFuncionarioRepository _repositorio; // Campo privado para o repositório de funcionários[cite: 13]
    private readonly IGestorRepository _gestorRepositorio; // Campo privado para consultar dados de gestores[cite: 13]

    public FuncionarioService(IFuncionarioRepository repositorio, IGestorRepository gestorRepositorio) // Construtor com injeção de ambos os repositórios[cite: 13]
    {
        _repositorio = repositorio; // Armazena a instância de IFuncionarioRepository[cite: 13]
        _gestorRepositorio = gestorRepositorio; // Armazena a instância de IGestorRepository[cite: 13]
    }

    public async Task<List<Funcionario>> ListarAsync() // Lista todos os funcionários de forma assíncrona[cite: 13]
    {
        return await _repositorio.ListarAsync(); // Delega a consulta ao repositório de funcionários[cite: 13]
    }

    public async Task<Funcionario?> ObterAsync(int id) // Busca um funcionário pelo id[cite: 13]
    {
        return await _repositorio.ObterAsync(id); // Delega a busca por id ao repositório[cite: 13]
    }

    public async Task<List<Funcionario>> ListarPorSetorAsync(string setor) // Lista funcionários filtrados por setor[cite: 13]
    {
        return await _repositorio.ListarPorSetorAsync(setor); // Chama o filtro por setor no repositório[cite: 13]
    }

    public async Task<Funcionario> CriarAsync(Funcionario funcionario) // Valida e cria um novo funcionário[cite: 13]
    {
        await ValidarRegrasAsync(funcionario, idIgnorado: null); // Executa as validações sem ignorar nenhum id (novo registro)[cite: 13]

        funcionario.Id = 0; // Garante que o Id comece zerado para ser gerado pelo repositório/banco[cite: 13]
        _repositorio.Adicionar(funcionario); // Adiciona o funcionário ao repositório[cite: 13]
        await _repositorio.SalvarAsync(); // Persiste as alterações[cite: 13]

        return funcionario; // Retorna o funcionário criado (já com Id atribuído após salvar)[cite: 13]
    }

    public async Task<bool> AtualizarAsync(int id, Funcionario funcionarioAtualizado) // Atualiza um funcionário existente pelo id[cite: 13]
    {
        var funcionario = await _repositorio.ObterAsync(id); // Busca o funcionário atual no repositório[cite: 13]

        if (funcionario is null) // Verifica se o funcionário existe[cite: 13]
        {
            return false; // Retorna false se não encontrar o registro para atualizar[cite: 13]
        }

        await ValidarRegrasAsync(funcionarioAtualizado, idIgnorado: id); // Valida as regras ignorando o próprio id na checagem de CPF[cite: 13]

        funcionario.Nome = funcionarioAtualizado.Nome; // Atualiza o nome[cite: 13]
        funcionario.Cpf = funcionarioAtualizado.Cpf; // Atualiza o CPF[cite: 13]
        funcionario.Cargo = funcionarioAtualizado.Cargo; // Atualiza o cargo[cite: 13]
        funcionario.Setor = funcionarioAtualizado.Setor; // Atualiza o setor[cite: 13]
        funcionario.Salario = funcionarioAtualizado.Salario; // Atualiza o salário[cite: 13]
        funcionario.GestorId = funcionarioAtualizado.GestorId; // Atualiza o vínculo com o gestor (chave estrangeira)[cite: 13]

        await _repositorio.SalvarAsync(); // Persiste as alterações no repositório/banco[cite: 13]

        return true; // Indica que a atualização foi concluída com sucesso[cite: 13]
    }

    public async Task<bool> RemoverAsync(int id) // Remove um funcionário pelo id[cite: 13]
    {
        var funcionario = await _repositorio.ObterAsync(id); // Busca o funcionário a ser removido[cite: 13]

        if (funcionario is null) // Verifica se o funcionário existe[cite: 13]
        {
            return false; // Retorna false se não existir[cite: 13]
        }

        _repositorio.Remover(funcionario); // Marca o funcionário para remoção[cite: 13]
        await _repositorio.SalvarAsync(); // Efetiva a remoção[cite: 13]

        return true; // Indica que a remoção foi realizada com sucesso[cite: 13]
    }

    // As regras de negócio do RH moram aqui, num lugar só.[cite: 13]
    private async Task ValidarRegrasAsync(Funcionario funcionario, int? idIgnorado) // Método auxiliar privado que centraliza as validações de negócio[cite: 13]
    {
        // Regra 1: não pode haver dois funcionários com o mesmo CPF[cite: 13]
        if (await _repositorio.ExisteCpfAsync(funcionario.Cpf, idIgnorado)) // Consulta se o CPF já está em uso por outro funcionário[cite: 13]
        {
            throw new RegraDeNegocioException("Já existe um funcionário cadastrado com este CPF."); // Lança exceção de regra de negócio se duplicado[cite: 13]
        }

        // Regra 2: o salário precisa ser maior que zero[cite: 13]
        if (funcionario.Salario <= 0) // Verifica se o salário é zero ou negativo[cite: 13]
        {
            throw new RegraDeNegocioException("O salário do funcionário deve ser maior que zero."); // Lança exceção se o salário for inválido[cite: 13]
        }

        // Regra 3: o gestor informado na chave estrangeira (1:N) deve existir[cite: 13]
        if (!await _gestorRepositorio.ExisteAsync(funcionario.GestorId)) // Verifica se o GestorId informado existe no repositório de gestores[cite: 13]
        {
            throw new RegraDeNegocioException("O gestor informado (GestorId) não está cadastrado."); // Lança exceção se o gestor não existir[cite: 13]
        }
    } // Fecha ValidarRegrasAsync[cite: 13]
} // Fecha a classe FuncionarioService[cite: 13]
