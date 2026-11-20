// =========================================================================================
// SERVIÇO DE REGRAS DE NEGÓCIO DE FUNCIONÁRIOS (Services/FuncionarioService.cs)
// Conceitos abordados neste arquivo:
// [2] Camada de Serviços (concentra todas as validações e regras de negócio do RH)
// [3] Injeção de Dependência (recebe IFuncionarioRepository e IGestorRepository via construtor)
// [4] Relacionamento 1 para Muitos (1:N) — valida se o GestorId informado existe no banco
// [5] Processamento Assíncrono (encadeamento de chamadas async/await sem bloquear threads)
// =========================================================================================

// Importa a entidade Funcionario
using APIGestaoRH.Models;

// Define o namespace da camada de serviços
namespace APIGestaoRH.Services;

/// <summary>
/// [2] O "cozinheiro": conhece todas as regras de negócio de Funcionários.
/// Não sabe nada sobre requisições HTTP (Controllers) nem sobre detalhes do banco (EF Core).
/// </summary>
public class FuncionarioService : IFuncionarioService
{
    // Dependência abstrata para persistência de funcionários
    private readonly IFuncionarioRepository _repositorio;
    // [4] Dependência abstrata para consultar gestores e validar a chave estrangeira GestorId (1:N)
    private readonly IGestorRepository _gestorRepositorio;

    // [3] Injeção de Dependência: o ASP.NET Core injeta automaticamente os dois repositórios registrados no Program.cs
    public FuncionarioService(IFuncionarioRepository repositorio, IGestorRepository gestorRepositorio)
    {
        _repositorio = repositorio;
        _gestorRepositorio = gestorRepositorio;
    }

    // [5] Delega de forma assíncrona a listagem completa ao repositório
    public async Task<List<Funcionario>> ListarAsync()
    {
        return await _repositorio.ListarAsync();
    }

    // [5] Delega de forma assíncrona a busca por Id ao repositório (retorna null se não encontrado)
    public async Task<Funcionario?> ObterAsync(int id)
    {
        return await _repositorio.ObterAsync(id);
    }

    // [5] Delega de forma assíncrona o filtro por setor ao repositório
    public async Task<List<Funcionario>> ListarPorSetorAsync(string setor)
    {
        return await _repositorio.ListarPorSetorAsync(setor);
    }

    // [2] [5] Executa o fluxo de criação de um novo funcionário
    public async Task<Funcionario> CriarAsync(Funcionario funcionario)
    {
        // Valida todas as regras de negócio antes de tocar no banco (idIgnorado = null pois é novo cadastro)
        await ValidarRegrasAsync(funcionario, idIgnorado: null);

        // Garante que o Id seja zerado para que o banco gere a chave primária automaticamente
        funcionario.Id = 0;
        // Adiciona o funcionário ao repositório
        _repositorio.Adicionar(funcionario);
        // [5] Persiste assincronamente no banco de dados
        await _repositorio.SalvarAsync();

        // Retorna o funcionário já preenchido com o Id gerado pelo banco
        return funcionario;
    }

    // [2] [5] Executa o fluxo de atualização (cargo, salário, setor, gestor, etc.) de um funcionário
    public async Task<bool> AtualizarAsync(int id, Funcionario funcionarioAtualizado)
    {
        // [5] Busca o registro atual no banco
        var funcionario = await _repositorio.ObterAsync(id);

        // Se o Id não existir, retorna false para o Controller responder 404 NotFound
        if (funcionario is null)
        {
            return false;
        }

        // Valida as regras de negócio ignorando o próprio Id na checagem de CPF duplicado
        await ValidarRegrasAsync(funcionarioAtualizado, idIgnorado: id);

        // Atualiza os campos permitidos na entidade rastreada
        funcionario.Nome = funcionarioAtualizado.Nome;
        funcionario.Cpf = funcionarioAtualizado.Cpf;
        funcionario.Cargo = funcionarioAtualizado.Cargo;
        funcionario.Setor = funcionarioAtualizado.Setor;
        funcionario.Salario = funcionarioAtualizado.Salario;
        funcionario.GestorId = funcionarioAtualizado.GestorId;

        // [5] Grava as alterações no banco de dados
        await _repositorio.SalvarAsync();

        // Retorna true indicando sucesso para o Controller responder 204 NoContent
        return true;
    }

    // [2] [5] Executa o fluxo de exclusão de um funcionário
    public async Task<bool> RemoverAsync(int id)
    {
        // [5] Busca o funcionário pelo Id
        var funcionario = await _repositorio.ObterAsync(id);

        // Se não existir, devolve false (Controller responde 404)
        if (funcionario is null)
        {
            return false;
        }

        // Marca para exclusão e salva assincronamente no banco
        _repositorio.Remover(funcionario);
        await _repositorio.SalvarAsync();

        // Retorna true (Controller responde 204)
        return true;
    }

    // [2] Centraliza todas as regras de domínio do Funcionário em um único método privado
    private async Task ValidarRegrasAsync(Funcionario funcionario, int? idIgnorado)
    {
        // Regra 1: impede cadastrar ou alterar para um CPF que já pertence a outro funcionário
        if (await _repositorio.ExisteCpfAsync(funcionario.Cpf, idIgnorado))
        {
            throw new RegraDeNegocioException("Já existe um funcionário cadastrado com este CPF.");
        }

        // Regra 2: garante consistência financeira (salário deve ser positivo)
        if (funcionario.Salario <= 0)
        {
            throw new RegraDeNegocioException("O salário do funcionário deve ser maior que zero.");
        }

        // [4] Regra 3 (Integridade do Relacionamento 1:N):
        // Verifica no repositório de gestores se o GestorId informado realmente existe antes de vincular.
        if (!await _gestorRepositorio.ExisteAsync(funcionario.GestorId))
        {
            throw new RegraDeNegocioException("O gestor informado (GestorId) não está cadastrado.");
        }
    }
}
