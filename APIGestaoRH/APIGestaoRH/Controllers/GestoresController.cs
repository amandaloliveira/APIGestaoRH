// =========================================================================================
// CONTROLLER REST DE GESTORES (Controllers/GestoresController.cs)
// Conceitos abordados neste arquivo:
// [2] Camada Controller (expõe endpoints CRUD para o lado "1" do relacionamento 1:N)
// [3] Injeção de Dependência (recebe IGestorService via construtor)
// [4] Relacionamento 1 para Muitos (1:N) — exibe cada gestor junto com sua lista de funcionários
// [5] Processamento Assíncrono (actions async Task<...>)
// =========================================================================================

// Importa a entidade Gestor
using APIGestaoRH.Models;
// Importa o contrato IGestorService e RegraDeNegocioException
using APIGestaoRH.Services;
// Importa recursos WebAPI do ASP.NET Core
using Microsoft.AspNetCore.Mvc;

// Define o namespace da camada de controllers
namespace APIGestaoRH.Controllers;

/// <summary>
/// [2] O "garçom": gerencia as requisições HTTP referentes aos Gestores (lado 1 do relacionamento 1:N).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GestoresController : ControllerBase
{
    // Dependência abstrata do serviço de gestores
    private readonly IGestorService _servico;

    // [3] Injeção de Dependência: recebe IGestorService do contêiner IoC
    public GestoresController(IGestorService servico)
    {
        _servico = servico;
    }

    // [4] [5] Endpoint GET: api/Gestores -> Lista todos os gestores e seus funcionários subordinados (HTTP 200)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Gestor>>> GetGestores()
    {
        return Ok(await _servico.ListarAsync());
    }

    // [4] [5] Endpoint GET: api/Gestores/{id} -> Obtém um gestor específico e sua equipe (200 ou 404)
    [HttpGet("{id}")]
    public async Task<ActionResult<Gestor>> GetGestor(int id)
    {
        var gestor = await _servico.ObterAsync(id);

        if (gestor is null)
        {
            return NotFound(); // null -> HTTP 404
        }

        return Ok(gestor);
    }

    // [5] Endpoint POST: api/Gestores -> Cadastra um novo gestor (201 Created ou 400 BadRequest)
    [HttpPost]
    public async Task<ActionResult<Gestor>> PostGestor(Gestor gestor)
    {
        try
        {
            var criado = await _servico.CriarAsync(gestor);
            return CreatedAtAction(nameof(GetGestor), new { id = criado.Id }, criado);
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message); // e-mail duplicado -> HTTP 400
        }
    }

    // [5] Endpoint PUT: api/Gestores/{id} -> Atualiza os dados de um gestor (204, 400 ou 404)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGestor(int id, Gestor gestorAtualizado)
    {
        try
        {
            var atualizado = await _servico.AtualizarAsync(id, gestorAtualizado);

            if (!atualizado)
            {
                return NotFound(); // false -> HTTP 404
            }

            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // [4] [5] Endpoint DELETE: api/Gestores/{id} -> Remove um gestor sem funcionários vinculados (204, 400 ou 404)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGestor(int id)
    {
        try
        {
            var removido = await _servico.RemoverAsync(id);

            if (!removido)
            {
                return NotFound(); // false -> HTTP 404
            }

            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            // Se o gestor ainda possuir funcionários (1:N), retorna HTTP 400 com a explicação
            return BadRequest(ex.Message);
        }
    }
}
