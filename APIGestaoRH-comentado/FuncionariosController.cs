using APIGestaoRH.Models; // Importa os modelos de dados (como Funcionario)
using APIGestaoRH.Services; // Importa as interfaces e classes de serviço (como IFuncionarioService)
using Microsoft.AspNetCore.Mvc; // Importa recursos do ASP.NET Core para controladores e respostas HTTP

namespace APIGestaoRH.Controllers; // Define o namespace onde o controlador está localizado

/// <summary>
/// O "garçom": recebe a requisição, chama o serviço e escolhe a resposta HTTP.
/// </summary> // Comentário XML que descreve o papel do controlador na arquitetura
[Route("api/[controller]")] // Define a rota base como "api/Funcionarios" (usando o nome do controlador sem "Controller")
[ApiController] // Ativa comportamentos automáticos de API (ex.: validação de modelo e binding de parâmetros)
public class FuncionariosController : ControllerBase // Declara o controlador herdando de ControllerBase (sem suporte a Views)
{
    private readonly IFuncionarioService _servico; // Campo privado somente leitura para a injeção do serviço de funcionários

    public FuncionariosController(IFuncionarioService servico) // Construtor que recebe IFuncionarioService via injeção de dependência
    {
        _servico = servico; // Atribui a instância recebida ao campo privado _servico
    }

    // GET: api/Funcionarios
    [HttpGet] // Mapeia requisições HTTP GET para a rota base do controlador
    public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios() // Método assíncrono que retorna a lista de funcionários
    {
        return Ok(await _servico.ListarAsync()); // Chama o serviço para listar os funcionários e devolve HTTP 200 (OK) com os dados
    }

    // GET: api/Funcionarios/5
    [HttpGet("{id}")] // Mapeia requisições HTTP GET recebendo o parâmetro {id} na URL
    public async Task<ActionResult<Funcionario>> GetFuncionario(int id) // Método assíncrono para buscar um funcionário pelo ID
    {
        var funcionario = await _servico.ObterAsync(id); // Consulta o serviço em busca do funcionário correspondente ao id

        if (funcionario is null) // Verifica se nenhum funcionário foi encontrado
        {
            return NotFound(); // null -> 404 // Retorna HTTP 404 (Not Found) caso não exista
        }

        return Ok(funcionario); // Retorna HTTP 200 (OK) com o funcionário encontrado
    }

    // GET: api/Funcionarios/setor/TI
    [HttpGet("setor/{setor}")] // Mapeia requisições HTTP GET para "api/Funcionarios/setor/{setor}"
    public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionariosPorSetor(string setor) // Método para listar funcionários filtrados por setor
    {
        var funcionarios = await _servico.ListarPorSetorAsync(setor); // Solicita ao serviço os funcionários do setor informado
        return Ok(funcionarios); // Retorna HTTP 200 (OK) com a lista filtrada
    }

    // POST: api/Funcionarios
    [HttpPost] // Mapeia requisições HTTP POST para criar um novo funcionário
    public async Task<ActionResult<Funcionario>> PostFuncionario(Funcionario funcionario) // Recebe o objeto Funcionario no corpo da requisição
    {
        try // Inicia bloco de tratamento de exceções
        {
            var criado = await _servico.CriarAsync(funcionario); // Chama o serviço para validar e criar o funcionário
            return CreatedAtAction(nameof(GetFuncionario), new { id = criado.Id }, criado); // Retorna HTTP 201 (Created) com link para GetFuncionario e o objeto criado
        } // Fecha o bloco try
        catch (RegraDeNegocioException ex) // Captura exceções específicas de regra de negócio
        {
            return BadRequest(ex.Message); // regra quebrada -> 400 // Retorna HTTP 400 (Bad Request) com a mensagem da exceção
        } // Fecha o bloco catch
    } // Fecha o método PostFuncionario

    // PUT: api/Funcionarios/5
    [HttpPut("{id}")] // Mapeia requisições HTTP PUT passando o id na rota
    public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionarioAtualizado) // Recebe o id da URL e os dados atualizados no corpo
    {
        try // Inicia bloco de tratamento de exceções
        {
            var atualizado = await _servico.AtualizarAsync(id, funcionarioAtualizado); // Solicita ao serviço a atualização do registro

            if (!atualizado) // Verifica se o registro não foi encontrado para atualizar
            {
                return NotFound(); // false -> 404 // Retorna HTTP 404 (Not Found) se não existir
            }

            return NoContent(); // Retorna HTTP 204 (No Content) indicando atualização bem-sucedida sem corpo de resposta
        } // Fecha o bloco try
        catch (RegraDeNegocioException ex) // Captura violações de regra de negócio durante a atualização
        {
            return BadRequest(ex.Message); // Retorna HTTP 400 (Bad Request) com a descrição do erro
        } // Fecha o bloco catch
    } // Fecha o método PutFuncionario

    // DELETE: api/Funcionarios/5
    [HttpDelete("{id}")] // Mapeia requisições HTTP DELETE para remover um funcionário pelo id
    public async Task<IActionResult> DeleteFuncionario(int id) // Método assíncrono de exclusão por id
    {
        var removido = await _servico.RemoverAsync(id); // Chama o serviço para remover o funcionário

        if (!removido) // Verifica se o funcionário não existia no banco/repositório
        {
            return NotFound(); // Retorna HTTP 404 (Not Found)
        }

        return NoContent(); // Retorna HTTP 204 (No Content) após a remoção bem-sucedida
    } // Fecha o método DeleteFuncionario
} // Fecha a classe FuncionariosController
