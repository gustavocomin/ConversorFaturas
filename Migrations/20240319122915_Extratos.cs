using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Financeiro.Migrations
{
    /// <inheritdoc />
    public partial class Extratos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "CONTAMENSAL",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "EXTRATOMESANO",
                columns: table => new
                {
                    CodigoExtratoMesAno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    MesAno = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXTRATOMESANO", x => x.CodigoExtratoMesAno);
                });

            migrationBuilder.CreateTable(
                name: "EXTRATO",
                columns: table => new
                {
                    CodigoExtrato = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Identificador = table.Column<Guid>(type: "uuid", nullable: false),
                    CODIGOEXTRATOMESANO = table.Column<int>(type: "INT", nullable: false),
                    DESCRICAO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VALOR = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    DATA = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXTRATO", x => x.CodigoExtrato);
                    table.ForeignKey(
                        name: "FK_EXTRATO_EXTRATOMESANO_CODIGOEXTRATOMESANO",
                        column: x => x.CODIGOEXTRATOMESANO,
                        principalTable: "EXTRATOMESANO",
                        principalColumn: "CodigoExtratoMesAno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EXTRATO_CODIGOEXTRATOMESANO",
                table: "EXTRATO",
                column: "CODIGOEXTRATOMESANO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EXTRATO");

            migrationBuilder.DropTable(
                name: "EXTRATOMESANO");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "CONTAMENSAL");
        }
    }
}
