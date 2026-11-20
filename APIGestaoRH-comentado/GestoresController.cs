using APIGestaoRH.Models; // Importa os modelos de domínio (como Gestor)[cite: 3]
using APIGestaoRH.Services; // Importa as interfaces e serviços de negócio (como IGestorService)[cite: 3]
using Microsoft.AspNetCore.Mvc; // Importa classes e atributos de controladores web do ASP.NET Core[cite: 3]

namespace APIGestaoRH.Controllers; // Define o namespace dos controladores da API[cite: 3]

/// <summary>
/// O "garçom": recebe a requisição, chama o serviço e escolhe a resposta HTTP.
/// </summary> // Resumo XML do papel do controlador[cite: 3]
[Route("api/[controller]")] // Define a rota base como "api/Gestores"[cite: 3]
[ApiController] // Habilita comportamentos automáticos de API REST (validação de dados, etc.)[cite: 3]
public class GestoresController : ControllerBase // Declara o controlador de gestores herdando de ControllerBase[cite: 3]
{
    private readonly IGestorService _servico; // Campo privado para armazenar a referência ao serviço de gestores[cite: 3]

    public GestoresController(IGestorService servico) // Construtor que injeta IGestorService[cite: 3]
    {
        _servico = servico; // Armazena a instância injetada em _servico[cite: 3]
    }

    // GET: api/Gestores[cite: 3]
    [HttpGet] // Mapeia requisições HTTP GET para listar gestores[cite: 3]
    public async Task<ActionResult<IEnumerable<Gestor>>> GetGestores() // Retorna de forma assíncrona a coleção de gestores[cite: 3]
    {
        return Ok(await _servico.ListarAsync()); // Busca todos os gestores no serviço e retorna HTTP 200 (OK)[cite: 3]
    }

    // GET: api/Gestores/1[cite: 3]
    [HttpGet("{id}")] // Mapeia requisições HTTP GET com o parâmetro de rota {id}[cite: 3]
    public async Task<ActionResult<Gestor>> GetGestor(int id) // Busca um gestor específico pelo id[cite: 3]
    {
        var gestor = await _servico.ObterAsync(id); // Consulta o gestor no serviço[cite: 3]

        if (gestor is null) // Verifica se o gestor não foi encontrado[cite: 3]
        {
            return NotFound(); // null -> 404 // Retorna HTTP 404 (Not Found)[cite: 3]
        }

        return Ok(gestor); // Retorna HTTP 200 (OK) com os dados do gestor[cite: 3]
    }

    // POST: api/Gestores[cite: 3]
    [HttpPost] // Mapeia requisições HTTP POST para cadastrar um novo gestor[cite: 3]
    public async Task<ActionResult<Gestor>> PostGestor(Gestor gestor) // Recebe um objeto Gestor no corpo da requisição[cite: 3]
    {
        try // Inicia bloco try para capturar erros de regra de negócio[cite: 3]
        {
            var criado = await _servico.CriarAsync(gestor); // Chama o serviço para criar o gestor[cite: 3]
            return CreatedAtAction(nameof(GetGestor), new { id = criado.Id }, criado); // Retorna HTTP 201 (Created) apontando para GetGestor com o novo id[cite: 3]
        } // Fecha o bloco try[cite: 3]
        catch (RegraDeNegocioException ex) // Captura exceção quando uma regra de negócio é violada[cite: 3]
        {
            return BadRequest(ex.Message); // regra quebrada -> 400 // Retorna HTTP 400 (Bad Request) com a mensagem de erro[cite: 3]
        } // Fecha o bloco catch[cite: 3]
    } // Fecha o método PostGestor[cite: 3]

    // PUT: api/Gestores/1[cite: 3]
    [HttpPut("{id}")] // Mapeia requisições HTTP PUT para atualizar um gestor existente por id[cite: 3]
    public async Task<IActionResult> PutGestor(int id, Gestor gestorAtualizado) // Recebe o id da URL e o objeto Gestor atualizado[cite: 3]
    {
        try // Inicia bloco try[cite: 3]
        {
            var atualizado = await _servico.AtualizarAsync(id, gestorAtualizado); // Chama o serviço para atualizar o gestor[cite: 3]

            if (!atualizado) // Se o gestor com o id informado não existir[cite: 3]
            {
                return NotFound(); // false -> 404 // Retorna HTTP 404 (Not Found)[cite: 3]
            }

            return NoContent(); // Retorna HTTP 204 (No Content) em caso de sucesso[cite: 3]
        } // Fecha o bloco try[cite: 3]
        catch (RegraDeNegocioException ex) // Captura exceção caso alguma regra de negócio falhe[cite: 3]
        {
            return BadRequest(ex.Message); // Retorna HTTP 400 (Bad Request) com a mensagem da exceção[cite: 3]
        } // Fecha o bloco catch[cite: 3]
    } // Fecha o método PutGestor[cite: 3]

    // DELETE: api/Gestores/1[cite: 3]
    [HttpDelete("{id}")] // Mapeia requisições HTTP DELETE para excluir um gestor pelo id[cite: 3]
    public async Task<IActionResult> DeleteGestor(int id) // Método assíncrono de exclusão de gestor[cite: 3]
    {
        try // Inicia bloco try (pois remover gestor pode lançar RegraDeNegocioException)[cite: 3]
        {
            var removido = await _servico.RemoverAsync(id); // Tenta remover o gestor via serviço[cite: 3]

            if (!removido) // Se o gestor não foi encontrado[cite: 3]
            {
                return NotFound(); // false -> 404 // Retorna HTTP 404 (Not Found)[cite: 3]
            }

            return NoContent(); // Retorna HTTP 204 (No Content) quando removido com sucesso[cite: 3]
        } // Fecha o bloco try[cite: 3]
        catch (RegraDeNegocioException ex) // Captura erro caso o gestor tenha funcionários vinculados[cite: 3]
        {
            return BadRequest(ex.Message); // possui funcionários vinculados -> 400 // Retorna HTTP 400 (Bad Request) com a mensagem[cite: 3]
        } // Fecha o bloco catch[cite: 3]
    } // Fecha o método DeleteGestor[cite: 3]
} // Fecha a classe GestoresController[cite: 3]
