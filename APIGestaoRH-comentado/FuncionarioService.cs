using APIGestaoRH.Models; // Importa os modelos de domínio (como Funcionario)

namespace APIGestaoRH.Services; // Define o namespace da camada de serviços

/// <summary>
/// O "cozinheiro": conhece as regras de negócio do RH.
/// Não sabe nada de HTTP nem de Entity Framework.
/// </summary> // Comentário XML explicando o isolamento das regras de negócio
public class FuncionarioService : IFuncionarioService // Implementa a interface IFuncionarioService
{
    private readonly IFuncionarioRepository _repositorio; // Campo privado para o repositório de funcionários
    private readonly IGestorRepository _gestorRepositorio; // Campo privado para consultar dados de gestores

    public FuncionarioService(IFuncionarioRepository repositorio, IGestorRepository gestorRepositorio) // Construtor com injeção de ambos os repositórios
    {
        _repositorio = repositorio; // Armazena a instância de IFuncionarioRepository
        _gestorRepositorio = gestorRepositorio; // Armazena a instância de IGestorRepository
    }

    public async Task<List<Funcionario>> ListarAsync() // Lista todos os funcionários de forma assíncrona
    {
        return await _repositorio.ListarAsync(); // Delega a consulta ao repositório de funcionários
    }

    public async Task<Funcionario?> ObterAsync(int id) // Busca um funcionário pelo id
    {
        return await _repositorio.ObterAsync(id); // Delega a busca por id ao repositório
    }

    public async Task<List<Funcionario>> ListarPorSetorAsync(string setor) // Lista funcionários filtrados por setor
    {
        return await _repositorio.ListarPorSetorAsync(setor); // Chama o filtro por setor no repositório
    }

    public async Task<Funcionario> CriarAsync(Funcionario funcionario) // Valida e cria um novo funcionário
    {
        await ValidarRegrasAsync(funcionario, idIgnorado: null); // Executa as validações sem ignorar nenhum id (novo registro)

        funcionario.Id = 0; // Garante que o Id comece zerado para ser gerado pelo repositório/banco
        _repositorio.Adicionar(funcionario); // Adiciona o funcionário ao repositório
        await _repositorio.SalvarAsync(); // Persiste as alterações

        return funcionario; // Retorna o funcionário criado (já com Id atribuído após salvar)
    }

    public async Task<bool> AtualizarAsync(int id, Funcionario funcionarioAtualizado) // Atualiza um funcionário existente pelo id
    {
        var funcionario = await _repositorio.ObterAsync(id); // Busca o funcionário atual no repositório

        if (funcionario is null) // Verifica se o funcionário existe
        {
            return false; // Retorna false se não encontrar o registro para atualizar
        }

        await ValidarRegrasAsync(funcionarioAtualizado, idIgnorado: id); // Valida as regras ignorando o próprio id na checagem de CPF

        funcionario.Nome = funcionarioAtualizado.Nome; // Atualiza o nome
        funcionario.Cpf = funcionarioAtualizado.Cpf; // Atualiza o CPF
        funcionario.Cargo = funcionarioAtualizado.Cargo; // Atualiza o cargo
        funcionario.Setor = funcionarioAtualizado.Setor; // Atualiza o setor
        funcionario.Salario = funcionarioAtualizado.Salario; // Atualiza o salário[
        funcionario.GestorId = funcionarioAtualizado.GestorId; // Atualiza o vínculo com o gestor (chave estrangeira)

        await _repositorio.SalvarAsync(); // Persiste as alterações no repositório/banco

        return true; // Indica que a atualização foi concluída com sucesso
    }

    public async Task<bool> RemoverAsync(int id) // Remove um funcionário pelo id
    {
        var funcionario = await _repositorio.ObterAsync(id); // Busca o funcionário a ser removido

        if (funcionario is null) // Verifica se o funcionário existe
        {
            return false; // Retorna false se não existir
        }

        _repositorio.Remover(funcionario); // Marca o funcionário para remoção
        await _repositorio.SalvarAsync(); // Efetiva a remoção

        return true; // Indica que a remoção foi realizada com sucesso
    }

    // As regras de negócio do RH moram aqui, num lugar só.
    private async Task ValidarRegrasAsync(Funcionario funcionario, int? idIgnorado) // Método auxiliar privado que centraliza as validações de negócio
    {
        // Regra 1: não pode haver dois funcionários com o mesmo CPF
        if (await _repositorio.ExisteCpfAsync(funcionario.Cpf, idIgnorado)) // Consulta se o CPF já está em uso por outro funcionário
        {
            throw new RegraDeNegocioException("Já existe um funcionário cadastrado com este CPF."); // Lança exceção de regra de negócio se duplicado
        }

        // Regra 2: o salário precisa ser maior que zero
        if (funcionario.Salario <= 0) // Verifica se o salário é zero ou negativo
        {
            throw new RegraDeNegocioException("O salário do funcionário deve ser maior que zero."); // Lança exceção se o salário for inválido
        }

        // Regra 3: o gestor informado na chave estrangeira (1:N) deve existir
        if (!await _gestorRepositorio.ExisteAsync(funcionario.GestorId)) // Verifica se o GestorId informado existe no repositório de gestores
        {
            throw new RegraDeNegocioException("O gestor informado (GestorId) não está cadastrado."); // Lança exceção se o gestor não existir
        }
    } // Fecha ValidarRegrasAsync
} // Fecha a classe FuncionarioService
