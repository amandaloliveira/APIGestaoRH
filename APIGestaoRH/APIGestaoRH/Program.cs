using APIGestaoRH.Data;
using APIGestaoRH.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registra os controllers (as classes que recebem as requisições HTTP)
builder.Services.AddControllers();

// Registra o AppDbContext e diz que o banco é SQLite,
// usando a connection string do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositórios com Entity Framework Core
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepositoryEf>();
builder.Services.AddScoped<IGestorRepository, GestorRepositoryEf>();

// Alternativa em memória (para testes sem banco, igual ao Passo 7 do projeto original):
// builder.Services.AddSingleton<IFuncionarioRepository, FuncionarioRepositoryMemoria>();

// Serviços com regras de negócio
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IGestorService, GestorService>();

// Swagger: gera a documentação da API e uma página para testar os endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Em desenvolvimento, habilita a página do Swagger em /swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();