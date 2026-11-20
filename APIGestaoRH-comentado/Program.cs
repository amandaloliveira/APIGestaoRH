using APIGestaoRH.Data; // Importa o namespace da camada de acesso a dados (AppDbContext e repositórios)[cite: 23]
using APIGestaoRH.Services; // Importa o namespace da camada de serviços e interfaces[cite: 23]
using Microsoft.EntityFrameworkCore; // Importa extensões do Entity Framework Core (como UseSqlite)[cite: 23]

var builder = WebApplication.CreateBuilder(args); // Cria o construtor da aplicação web com configuração padrão e argumentos da linha de comandos[cite: 23]

// Registra os controllers (as classes que recebem as requisições HTTP)[cite: 23]
builder.Services.AddControllers(); // Regista os serviços necessários para controladores MVC/API e rotas de atributos[cite: 23]

// Registra o AppDbContext e diz que o banco é SQLite,[cite: 23]
// usando a connection string do appsettings.json[cite: 23]
builder.Services.AddDbContext<AppDbContext>(options => // Regista AppDbContext no contentor de injeção de dependência (tempo de vida Scoped)[cite: 23]
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // Configura o provedor SQLite com a connection string "DefaultConnection"[cite: 23]

// Repositórios com Entity Framework Core[cite: 23]
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepositoryEf>(); // Regista FuncionarioRepositoryEf como implementação de IFuncionarioRepository por requisição[cite: 23]
builder.Services.AddScoped<IGestorRepository, GestorRepositoryEf>(); // Regista GestorRepositoryEf como implementação de IGestorRepository por requisição[cite: 23]

// Alternativa em memória (para testes sem banco, igual ao Passo 7 do projeto original):[cite: 23]
// builder.Services.AddSingleton<IFuncionarioRepository, FuncionarioRepositoryMemoria>(); // Linha comentada: alternativa Singleton em memória sem base de dados[cite: 23]

// Serviços com regras de negócio[cite: 23]
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>(); // Regista FuncionarioService como implementação de IFuncionarioService por requisição[cite: 23]
builder.Services.AddScoped<IGestorService, GestorService>(); // Regista GestorService como implementação de IGestorService por requisição[cite: 23]

// Swagger: gera a documentação da API e uma página para testar os endpoints[cite: 23]
builder.Services.AddEndpointsApiExplorer(); // Regista o explorador de endpoints para documentação OpenAPI/Swagger[cite: 23]
builder.Services.AddSwaggerGen(); // Regista o gerador de especificações e interface Swagger[cite: 23]

var app = builder.Build(); // Constrói a instância da aplicação web com os serviços configurados[cite: 23]

// Em desenvolvimento, habilita a página do Swagger em /swagger[cite: 23]
if (app.Environment.IsDevelopment()) // Verifica se o ambiente atual é "Development"[cite: 23]
{ // Abre o bloco condicional de ambiente de desenvolvimento[cite: 23]
    app.UseSwagger(); // Ativa o middleware que expõe o documento JSON do Swagger[cite: 23]
    app.UseSwaggerUI(); // Ativa a interface visual interativa do Swagger UI[cite: 23]
} // Fecha o bloco condicional[cite: 23]

app.UseHttpsRedirection(); // Redireciona automaticamente pedidos HTTP para HTTPS quando aplicável[cite: 23]

app.MapControllers(); // Mapeia as rotas definidas nos controladores[cite: 23]

app.Run(); // Inicia a aplicação e começa a escutar pedidos HTTP/HTTPS[cite: 23]
