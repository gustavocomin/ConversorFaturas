using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financeiro.Migrations
{
    /// <inheritdoc />
    public partial class Novo1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "FATURAMESANO",
                newName: "CodigoFaturaMesAno");

            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "FATURA",
                newName: "CodigoFatura");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CodigoFaturaMesAno",
                table: "FATURAMESANO",
                newName: "Codigo");

            migrationBuilder.RenameColumn(
                name: "CodigoFatura",
                table: "FATURA",
                newName: "Codigo");
        }
    }
}
