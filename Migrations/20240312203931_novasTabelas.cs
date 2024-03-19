using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Financeiro.Migrations
{
    /// <inheritdoc />
    public partial class novasTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CONTAMENSAL",
                columns: table => new
                {
                    CODIGOCONTAMENSAL = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DESCRICAO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VALOR = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTAMENSAL", x => x.CODIGOCONTAMENSAL);
                });

            migrationBuilder.CreateTable(
                name: "PARCELA",
                columns: table => new
                {
                    CODIGOPARCELA = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PARCELAATUAL = table.Column<int>(type: "INT", nullable: false),
                    PARCELAFINAL = table.Column<int>(type: "INT", nullable: false),
                    DATAINICIAL = table.Column<DateTime>(type: "DATE", nullable: false),
                    DATAFINAL = table.Column<DateTime>(type: "DATE", nullable: false),
                    CODIGOCONTAMENSAL = table.Column<int>(type: "INT", nullable: false),
                    ContaMensalCodigo = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PARCELA", x => x.CODIGOPARCELA);
                    table.ForeignKey(
                        name: "FK_PARCELA_CONTAMENSAL_CODIGOCONTAMENSAL",
                        column: x => x.CODIGOCONTAMENSAL,
                        principalTable: "CONTAMENSAL",
                        principalColumn: "CODIGOCONTAMENSAL",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PARCELA_CONTAMENSAL_ContaMensalCodigo",
                        column: x => x.ContaMensalCodigo,
                        principalTable: "CONTAMENSAL",
                        principalColumn: "CODIGOCONTAMENSAL");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PARCELA_CODIGOCONTAMENSAL",
                table: "PARCELA",
                column: "CODIGOCONTAMENSAL");

            migrationBuilder.CreateIndex(
                name: "IX_PARCELA_ContaMensalCodigo",
                table: "PARCELA",
                column: "ContaMensalCodigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PARCELA");

            migrationBuilder.DropTable(
                name: "CONTAMENSAL");
        }
    }
}
