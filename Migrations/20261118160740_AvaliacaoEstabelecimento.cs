using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Projeto_Integrador_Plataforma_Acessibilidade_Humana.Migrations
{
    /// <inheritdoc />
    public partial class AvaliacaoEstabelecimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Avaliacao",
                table: "Estabelecimentos",
                newName: "MediaAvaliacao");

            migrationBuilder.CreateTable(
                name: "AvaliacaoEstabelecimentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EstabelecimentoId = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacaoEstabelecimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliacaoEstabelecimentos_Estabelecimentos_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacaoEstabelecimentos_EstabelecimentoId",
                table: "AvaliacaoEstabelecimentos",
                column: "EstabelecimentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvaliacaoEstabelecimentos");

            migrationBuilder.RenameColumn(
                name: "MediaAvaliacao",
                table: "Estabelecimentos",
                newName: "Avaliacao");
        }
    }
}
