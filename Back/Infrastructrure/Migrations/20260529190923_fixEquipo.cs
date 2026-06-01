using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixEquipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DniCliente",
                table: "Equipo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_DniCliente",
                table: "Equipo",
                column: "DniCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Clientes_DniCliente",
                table: "Equipo",
                column: "DniCliente",
                principalTable: "Clientes",
                principalColumn: "Dni",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Clientes_DniCliente",
                table: "Equipo");

            migrationBuilder.DropIndex(
                name: "IX_Equipo_DniCliente",
                table: "Equipo");

            migrationBuilder.DropColumn(
                name: "DniCliente",
                table: "Equipo");
        }
    }
}
