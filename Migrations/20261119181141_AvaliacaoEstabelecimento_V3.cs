using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_Integrador_Plataforma_Acessibilidade_Humana.Migrations
{
    /// <inheritdoc />
    public partial class AvaliacaoEstabelecimento_V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comentario",
                table: "AvaliacaoEstabelecimentos",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comentario",
                table: "AvaliacaoEstabelecimentos");
        }
    }
}
