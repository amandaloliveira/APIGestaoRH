using APIGestaoRH.Data; // Importa o namespace da camada de acesso a dados (AppDbContext e repositórios)
using APIGestaoRH.Services; // Importa o namespace da camada de serviços e interfaces
using Microsoft.EntityFrameworkCore; // Importa extensões do Entity Framework Core (como UseSqlite)

var builder = WebApplication.CreateBuilder(args); // Cria o construtor da aplicação web com configuração padrão e argumentos da linha de comandos

// Registra os controllers (as classes que recebem as requisições HTTP)
builder.Services.AddControllers(); // Regista os serviços necessários para controladores MVC/API e rotas de atributos

// Registra o AppDbContext e diz que o banco é SQLite,
// usando a connection string do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options => // Regista AppDbContext no contentor de injeção de dependência (tempo de vida Scoped)
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // Configura o provedor SQLite com a connection string "DefaultConnection"

// Repositórios com Entity Framework Core
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepositoryEf>(); // Regista FuncionarioRepositoryEf como implementação de IFuncionarioRepository por requisição
builder.Services.AddScoped<IGestorRepository, GestorRepositoryEf>(); // Regista GestorRepositoryEf como implementação de IGestorRepository por requisição

// Alternativa em memória (para testes sem banco, igual ao Passo 7 do projeto original):
// builder.Services.AddSingleton<IFuncionarioRepository, FuncionarioRepositoryMemoria>(); // Linha comentada: alternativa Singleton em memória sem base de dados

// Serviços com regras de negócio
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>(); // Regista FuncionarioService como implementação de IFuncionarioService por requisição
builder.Services.AddScoped<IGestorService, GestorService>(); // Regista GestorService como implementação de IGestorService por requisição

// Swagger: gera a documentação da API e uma página para testar os endpoints
builder.Services.AddEndpointsApiExplorer(); // Regista o explorador de endpoints para documentação OpenAPI/Swagger
builder.Services.AddSwaggerGen(); // Regista o gerador de especificações e interface Swagger

var app = builder.Build(); // Constrói a instância da aplicação web com os serviços configurados

// Em desenvolvimento, habilita a página do Swagger em /swagger
if (app.Environment.IsDevelopment()) // Verifica se o ambiente atual é "Development"
{ // Abre o bloco condicional de ambiente de desenvolvimento
    app.UseSwagger(); // Ativa o middleware que expõe o documento JSON do Swagger
    app.UseSwaggerUI(); // Ativa a interface visual interativa do Swagger UI
} // Fecha o bloco condicional

app.UseHttpsRedirection(); // Redireciona automaticamente pedidos HTTP para HTTPS quando aplicável

app.MapControllers(); // Mapeia as rotas definidas nos controladores

app.Run(); // Inicia a aplicação e começa a escutar pedidos HTTP/HTTPS
