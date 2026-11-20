using Microsoft.EntityFrameworkCore.Migrations; // Importa classes base de migrações do EF Core[cite: 8]

#nullable disable // Desativa a verificação de nulidade neste arquivo de migração gerado[cite: 8]

namespace APIGestaoRH.Migrations // Define o namespace das migrações do projeto[cite: 8]
{
    /// <inheritdoc /> // Herda a documentação XML da classe base Migration[cite: 8]
    public partial class CriacaoInicial : Migration // Declara a migração parcial CriacaoInicial herdando de Migration[cite: 8]
    {
        /// <inheritdoc /> // Herda a documentação XML do método Up[cite: 8]
        protected override void Up(MigrationBuilder migrationBuilder) // Define as operações para aplicar a migração (criar tabelas/índices)[cite: 8]
        {
            migrationBuilder.CreateTable( // Cria a tabela "Gestores" no banco de dados[cite: 8]
                name: "Gestores", // Nome da tabela[cite: 8]
                columns: table => new // Define as colunas da tabela Gestores[cite: 8]
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false) // Coluna Id do tipo inteiro não nulo[cite: 8]
                        .Annotation("Sqlite:Autoincrement", true), // Configura auto-incremento no SQLite[cite: 8]
                    Nome = table.Column<string>(type: "TEXT", nullable: false), // Coluna Nome do tipo texto obrigatório[cite: 8]
                    Setor = table.Column<string>(type: "TEXT", nullable: false), // Coluna Setor do tipo texto obrigatório[cite: 8]
                    Email = table.Column<string>(type: "TEXT", nullable: false) // Coluna Email do tipo texto obrigatório[cite: 8]
                }, // Fim da definição de colunas de Gestores[cite: 8]
                constraints: table => // Define as restrições da tabela Gestores[cite: 8]
                {
                    table.PrimaryKey("PK_Gestores", x => x.Id); // Define Id como chave primária[cite: 8]
                }); // Fim da criação da tabela Gestores[cite: 8]

            migrationBuilder.CreateTable( // Cria a tabela "Funcionarios" no banco de dados[cite: 8]
                name: "Funcionarios", // Nome da tabela[cite: 8]
                columns: table => new // Define as colunas da tabela Funcionarios[cite: 8]
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false) // Coluna Id inteira obrigatória[cite: 8]
                        .Annotation("Sqlite:Autoincrement", true), // Ativa auto-incremento no SQLite[cite: 8]
                    Nome = table.Column<string>(type: "TEXT", nullable: false), // Coluna Nome do tipo texto obrigatório[cite: 8]
                    Cpf = table.Column<string>(type: "TEXT", nullable: false), // Coluna Cpf do tipo texto obrigatório[cite: 8]
                    Cargo = table.Column<string>(type: "TEXT", nullable: false), // Coluna Cargo do tipo texto obrigatório[cite: 8]
                    Setor = table.Column<string>(type: "TEXT", nullable: false), // Coluna Setor do tipo texto obrigatório[cite: 8]
                    Salario = table.Column<double>(type: "REAL", nullable: false), // Coluna Salario do tipo real (ponto flutuante) obrigatório[cite: 8]
                    GestorId = table.Column<int>(type: "INTEGER", nullable: false) // Coluna de chave estrangeira GestorId inteira obrigatória[cite: 8]
                }, // Fim da definição de colunas de Funcionarios[cite: 8]
                constraints: table => // Define as restrições da tabela Funcionarios[cite: 8]
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id); // Define Id como chave primária de Funcionarios[cite: 8]
                    table.ForeignKey( // Cria a restrição de chave estrangeira para Gestores[cite: 8]
                        name: "FK_Funcionarios_Gestores_GestorId", // Nome da chave estrangeira[cite: 8]
                        column: x => x.GestorId, // Coluna local que referencia a outra tabela[cite: 8]
                        principalTable: "Gestores", // Tabela principal referenciada[cite: 8]
                        principalColumn: "Id", // Coluna referenciada na tabela Gestores[cite: 8]
                        onDelete: ReferentialAction.Restrict); // Impede excluir um Gestor se houver Funcionarios vinculados[cite: 8]
                }); // Fim da criação da tabela Funcionarios[cite: 8]

            migrationBuilder.CreateIndex( // Cria um índice na tabela Funcionarios para acelerar buscas por GestorId[cite: 8]
                name: "IX_Funcionarios_GestorId", // Nome do índice[cite: 8]
                table: "Funcionarios", // Tabela onde o índice será criado[cite: 8]
                column: "GestorId"); // Coluna indexada[cite: 8]
        } // Fecha o método Up[cite: 8]

        /// <inheritdoc /> // Herda a documentação XML do método Down[cite: 8]
        protected override void Down(MigrationBuilder migrationBuilder) // Define as operações para reverter a migração[cite: 8]
        {
            migrationBuilder.DropTable( // Remove a tabela Funcionarios primeiro devido à dependência de chave estrangeira[cite: 8]
                name: "Funcionarios"); // Nome da tabela a remover[cite: 8]

            migrationBuilder.DropTable( // Remove a tabela Gestores[cite: 8]
                name: "Gestores"); // Nome da tabela a remover[cite: 8]
        } // Fecha o método Down[cite: 8]
    } // Fecha a classe CriacaoInicial[cite: 8]
} // Fecha o namespace APIGestaoRH.Migrations[cite: 8]
