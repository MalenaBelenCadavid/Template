using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixAsistenciaRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NroActividad",
                table: "Asistencia");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_DniCliente",
                table: "Asistencia",
                column: "DniCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_IdClase",
                table: "Asistencia",
                column: "IdClase");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_IdEntrenamiento",
                table: "Asistencia",
                column: "IdEntrenamiento");

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencia_Clase_IdClase",
                table: "Asistencia",
                column: "IdClase",
                principalTable: "Clase",
                principalColumn: "IdClase",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencia_Clientes_DniCliente",
                table: "Asistencia",
                column: "DniCliente",
                principalTable: "Clientes",
                principalColumn: "Dni",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Asistencia_Entrenamiento_IdEntrenamiento",
                table: "Asistencia",
                column: "IdEntrenamiento",
                principalTable: "Entrenamiento",
                principalColumn: "IdEntrenamiento",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asistencia_Clase_IdClase",
                table: "Asistencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Asistencia_Clientes_DniCliente",
                table: "Asistencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Asistencia_Entrenamiento_IdEntrenamiento",
                table: "Asistencia");

            migrationBuilder.DropIndex(
                name: "IX_Asistencia_DniCliente",
                table: "Asistencia");

            migrationBuilder.DropIndex(
                name: "IX_Asistencia_IdClase",
                table: "Asistencia");

            migrationBuilder.DropIndex(
                name: "IX_Asistencia_IdEntrenamiento",
                table: "Asistencia");

            migrationBuilder.AddColumn<int>(
                name: "NroActividad",
                table: "Asistencia",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
