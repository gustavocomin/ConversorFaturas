using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Financeiro.Migrations
{
    /// <inheritdoc />
    public partial class ajuste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PARCELA");

            migrationBuilder.AddColumn<string>(
                name: "MesAnoConta",
                table: "CONTAMENSAL",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumeroParcela",
                table: "CONTAMENSAL",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MesAnoConta",
                table: "CONTAMENSAL");

            migrationBuilder.DropColumn(
                name: "NumeroParcela",
                table: "CONTAMENSAL");

            migrationBuilder.CreateTable(
                name: "PARCELA",
                columns: table => new
                {
                    CODIGOPARCELA = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CODIGOCONTAMENSAL = table.Column<int>(type: "INT", nullable: false),
                    ContaMensalCodigo = table.Column<int>(type: "integer", nullable: true),
                    DATAFINAL = table.Column<DateTime>(type: "DATE", nullable: false),
                    DATAINICIAL = table.Column<DateTime>(type: "DATE", nullable: false),
                    PARCELAATUAL = table.Column<int>(type: "INT", nullable: false),
                    PARCELAFINAL = table.Column<int>(type: "INT", nullable: false)
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
    }
}
