// =========================================================================================
// PONTO DE ENTRADA DA APLICAÇÃO (Program.cs)
// Conceitos abordados neste arquivo:
// [1] ORM com Entity Framework Core (registro do AppDbContext com SQLite)
// [2] Controller, Services e Repository Pattern (registro das camadas da aplicação)
// [3] Injeção de Dependência (DI) — configuração do contêiner nativo IoC do ASP.NET Core
// =========================================================================================

// Importa o namespace onde estão o AppDbContext e os repositórios (camada Data)
using APIGestaoRH.Data;
// Importa o namespace onde estão as interfaces e classes de regra de negócio (camada Services)
using APIGestaoRH.Services;
// Importa as extensões do Entity Framework Core (como o método UseSqlite)
using Microsoft.EntityFrameworkCore;

// Cria o construtor da aplicação Web (carrega appsettings.json, variáveis de ambiente e serviços)
var builder = WebApplication.CreateBuilder(args);

// [2] Registra no contêiner os Controllers (FuncionariosController e GestoresController) que tratam rotas HTTP
builder.Services.AddControllers();

// [1] [3] Registra o AppDbContext no contêiner de Injeção de Dependência (ciclo de vida Scoped por padrão)
// e configura o Entity Framework Core para usar o banco SQLite apontado por "DefaultConnection" no appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// [2] [3] Injeção de Dependência para o Repository Pattern (Funcionários):
// Sempre que alguma classe solicitar IFuncionarioRepository no construtor, o ASP.NET entregará FuncionarioRepositoryEf.
// AddScoped cria uma instância nova por requisição HTTP e a descarta ao final da resposta.
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepositoryEf>();

// [2] [3] Injeção de Dependência para o Repository Pattern (Gestores):
// Associa o contrato IGestorRepository à sua implementação concreta com Entity Framework Core.
builder.Services.AddScoped<IGestorRepository, GestorRepositoryEf>();

// [2] [3] Prova da independência de persistência do Repository Pattern:
// Se comentarmos FuncionarioRepositoryEf acima e ativarmos a linha abaixo, toda a API passa a rodar em memória
// sem alterar nenhuma linha nos Controllers ou Services. Usa AddSingleton para a lista sobreviver entre requisições.
// builder.Services.AddSingleton<IFuncionarioRepository, FuncionarioRepositoryMemoria>();

// [2] [3] Injeção de Dependência para a Camada de Serviço:
// Associa os contratos IFuncionarioService e IGestorService às suas implementações de regra de negócio.
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IGestorService, GestorService>();

// Registra o explorador de endpoints da API (necessário para o Swagger mapear as rotas)
builder.Services.AddEndpointsApiExplorer();
// Registra o gerador da especificação OpenAPI / Swagger UI
builder.Services.AddSwaggerGen();

// Constrói a instância final da aplicação Web com todos os serviços configurados
var app = builder.Build();

// Verifica se a aplicação está rodando em ambiente de desenvolvimento (ASPNETCORE_ENVIRONMENT=Development)
if (app.Environment.IsDevelopment())
{
    // Habilita o middleware que gera o arquivo JSON da documentação Swagger
    app.UseSwagger();
    // Habilita a interface gráfica interativa do Swagger acessível em /swagger
    app.UseSwaggerUI();
}

// Redireciona automaticamente requisições HTTP inseguras para HTTPS
app.UseHttpsRedirection();

// Mapeia todas as rotas definidas com [Route] e [HttpGet/Post/Put/Delete] nos Controllers
app.MapControllers();

// Inicia o servidor web Kestrel e passa a escutar requisições HTTP de forma assíncrona
app.Run();
