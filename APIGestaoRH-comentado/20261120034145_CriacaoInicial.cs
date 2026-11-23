using Microsoft.EntityFrameworkCore.Migrations; // Importa classes base de migrações do EF Core
#nullable disable // Desativa a verificação de nulidade neste arquivo de migração gerado

namespace APIGestaoRH.Migrations // Define o namespace das migrações do projeto
{
    /// <inheritdoc /> // Herda a documentação XML da classe base Migration
    public partial class CriacaoInicial : Migration // Declara a migração parcial CriacaoInicial herdando de Migration
    {
        /// <inheritdoc /> // Herda a documentação XML do método Up
        protected override void Up(MigrationBuilder migrationBuilder) // Define as operações para aplicar a migração (criar tabelas/índices)
        {
            migrationBuilder.CreateTable( // Cria a tabela "Gestores" no banco de dados
                name: "Gestores", // Nome da tabela
                columns: table => new // Define as colunas da tabela Gestores
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false) // Coluna Id do tipo inteiro não nulo
                        .Annotation("Sqlite:Autoincrement", true), // Configura auto-incremento no SQLite
                    Nome = table.Column<string>(type: "TEXT", nullable: false), // Coluna Nome do tipo texto obrigatório
                    Setor = table.Column<string>(type: "TEXT", nullable: false), // Coluna Setor do tipo texto obrigatório
                    Email = table.Column<string>(type: "TEXT", nullable: false) // Coluna Email do tipo texto obrigatório
                }, // Fim da definição de colunas de Gestores
                constraints: table => // Define as restrições da tabela Gestores
                {
                    table.PrimaryKey("PK_Gestores", x => x.Id); // Define Id como chave primária
                }); // Fim da criação da tabela Gestores

            migrationBuilder.CreateTable( // Cria a tabela "Funcionarios" no banco de dados
                name: "Funcionarios", // Nome da tabela
                columns: table => new // Define as colunas da tabela Funcionarios
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false) // Coluna Id inteira obrigatória
                        .Annotation("Sqlite:Autoincrement", true), // Ativa auto-incremento no SQLite
                    Nome = table.Column<string>(type: "TEXT", nullable: false), // Coluna Nome do tipo texto obrigatório
                    Cpf = table.Column<string>(type: "TEXT", nullable: false), // Coluna Cpf do tipo texto obrigatório
                    Cargo = table.Column<string>(type: "TEXT", nullable: false), // Coluna Cargo do tipo texto obrigatório
                    Setor = table.Column<string>(type: "TEXT", nullable: false), // Coluna Setor do tipo texto obrigatório
                    Salario = table.Column<double>(type: "REAL", nullable: false), // Coluna Salario do tipo real (ponto flutuante) obrigatório
                    GestorId = table.Column<int>(type: "INTEGER", nullable: false) // Coluna de chave estrangeira GestorId inteira obrigatória
                }, // Fim da definição de colunas de Funcionarios
                constraints: table => // Define as restrições da tabela Funcionarios
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id); // Define Id como chave primária de Funcionarios
                    table.ForeignKey( // Cria a restrição de chave estrangeira para Gestores
                        name: "FK_Funcionarios_Gestores_GestorId", // Nome da chave estrangeira
                        column: x => x.GestorId, // Coluna local que referencia a outra tabela
                        principalTable: "Gestores", // Tabela principal referenciada
                        principalColumn: "Id", // Coluna referenciada na tabela Gestores
                        onDelete: ReferentialAction.Restrict); // Impede excluir um Gestor se houver Funcionarios vinculados
                }); // Fim da criação da tabela Funcionarios

            migrationBuilder.CreateIndex( // Cria um índice na tabela Funcionarios para acelerar buscas por GestorId
                name: "IX_Funcionarios_GestorId", // Nome do índice
                table: "Funcionarios", // Tabela onde o índice será criado
                column: "GestorId"); // Coluna indexada
        } // Fecha o método Up

        /// <inheritdoc /> // Herda a documentação XML do método Down
        protected override void Down(MigrationBuilder migrationBuilder) // Define as operações para reverter a migração
        {
            migrationBuilder.DropTable( // Remove a tabela Funcionarios primeiro devido à dependência de chave estrangeira
                name: "Funcionarios"); // Nome da tabela a remover

            migrationBuilder.DropTable( // Remove a tabela Gestores
                name: "Gestores"); // Nome da tabela a remover
        } // Fecha o método Down
    } // Fecha a classe CriacaoInicial
} // Fecha o namespace APIGestaoRH.Migrations
