// =========================================================================================
// MIGRATION INICIAL DO ENTITY FRAMEWORK CORE (Migrations/20261120034145_CriacaoInicial.cs)
// Conceitos abordados neste arquivo:
// [1] ORM com Entity Framework Core (evolução versionada do esquema do banco de dados)
// [4] Relacionamento 1 para Muitos (1:N) — criação física da Foreign Key FK_Funcionarios_Gestores_GestorId
// =========================================================================================

// Importa a infraestrutura de Migrations do Entity Framework Core
using Microsoft.EntityFrameworkCore.Migrations;

// Desativa avisos de nulabilidade em código gerado automaticamente
#nullable disable

// Define o namespace das migrations do projeto
namespace APIGestaoRH.Migrations
{
    /// <inheritdoc />
    // Classe gerada pelo comando "dotnet ef migrations add CriacaoInicial"
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        // Método Up: executado pelo "dotnet ef database update" para criar as tabelas no SQLite
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // [1] Cria a tabela "Gestores" (lado 1 do relacionamento)
            migrationBuilder.CreateTable(
                name: "Gestores",
                columns: table => new
                {
                    // Coluna Id como inteiro autoincrementável
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    // Colunas de texto obrigatórias mapeadas da classe Gestor
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Setor = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    // Define Id como Chave Primária da tabela Gestores
                    table.PrimaryKey("PK_Gestores", x => x.Id);
                });

            // [1] [4] Cria a tabela "Funcionarios" (lado N do relacionamento)
            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    // Coluna Id como inteiro autoincrementável
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    // Colunas mapeadas da classe Funcionario
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cpf = table.Column<string>(type: "TEXT", nullable: false),
                    Cargo = table.Column<string>(type: "TEXT", nullable: false),
                    Setor = table.Column<string>(type: "TEXT", nullable: false),
                    Salario = table.Column<double>(type: "REAL", nullable: false),
                    // Coluna de chave estrangeira que aponta para Gestores.Id
                    GestorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    // Define Id como Chave Primária da tabela Funcionarios
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                    // [4] Cria a restrição física de Chave Estrangeira (1:N) no banco SQLite:
                    // Funcionarios.GestorId referencia Gestores.Id com exclusão restrita (Restrict)
                    table.ForeignKey(
                        name: "FK_Funcionarios_Gestores_GestorId",
                        column: x => x.GestorId,
                        principalTable: "Gestores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // [4] Cria um índice na coluna GestorId para acelerar consultas e JOINs do relacionamento 1:N
            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_GestorId",
                table: "Funcionarios",
                column: "GestorId");
        }

        /// <inheritdoc />
        // Método Down: desfaz a migration caso seja necessário reverter o banco (remove primeiro a tabela dependente)
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "Gestores");
        }
    }
}
