// =========================================================================================
// SERVIÇO DE REGRAS DE NEGÓCIO DE GESTORES (Services/GestorService.cs)
// Conceitos abordados neste arquivo:
// [2] Camada de Serviços (regras de unicidade de e-mail e proteção contra exclusão indevida)
// [3] Injeção de Dependência (recebe IGestorRepository via construtor)
// [4] Relacionamento 1 para Muitos (1:N) — impede remover gestor que ainda lidera funcionários
// [5] Processamento Assíncrono (execução não bloqueante com async/await)
// =========================================================================================

// Importa a entidade Gestor
using APIGestaoRH.Models;

// Define o namespace da camada de serviços
namespace APIGestaoRH.Services;

/// <summary>
/// [2] O "cozinheiro": conhece as regras de negócio da entidade Gestor.
/// </summary>
public class GestorService : IGestorService
{
    // Dependência abstrata do repositório de gestores
    private readonly IGestorRepository _repositorio;

    // [3] Injeção de Dependência: recebe IGestorRepository do contêiner do ASP.NET Core
    public GestorService(IGestorRepository repositorio)
    {
        _repositorio = repositorio;
    }

    // [5] Retorna assincronamente todos os gestores com suas listas de funcionários
    public async Task<List<Gestor>> ListarAsync()
    {
        return await _repositorio.ListarAsync();
    }

    // [5] Busca assincronamente um gestor específico pelo Id
    public async Task<Gestor?> ObterAsync(int id)
    {
        return await _repositorio.ObterAsync(id);
    }

    // [2] [5] Valida e cadastra um novo gestor
    public async Task<Gestor> CriarAsync(Gestor gestor)
    {
        // Verifica se o e-mail já está em uso
        await ValidarRegrasAsync(gestor, idIgnorado: null);

        // Garante geração automática de Id pelo banco
        gestor.Id = 0;
        _repositorio.Adicionar(gestor);
        // [5] Salva assincronamente no SQLite
        await _repositorio.SalvarAsync();

        return gestor;
    }

    // [2] [5] Atualiza os dados cadastrais de um gestor existente
    public async Task<bool> AtualizarAsync(int id, Gestor gestorAtualizado)
    {
        var gestor = await _repositorio.ObterAsync(id);

        // Retorna false se o gestor não existir (Controller devolve 404)
        if (gestor is null)
        {
            return false;
        }

        // Valida unicidade de e-mail desconsiderando o próprio gestor editado
        await ValidarRegrasAsync(gestorAtualizado, idIgnorado: id);

        gestor.Nome = gestorAtualizado.Nome;
        gestor.Setor = gestorAtualizado.Setor;
        gestor.Email = gestorAtualizado.Email;

        // [5] Persiste assincronamente as mudanças
        await _repositorio.SalvarAsync();

        return true;
    }

    // [2] [4] [5] Remove um gestor, desde que não existam funcionários subordinados a ele
    public async Task<bool> RemoverAsync(int id)
    {
        var gestor = await _repositorio.ObterAsync(id);

        // Retorna false se o Id não existir (Controller devolve 404)
        if (gestor is null)
        {
            return false;
        }

        // [4] Regra de integridade do relacionamento 1:N:
        // Como o banco usa DeleteBehavior.Restrict, validamos aqui para devolver uma mensagem clara (HTTP 400).
        if (await _repositorio.PossuiFuncionariosAsync(id))
        {
            throw new RegraDeNegocioException("Não é possível remover um gestor que ainda possui funcionários vinculados.");
        }

        _repositorio.Remover(gestor);
        // [5] Efetiva a remoção no SQLite de forma assíncrona
        await _repositorio.SalvarAsync();

        return true;
    }

    // [2] Valida as regras de negócio comuns ao cadastro e à edição de gestores
    private async Task ValidarRegrasAsync(Gestor gestor, int? idIgnorado)
    {
        if (await _repositorio.ExisteEmailAsync(gestor.Email, idIgnorado))
        {
            throw new RegraDeNegocioException("Já existe um gestor cadastrado com este e-mail.");
        }
    }
}
