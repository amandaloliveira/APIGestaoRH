using APIGestaoRH.Models;
using APIGestaoRH.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIGestaoRH.Controllers;

/// <summary>
/// O "garçom": recebe a requisição, chama o serviço e escolhe a resposta HTTP.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class FuncionariosController : ControllerBase
{
    private readonly IFuncionarioService _servico;

    public FuncionariosController(IFuncionarioService servico)
    {
        _servico = servico;
    }

    // GET: api/Funcionarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
    {
        return Ok(await _servico.ListarAsync());
    }

    // GET: api/Funcionarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
    {
        var funcionario = await _servico.ObterAsync(id);

        if (funcionario is null)
        {
            return NotFound(); // null -> 404
        }

        return Ok(funcionario);
    }

    // GET: api/Funcionarios/setor/TI
    [HttpGet("setor/{setor}")]
    public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionariosPorSetor(string setor)
    {
        var funcionarios = await _servico.ListarPorSetorAsync(setor);
        return Ok(funcionarios);
    }

    // POST: api/Funcionarios
    [HttpPost]
    public async Task<ActionResult<Funcionario>> PostFuncionario(Funcionario funcionario)
    {
        try
        {
            var criado = await _servico.CriarAsync(funcionario);
            return CreatedAtAction(nameof(GetFuncionario), new { id = criado.Id }, criado);
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message); // regra quebrada -> 400
        }
    }

    // PUT: api/Funcionarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionarioAtualizado)
    {
        try
        {
            var atualizado = await _servico.AtualizarAsync(id, funcionarioAtualizado);

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

    // DELETE: api/Funcionarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFuncionario(int id)
    {
        var removido = await _servico.RemoverAsync(id);

        if (!removido)
        {
            return NotFound();
        }

        return NoContent();
    }
}