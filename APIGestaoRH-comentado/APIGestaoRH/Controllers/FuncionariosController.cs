// =========================================================================================
// CONTROLLER REST DE FUNCIONÁRIOS (Controllers/FuncionariosController.cs)
// Conceitos abordados neste arquivo:
// [2] Camada Controller (o "garçom": traduz requisições HTTP em chamadas ao Service)
// [3] Injeção de Dependência (recebe IFuncionarioService via construtor)
// [5] Processamento Assíncrono (actions async Task<ActionResult<...>> que liberam threads do Kestrel)
// =========================================================================================

// Importa a entidade Funcionario
using APIGestaoRH.Models;
// Importa o contrato IFuncionarioService e a exceção RegraDeNegocioException
using APIGestaoRH.Services;
// Importa os atributos e classes base do ASP.NET Core MVC/WebAPI
using Microsoft.AspNetCore.Mvc;

// Define o namespace da camada de apresentação HTTP
namespace APIGestaoRH.Controllers;

/// <summary>
/// [2] O "garçom": recebe a requisição HTTP, repassa ao IFuncionarioService e devolve o status HTTP adequado.
/// Não acessa o banco diretamente e não contém regras de negócio.
/// </summary>
// Define a rota base como "api/Funcionarios" ([controller] assume o nome da classe sem "Controller")
[Route("api/[controller]")]
// Habilita comportamentos automáticos de API REST (validação automática de [Required]/[Range], binding de JSON)
[ApiController]
public class FuncionariosController : ControllerBase
{
    // Dependência abstrata para a camada de serviço
    private readonly IFuncionarioService _servico;

    // [3] Injeção de Dependência: o ASP.NET Core entrega uma instância de FuncionarioService a cada requisição
    public FuncionariosController(IFuncionarioService servico)
    {
        _servico = servico;
    }

    // [5] Endpoint GET: api/Funcionarios -> Lista todos os funcionários registrados (HTTP 200 OK)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
    {
        // Aguarda assincronamente a lista sem bloquear a thread do servidor web
        return Ok(await _servico.ListarAsync());
    }

    // [5] Endpoint GET: api/Funcionarios/{id} -> Lista um funcionário específico pelo Id (200 OK ou 404 NotFound)
    [HttpGet("{id}")]
    public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
    {
        // Busca assincronamente o funcionário pelo Id
        var funcionario = await _servico.ObterAsync(id);

        // Se o serviço devolveu null, o Id não existe no banco -> retorna HTTP 404
        if (funcionario is null)
        {
            return NotFound();
        }

        // Retorna HTTP 200 com o JSON do funcionário encontrado
        return Ok(funcionario);
    }

    // [5] Endpoint GET: api/Funcionarios/setor/{setor} -> Lista funcionários de um setor específico (HTTP 200 OK)
    [HttpGet("setor/{setor}")]
    public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionariosPorSetor(string setor)
    {
        // Consulta assincronamente os funcionários filtrados pelo setor informado na URL
        var funcionarios = await _servico.ListarPorSetorAsync(setor);
        return Ok(funcionarios);
    }

    // [5] Endpoint POST: api/Funcionarios -> Insere um novo funcionário no banco (201 Created ou 400 BadRequest)
    [HttpPost]
    public async Task<ActionResult<Funcionario>> PostFuncionario(Funcionario funcionario)
    {
        try
        {
            // Solicita ao serviço a validação e gravação assíncrona do funcionário
            var criado = await _servico.CriarAsync(funcionario);
            // Retorna HTTP 201 Created, preenche o cabeçalho Location com a URL do novo recurso e devolve o objeto
            return CreatedAtAction(nameof(GetFuncionario), new { id = criado.Id }, criado);
        }
        catch (RegraDeNegocioException ex)
        {
            // Se alguma regra de negócio falhou (CPF duplicado, GestorId inexistente, etc.), devolve HTTP 400
            return BadRequest(ex.Message);
        }
    }

    // [5] Endpoint PUT: api/Funcionarios/{id} -> Altera dados (cargo, salário, setor, etc.) (204, 400 ou 404)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionarioAtualizado)
    {
        try
        {
            // Solicita assincronamente a atualização ao serviço
            var atualizado = await _servico.AtualizarAsync(id, funcionarioAtualizado);

            // Se retornou false, o Id não foi encontrado -> HTTP 404 NotFound
            if (!atualizado)
            {
                return NotFound();
            }

            // Atualização concluída com sucesso -> HTTP 204 NoContent (sem corpo de resposta)
            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            // Regra de negócio violada durante a edição -> HTTP 400 BadRequest
            return BadRequest(ex.Message);
        }
    }

    // [5] Endpoint DELETE: api/Funcionarios/{id} -> Remove o funcionário do banco de dados (204 ou 404)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFuncionario(int id)
    {
        // Solicita assincronamente a exclusão ao serviço
        var removido = await _servico.RemoverAsync(id);

        // Se o Id não existe -> HTTP 404 NotFound
        if (!removido)
        {
            return NotFound();
        }

        // Exclusão realizada com sucesso -> HTTP 204 NoContent
        return NoContent();
    }
}
