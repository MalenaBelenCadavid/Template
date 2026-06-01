using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class competicionesYcoFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administradores",
                columns: table => new
                {
                    Dni = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administradores", x => x.Dni);
                });

            migrationBuilder.CreateTable(
                name: "Asistencia",
                columns: table => new
                {
                    IdAsistencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DniCliente = table.Column<int>(type: "int", nullable: false),
                    IdClase = table.Column<int>(type: "int", nullable: false),
                    Presente = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencia", x => x.IdAsistencia);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Dni = table.Column<int>(type: "int", nullable: false),
                    EsSocio = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Dni);
                });

            migrationBuilder.CreateTable(
                name: "Competencia",
                columns: table => new
                {
                    IdCompetencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cupos = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TipoCompetencia = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    FaseAct = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencia", x => x.IdCompetencia);
                });

            migrationBuilder.CreateTable(
                name: "Descuento",
                columns: table => new
                {
                    IdDescuento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Valor = table.Column<double>(type: "float", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Descuento", x => x.IdDescuento);
                });

            migrationBuilder.CreateTable(
                name: "Reporte",
                columns: table => new
                {
                    IdReporte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoReporte = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporte", x => x.IdReporte);
                });

            migrationBuilder.CreateTable(
                name: "TipoCancha",
                columns: table => new
                {
                    IdTipoCancha = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Superficie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCancha", x => x.IdTipoCancha);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Dni = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Localidad = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaNac = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Dni);
                });

            migrationBuilder.CreateTable(
                name: "Equipo",
                columns: table => new
                {
                    IdEquipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompetencia = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Victorias = table.Column<int>(type: "int", nullable: false),
                    Derrotas = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipo", x => x.IdEquipo);
                    table.ForeignKey(
                        name: "FK_Equipo_Competencia_IdCompetencia",
                        column: x => x.IdCompetencia,
                        principalTable: "Competencia",
                        principalColumn: "IdCompetencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cancha",
                columns: table => new
                {
                    IdCancha = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoCanchaId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cancha", x => x.IdCancha);
                    table.ForeignKey(
                        name: "FK_Cancha_TipoCancha_TipoCanchaId",
                        column: x => x.TipoCanchaId,
                        principalTable: "TipoCancha",
                        principalColumn: "IdTipoCancha",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profesional",
                columns: table => new
                {
                    Dni = table.Column<int>(type: "int", nullable: false),
                    Certificado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstaCertificado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesional", x => x.Dni);
                    table.ForeignKey(
                        name: "FK_Profesional_Usuario_Dni",
                        column: x => x.Dni,
                        principalTable: "Usuario",
                        principalColumn: "Dni",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Partido",
                columns: table => new
                {
                    IdPartido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompetencia = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<int>(type: "int", nullable: false),
                    IdEquipoLocal = table.Column<int>(type: "int", nullable: true),
                    IdEquipoVis = table.Column<int>(type: "int", nullable: true),
                    GolesLocal = table.Column<int>(type: "int", nullable: true),
                    GolesVis = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdSigPartido = table.Column<int>(type: "int", nullable: true),
                    EquipoIdEquipo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partido", x => x.IdPartido);
                    table.ForeignKey(
                        name: "FK_Partido_Competencia_IdCompetencia",
                        column: x => x.IdCompetencia,
                        principalTable: "Competencia",
                        principalColumn: "IdCompetencia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partido_Equipo_EquipoIdEquipo",
                        column: x => x.EquipoIdEquipo,
                        principalTable: "Equipo",
                        principalColumn: "IdEquipo");
                    table.ForeignKey(
                        name: "FK_Partido_Equipo_IdEquipoLocal",
                        column: x => x.IdEquipoLocal,
                        principalTable: "Equipo",
                        principalColumn: "IdEquipo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partido_Equipo_IdEquipoVis",
                        column: x => x.IdEquipoVis,
                        principalTable: "Equipo",
                        principalColumn: "IdEquipo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partido_Partido_IdSigPartido",
                        column: x => x.IdSigPartido,
                        principalTable: "Partido",
                        principalColumn: "IdPartido",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HorarioCancha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCancha = table.Column<int>(type: "int", nullable: false),
                    Dia = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioCancha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioCancha_Cancha_IdCancha",
                        column: x => x.IdCancha,
                        principalTable: "Cancha",
                        principalColumn: "IdCancha",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entrenadores",
                columns: table => new
                {
                    Dni = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrenadores", x => x.Dni);
                    table.ForeignKey(
                        name: "FK_Entrenadores_Profesional_Dni",
                        column: x => x.Dni,
                        principalTable: "Profesional",
                        principalColumn: "Dni",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profesores",
                columns: table => new
                {
                    Dni = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesores", x => x.Dni);
                    table.ForeignKey(
                        name: "FK_Profesores_Profesional_Dni",
                        column: x => x.Dni,
                        principalTable: "Profesional",
                        principalColumn: "Dni",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    IdReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DniCliente = table.Column<int>(type: "int", nullable: false),
                    IdCancha = table.Column<int>(type: "int", nullable: false),
                    IdCanchaHorario = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdDescuento = table.Column<int>(type: "int", nullable: true),
                    MontoTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EsValida = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserva", x => x.IdReserva);
                    table.ForeignKey(
                        name: "FK_Reserva_Cancha_IdCancha",
                        column: x => x.IdCancha,
                        principalTable: "Cancha",
                        principalColumn: "IdCancha",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reserva_Clientes_DniCliente",
                        column: x => x.DniCliente,
                        principalTable: "Clientes",
                        principalColumn: "Dni",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reserva_HorarioCancha_IdCanchaHorario",
                        column: x => x.IdCanchaHorario,
                        principalTable: "HorarioCancha",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entrenamiento",
                columns: table => new
                {
                    IdEntrenamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DniEntrenador = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Horario = table.Column<TimeSpan>(type: "time", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Cupo = table.Column<int>(type: "int", nullable: false),
                    IdActividad = table.Column<int>(type: "int", nullable: false),
                    EntrenadorDni = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrenamiento", x => x.IdEntrenamiento);
                    table.ForeignKey(
                        name: "FK_Entrenamiento_Entrenadores_DniEntrenador",
                        column: x => x.DniEntrenador,
                        principalTable: "Entrenadores",
                        principalColumn: "Dni",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entrenamiento_Entrenadores_EntrenadorDni",
                        column: x => x.EntrenadorDni,
                        principalTable: "Entrenadores",
                        principalColumn: "Dni");
                });

            migrationBuilder.CreateTable(
                name: "Clase",
                columns: table => new
                {
                    IdClase = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cupo = table.Column<int>(type: "int", nullable: false),
                    Dia = table.Column<DateOnly>(type: "date", nullable: false),
                    Horario = table.Column<TimeSpan>(type: "time", nullable: false),
                    DniProfesor = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IdActividad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clase", x => x.IdClase);
                    table.ForeignKey(
                        name: "FK_Clase_Profesores_DniProfesor",
                        column: x => x.DniProfesor,
                        principalTable: "Profesores",
                        principalColumn: "Dni",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inscripcion",
                columns: table => new
                {
                    IdInscripcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DniCliente = table.Column<int>(type: "int", nullable: false),
                    Horario = table.Column<DateTime>(type: "datetime", nullable: false),
                    PrecioInscr = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NroAct = table.Column<int>(type: "int", nullable: false),
                    IdAct = table.Column<int>(type: "int", nullable: false),
                    IdDescuento = table.Column<int>(type: "int", nullable: false),
                    claseIdClase = table.Column<int>(type: "int", nullable: true),
                    entrenamientoIdEntrenamiento = table.Column<int>(type: "int", nullable: true),
                    competenciaIdCompetencia = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripcion", x => x.IdInscripcion);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Clase_claseIdClase",
                        column: x => x.claseIdClase,
                        principalTable: "Clase",
                        principalColumn: "IdClase");
                    table.ForeignKey(
                        name: "FK_Inscripcion_Clientes_DniCliente",
                        column: x => x.DniCliente,
                        principalTable: "Clientes",
                        principalColumn: "Dni",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inscripcion_Competencia_competenciaIdCompetencia",
                        column: x => x.competenciaIdCompetencia,
                        principalTable: "Competencia",
                        principalColumn: "IdCompetencia");
                    table.ForeignKey(
                        name: "FK_Inscripcion_Entrenamiento_entrenamientoIdEntrenamiento",
                        column: x => x.entrenamientoIdEntrenamiento,
                        principalTable: "Entrenamiento",
                        principalColumn: "IdEntrenamiento");
                });

            migrationBuilder.CreateTable(
                name: "Cobro",
                columns: table => new
                {
                    IdCobro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdReserva = table.Column<int>(type: "int", nullable: true),
                    IdInscripcion = table.Column<int>(type: "int", nullable: true),
                    DniCliente = table.Column<int>(type: "int", nullable: false),
                    MetodoPago = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstaCompleto = table.Column<bool>(type: "bit", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cobro", x => x.IdCobro);
                    table.ForeignKey(
                        name: "FK_Cobro_Clientes_DniCliente",
                        column: x => x.DniCliente,
                        principalTable: "Clientes",
                        principalColumn: "Dni",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cobro_Inscripcion_IdInscripcion",
                        column: x => x.IdInscripcion,
                        principalTable: "Inscripcion",
                        principalColumn: "IdInscripcion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cobro_Reserva_IdReserva",
                        column: x => x.IdReserva,
                        principalTable: "Reserva",
                        principalColumn: "IdReserva",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recibo",
                columns: table => new
                {
                    IdRecibo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCobro = table.Column<int>(type: "int", nullable: false),
                    IdReserva = table.Column<int>(type: "int", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recibo", x => x.IdRecibo);
                    table.ForeignKey(
                        name: "FK_Recibo_Cobro_IdCobro",
                        column: x => x.IdCobro,
                        principalTable: "Cobro",
                        principalColumn: "IdCobro",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recibo_Reserva_IdReserva",
                        column: x => x.IdReserva,
                        principalTable: "Reserva",
                        principalColumn: "IdReserva",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cancha_TipoCanchaId",
                table: "Cancha",
                column: "TipoCanchaId");

            migrationBuilder.CreateIndex(
                name: "IX_Clase_DniProfesor",
                table: "Clase",
                column: "DniProfesor");

            migrationBuilder.CreateIndex(
                name: "IX_Cobro_DniCliente",
                table: "Cobro",
                column: "DniCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Cobro_IdInscripcion",
                table: "Cobro",
                column: "IdInscripcion",
                unique: true,
                filter: "[IdInscripcion] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cobro_IdReserva",
                table: "Cobro",
                column: "IdReserva",
                unique: true,
                filter: "[IdReserva] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenamiento_DniEntrenador",
                table: "Entrenamiento",
                column: "DniEntrenador");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenamiento_EntrenadorDni",
                table: "Entrenamiento",
                column: "EntrenadorDni");

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_IdCompetencia",
                table: "Equipo",
                column: "IdCompetencia");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioCancha_IdCancha",
                table: "HorarioCancha",
                column: "IdCancha");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_claseIdClase",
                table: "Inscripcion",
                column: "claseIdClase");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_competenciaIdCompetencia",
                table: "Inscripcion",
                column: "competenciaIdCompetencia");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_DniCliente",
                table: "Inscripcion",
                column: "DniCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripcion_entrenamientoIdEntrenamiento",
                table: "Inscripcion",
                column: "entrenamientoIdEntrenamiento");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_EquipoIdEquipo",
                table: "Partido",
                column: "EquipoIdEquipo");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_IdCompetencia",
                table: "Partido",
                column: "IdCompetencia");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_IdEquipoLocal",
                table: "Partido",
                column: "IdEquipoLocal");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_IdEquipoVis",
                table: "Partido",
                column: "IdEquipoVis");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_IdSigPartido",
                table: "Partido",
                column: "IdSigPartido");

            migrationBuilder.CreateIndex(
                name: "IX_Recibo_IdCobro",
                table: "Recibo",
                column: "IdCobro");

            migrationBuilder.CreateIndex(
                name: "IX_Recibo_IdReserva",
                table: "Recibo",
                column: "IdReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_DniCliente",
                table: "Reserva",
                column: "DniCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_IdCancha",
                table: "Reserva",
                column: "IdCancha");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_IdCanchaHorario",
                table: "Reserva",
                column: "IdCanchaHorario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administradores");

            migrationBuilder.DropTable(
                name: "Asistencia");

            migrationBuilder.DropTable(
                name: "Descuento");

            migrationBuilder.DropTable(
                name: "Partido");

            migrationBuilder.DropTable(
                name: "Recibo");

            migrationBuilder.DropTable(
                name: "Reporte");

            migrationBuilder.DropTable(
                name: "Equipo");

            migrationBuilder.DropTable(
                name: "Cobro");

            migrationBuilder.DropTable(
                name: "Inscripcion");

            migrationBuilder.DropTable(
                name: "Reserva");

            migrationBuilder.DropTable(
                name: "Clase");

            migrationBuilder.DropTable(
                name: "Competencia");

            migrationBuilder.DropTable(
                name: "Entrenamiento");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "HorarioCancha");

            migrationBuilder.DropTable(
                name: "Profesores");

            migrationBuilder.DropTable(
                name: "Entrenadores");

            migrationBuilder.DropTable(
                name: "Cancha");

            migrationBuilder.DropTable(
                name: "Profesional");

            migrationBuilder.DropTable(
                name: "TipoCancha");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
