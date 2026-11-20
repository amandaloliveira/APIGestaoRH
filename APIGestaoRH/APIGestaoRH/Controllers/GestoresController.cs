using APIGestaoRH.Models;
using APIGestaoRH.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIGestaoRH.Controllers;

/// <summary>
/// O "garçom": recebe a requisição, chama o serviço e escolhe a resposta HTTP.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GestoresController : ControllerBase
{
    private readonly IGestorService _servico;

    public GestoresController(IGestorService servico)
    {
        _servico = servico;
    }

    // GET: api/Gestores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Gestor>>> GetGestores()
    {
        return Ok(await _servico.ListarAsync());
    }

    // GET: api/Gestores/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Gestor>> GetGestor(int id)
    {
        var gestor = await _servico.ObterAsync(id);

        if (gestor is null)
        {
            return NotFound(); // null -> 404
        }

        return Ok(gestor);
    }

    // POST: api/Gestores
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
            return BadRequest(ex.Message); // regra quebrada -> 400
        }
    }

    // PUT: api/Gestores/1
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGestor(int id, Gestor gestorAtualizado)
    {
        try
        {
            var atualizado = await _servico.AtualizarAsync(id, gestorAtualizado);

            if (!atualizado)
            {
                return NotFound(); // false -> 404
            }

            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/Gestores/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGestor(int id)
    {
        try
        {
            var removido = await _servico.RemoverAsync(id);

            if (!removido)
            {
                return NotFound(); // false -> 404
            }

            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message); // possui funcionários vinculados -> 400
        }
    }
}