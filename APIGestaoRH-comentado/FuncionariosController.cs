using APIGestaoRH.Models; // Importa os modelos de dados (como Funcionario)[cite: 2]
using APIGestaoRH.Services; // Importa as interfaces e classes de serviço (como IFuncionarioService)[cite: 2]
using Microsoft.AspNetCore.Mvc; // Importa recursos do ASP.NET Core para controladores e respostas HTTP[cite: 2]

namespace APIGestaoRH.Controllers; // Define o namespace onde o controlador está localizado[cite: 2]

/// <summary>
/// O "garçom": recebe a requisição, chama o serviço e escolhe a resposta HTTP.
/// </summary> // Comentário XML que descreve o papel do controlador na arquitetura[cite: 2]
[Route("api/[controller]")] // Define a rota base como "api/Funcionarios" (usando o nome do controlador sem "Controller")[cite: 2]
[ApiController] // Ativa comportamentos automáticos de API (ex.: validação de modelo e binding de parâmetros)[cite: 2]
public class FuncionariosController : ControllerBase // Declara o controlador herdando de ControllerBase (sem suporte a Views)[cite: 2]
{
    private readonly IFuncionarioService _servico; // Campo privado somente leitura para a injeção do serviço de funcionários[cite: 2]

    public FuncionariosController(IFuncionarioService servico) // Construtor que recebe IFuncionarioService via injeção de dependência[cite: 2]
    {
        _servico = servico; // Atribui a instância recebida ao campo privado _servico[cite: 2]
    }

    // GET: api/Funcionarios[cite: 2]
    [HttpGet] // Mapeia requisições HTTP GET para a rota base do controlador[cite: 2]
    public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios() // Método assíncrono que retorna a lista de funcionários[cite: 2]
    {
        return Ok(await _servico.ListarAsync()); // Chama o serviço para listar os funcionários e devolve HTTP 200 (OK) com os dados[cite: 2]
    }

    // GET: api/Funcionarios/5[cite: 2]
    [HttpGet("{id}")] // Mapeia requisições HTTP GET recebendo o parâmetro {id} na URL[cite: 2]
    public async Task<ActionResult<Funcionario>> GetFuncionario(int id) // Método assíncrono para buscar um funcionário pelo ID[cite: 2]
    {
        var funcionario = await _servico.ObterAsync(id); // Consulta o serviço em busca do funcionário correspondente ao id[cite: 2]

        if (funcionario is null) // Verifica se nenhum funcionário foi encontrado[cite: 2]
        {
            return NotFound(); // null -> 404 // Retorna HTTP 404 (Not Found) caso não exista[cite: 2]
        }

        return Ok(funcionario); // Retorna HTTP 200 (OK) com o funcionário encontrado[cite: 2]
    }

    // GET: api/Funcionarios/setor/TI[cite: 2]
    [HttpGet("setor/{setor}")] // Mapeia requisições HTTP GET para "api/Funcionarios/setor/{setor}"[cite: 2]
    public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionariosPorSetor(string setor) // Método para listar funcionários filtrados por setor[cite: 2]
    {
        var funcionarios = await _servico.ListarPorSetorAsync(setor); // Solicita ao serviço os funcionários do setor informado[cite: 2]
        return Ok(funcionarios); // Retorna HTTP 200 (OK) com a lista filtrada[cite: 2]
    }

    // POST: api/Funcionarios[cite: 2]
    [HttpPost] // Mapeia requisições HTTP POST para criar um novo funcionário[cite: 2]
    public async Task<ActionResult<Funcionario>> PostFuncionario(Funcionario funcionario) // Recebe o objeto Funcionario no corpo da requisição[cite: 2]
    {
        try // Inicia bloco de tratamento de exceções[cite: 2]
        {
            var criado = await _servico.CriarAsync(funcionario); // Chama o serviço para validar e criar o funcionário[cite: 2]
            return CreatedAtAction(nameof(GetFuncionario), new { id = criado.Id }, criado); // Retorna HTTP 201 (Created) com link para GetFuncionario e o objeto criado[cite: 2]
        } // Fecha o bloco try[cite: 2]
        catch (RegraDeNegocioException ex) // Captura exceções específicas de regra de negócio[cite: 2]
        {
            return BadRequest(ex.Message); // regra quebrada -> 400 // Retorna HTTP 400 (Bad Request) com a mensagem da exceção[cite: 2]
        } // Fecha o bloco catch[cite: 2]
    } // Fecha o método PostFuncionario[cite: 2]

    // PUT: api/Funcionarios/5[cite: 2]
    [HttpPut("{id}")] // Mapeia requisições HTTP PUT passando o id na rota[cite: 2]
    public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionarioAtualizado) // Recebe o id da URL e os dados atualizados no corpo[cite: 2]
    {
        try // Inicia bloco de tratamento de exceções[cite: 2]
        {
            var atualizado = await _servico.AtualizarAsync(id, funcionarioAtualizado); // Solicita ao serviço a atualização do registro[cite: 2]

            if (!atualizado) // Verifica se o registro não foi encontrado para atualizar[cite: 2]
            {
                return NotFound(); // false -> 404 // Retorna HTTP 404 (Not Found) se não existir[cite: 2]
            }

            return NoContent(); // Retorna HTTP 204 (No Content) indicando atualização bem-sucedida sem corpo de resposta[cite: 2]
        } // Fecha o bloco try[cite: 2]
        catch (RegraDeNegocioException ex) // Captura violações de regra de negócio durante a atualização[cite: 2]
        {
            return BadRequest(ex.Message); // Retorna HTTP 400 (Bad Request) com a descrição do erro[cite: 2]
        } // Fecha o bloco catch[cite: 2]
    } // Fecha o método PutFuncionario[cite: 2]

    // DELETE: api/Funcionarios/5[cite: 2]
    [HttpDelete("{id}")] // Mapeia requisições HTTP DELETE para remover um funcionário pelo id[cite: 2]
    public async Task<IActionResult> DeleteFuncionario(int id) // Método assíncrono de exclusão por id[cite: 2]
    {
        var removido = await _servico.RemoverAsync(id); // Chama o serviço para remover o funcionário[cite: 2]

        if (!removido) // Verifica se o funcionário não existia no banco/repositório[cite: 2]
        {
            return NotFound(); // Retorna HTTP 404 (Not Found)[cite: 2]
        }

        return NoContent(); // Retorna HTTP 204 (No Content) após a remoção bem-sucedida[cite: 2]
    } // Fecha o método DeleteFuncionario[cite: 2]
} // Fecha a classe FuncionariosController[cite: 2]
