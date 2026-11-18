using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Projeto_Integrador_Plataforma_Acessibilidade_Humana.Migrations
{
    /// <inheritdoc />
    public partial class Denuncia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Denuncias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(type: "longtext", nullable: false),
                    CaminhoId = table.Column<int>(type: "int", nullable: true),
                    EstabelecimentoId = table.Column<int>(type: "int", nullable: true),
                    DataOcorrencia = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denuncias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Denuncias_Caminhos_CaminhoId",
                        column: x => x.CaminhoId,
                        principalTable: "Caminhos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Denuncias_Estabelecimentos_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_CaminhoId",
                table: "Denuncias",
                column: "CaminhoId");

            migrationBuilder.CreateIndex(
                name: "IX_Denuncias_EstabelecimentoId",
                table: "Denuncias",
                column: "EstabelecimentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Denuncias");
        }
    }
}
