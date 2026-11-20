using APIGestaoRH.Models;

namespace APIGestaoRH.Services;

/// <summary>
/// O "cozinheiro": conhece as regras de negócio dos gestores.
/// </summary>
public class GestorService : IGestorService
{
    private readonly IGestorRepository _repositorio;

    public GestorService(IGestorRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<Gestor>> ListarAsync()
    {
        return await _repositorio.ListarAsync();
    }

    public async Task<Gestor?> ObterAsync(int id)
    {
        return await _repositorio.ObterAsync(id);
    }

    public async Task<Gestor> CriarAsync(Gestor gestor)
    {
        await ValidarRegrasAsync(gestor, idIgnorado: null);

        var novoGestor = new Gestor
        {
            Id = 0,
            Nome = gestor.Nome,
            Setor = gestor.Setor,
            Email = gestor.Email
            // Não copia/persiste eventuais funcionários vindos no payload de criação do gestor
        };

        _repositorio.Adicionar(novoGestor);
        await _repositorio.SalvarAsync();

        return novoGestor;
    }

    public async Task<bool> AtualizarAsync(int id, Gestor gestorAtualizado)
    {
        var gestor = await _repositorio.ObterAsync(id);

        if (gestor is null)
        {
            return false;
        }

        await ValidarRegrasAsync(gestorAtualizado, idIgnorado: id);

        gestor.Nome = gestorAtualizado.Nome;
        gestor.Setor = gestorAtualizado.Setor;
        gestor.Email = gestorAtualizado.Email;

        await _repositorio.SalvarAsync();

        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var gestor = await _repositorio.ObterAsync(id);

        if (gestor is null)
        {
            return false;
        }

        // Regra de integridade 1:N: impede remover gestor com equipe ativa
        if (await _repositorio.PossuiFuncionariosAsync(id))
        {
            throw new RegraDeNegocioException("Não é possível remover um gestor que ainda possui funcionários vinculados.");
        }

        _repositorio.Remover(gestor);
        await _repositorio.SalvarAsync();

        return true;
    }

    private async Task ValidarRegrasAsync(Gestor gestor, int? idIgnorado)
    {
        if (await _repositorio.ExisteEmailAsync(gestor.Email, idIgnorado))
        {
            throw new RegraDeNegocioException("Já existe um gestor cadastrado com este e-mail.");
        }
    }
}