using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ConversorFaturas.Migrations
{
    /// <inheritdoc />
    public partial class Novo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FATURAMESANO",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    MesAno = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FATURAMESANO", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "FATURA",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DATA = table.Column<DateTime>(type: "DATE", nullable: false),
                    CATEGORIA = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DESCRICAO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VALOR = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CODIGOFATURAMESANO = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FATURA", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_FATURA_FATURAMESANO_CODIGOFATURAMESANO",
                        column: x => x.CODIGOFATURAMESANO,
                        principalTable: "FATURAMESANO",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FATURA_CODIGOFATURAMESANO",
                table: "FATURA",
                column: "CODIGOFATURAMESANO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FATURA");

            migrationBuilder.DropTable(
                name: "FATURAMESANO");
        }
    }
}
