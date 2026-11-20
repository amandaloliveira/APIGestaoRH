using APIGestaoRH.Models;

namespace APIGestaoRH.Services;

/// <summary>
/// O "cozinheiro": conhece as regras de negócio do RH.
/// Não sabe nada de HTTP nem de Entity Framework.
/// </summary>
public class FuncionarioService : IFuncionarioService
{
    private readonly IFuncionarioRepository _repositorio;
    private readonly IGestorRepository _gestorRepositorio;

    public FuncionarioService(IFuncionarioRepository repositorio, IGestorRepository gestorRepositorio)
    {
        _repositorio = repositorio;
        _gestorRepositorio = gestorRepositorio;
    }

    public async Task<List<Funcionario>> ListarAsync()
    {
        return await _repositorio.ListarAsync();
    }

    public async Task<Funcionario?> ObterAsync(int id)
    {
        return await _repositorio.ObterAsync(id);
    }

    public async Task<List<Funcionario>> ListarPorSetorAsync(string setor)
    {
        return await _repositorio.ListarPorSetorAsync(setor);
    }

    public async Task<Funcionario> CriarAsync(Funcionario funcionario)
    {
        await ValidarRegrasAsync(funcionario, idIgnorado: null);

        funcionario.Id = 0;
        _repositorio.Adicionar(funcionario);
        await _repositorio.SalvarAsync();

        return funcionario;
    }

    public async Task<bool> AtualizarAsync(int id, Funcionario funcionarioAtualizado)
    {
        var funcionario = await _repositorio.ObterAsync(id);

        if (funcionario is null)
        {
            return false;
        }

        await ValidarRegrasAsync(funcionarioAtualizado, idIgnorado: id);

        funcionario.Nome = funcionarioAtualizado.Nome;
        funcionario.Cpf = funcionarioAtualizado.Cpf;
        funcionario.Cargo = funcionarioAtualizado.Cargo;
        funcionario.Setor = funcionarioAtualizado.Setor;
        funcionario.Salario = funcionarioAtualizado.Salario;
        funcionario.GestorId = funcionarioAtualizado.GestorId;

        await _repositorio.SalvarAsync();

        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var funcionario = await _repositorio.ObterAsync(id);

        if (funcionario is null)
        {
            return false;
        }

        _repositorio.Remover(funcionario);
        await _repositorio.SalvarAsync();

        return true;
    }

    // As regras de negócio do RH moram aqui, num lugar só.
    private async Task ValidarRegrasAsync(Funcionario funcionario, int? idIgnorado)
    {
        // Regra 1: não pode haver dois funcionários com o mesmo CPF
        if (await _repositorio.ExisteCpfAsync(funcionario.Cpf, idIgnorado))
        {
            throw new RegraDeNegocioException("Já existe um funcionário cadastrado com este CPF.");
        }

        // Regra 2: o salário precisa ser maior que zero
        if (funcionario.Salario <= 0)
        {
            throw new RegraDeNegocioException("O salário do funcionário deve ser maior que zero.");
        }

        // Regra 3: o gestor informado na chave estrangeira (1:N) deve existir
        if (!await _gestorRepositorio.ExisteAsync(funcionario.GestorId))
        {
            throw new RegraDeNegocioException("O gestor informado (GestorId) não está cadastrado.");
        }
    }
}