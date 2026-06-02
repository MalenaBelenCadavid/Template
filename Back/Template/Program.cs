using Application.Interfaces;
using Application.Interfaces.Administrador;
using Application.Interfaces.Asistencia;
using Application.Interfaces.Cancha;
using Application.Interfaces.Clase;
using Application.Interfaces.Cliente;
using Application.Interfaces.Competencias;
using Application.Interfaces.Equipos;
using Application.Interfaces.Partidos;
using Application.Interfaces.Entrenamiento;
using Application.Interfaces.HorarioCancha;
using Application.Interfaces.Incripcion;
using Application.Interfaces.Profesionales;
using Application.Interfaces.Reserva;
using Application.Interfaces.TipoCancha;
using Application.UseCases;
using Application.Mappers.Profesional;
using Infrastructrure.Command;
using Infrastructrure.Query;
using Infrastructure.Command;
using Infrastructure.Persistence;
using Infrastructure.Query;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Cobro;
using Application.Interfaces.MetricasService;
using Application.Interfaces.Recibo;
using Application.UseCases.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.MapType<TimeSpan>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Example = new Microsoft.OpenApi.Any.OpenApiString("10:00:00")
    });

    c.MapType<DayOfWeek>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Example = new Microsoft.OpenApi.Any.OpenApiString("Monday")
    });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));




//Inyecciones de dependencia

//profesionales
builder.Services.AddScoped<IProfesionalCommand, ProfesionalCommand>();
builder.Services.AddScoped<IProfesionalQuery, ProfesionalQuery>();
builder.Services.AddScoped<IProfesionalService, ProfesionalService>();
builder.Services.AddScoped<IProfesionalMapper, ProfesionalMapper>();


//Cancha
builder.Services.AddScoped<ICanchaCommand, CanchaCommand>();
builder.Services.AddScoped<ICanchaQuery, CanchaQuery>();
builder.Services.AddScoped<ICanchaService, CanchaService>();

//Administrador
builder.Services.AddScoped<IAdministradorService, AdministadorService>();
builder.Services.AddScoped<IAdministradorQuery, AdministradorQuery>();
builder.Services.AddScoped<IAdministradorCommand, AdministradorCommand>();


//TipoCancha
builder.Services.AddScoped<ITipoCanchaService, TipoCanchaService>();
builder.Services.AddScoped<ITipoCanchaCommand, TipoCanchaCommand>();
builder.Services.AddScoped<ITipoCanchaQuery, TipoCanchaQuery>();


//Reservas
builder.Services.AddScoped<IReservaServices, ReservaService>();
builder.Services.AddScoped<IReservaCommand, ReservaCommand>();
builder.Services.AddScoped<IReservaQuery, ReservaQuery>();

//Reporte
builder.Services.AddScoped<IReporteService, ReporteService>();
builder.Services.AddScoped<IReporteCommand, ReporteCommand>();
builder.Services.AddScoped<IReporteQuery, ReporteQuery>();

//Clientes
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteCommand, ClienteCommand>();
builder.Services.AddScoped<IClienteQuery, ClienteQuery>();

//Cobros
builder.Services.AddScoped<ICobroService, CobroService>();
builder.Services.AddScoped<ICobroQuery, CobroQuery>();
builder.Services.AddScoped<ICobroCommand, CobroCommand>();

//Competencias
builder.Services.AddScoped<ICompetenciaService, CompetenciaService>();
builder.Services.AddScoped<ICompetenciaCommand, CompetenciaCommand>();
builder.Services.AddScoped<ICompetenciaQuery, CompetenciaQuery>();

//Ligas
builder.Services.AddScoped<ILigaService, LigaService>();

//Torneos
builder.Services.AddScoped<ITorneoService, TorneoService>();
//Equipos
builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<IEquipoCommand, EquipoCommand>();
builder.Services.AddScoped<IEquipoQuery, EquipoQuery>();

//Partidos
builder.Services.AddScoped<IPartidoService, PartidoService>();
builder.Services.AddScoped<IPartidoCommand, PartidoCommand>();
builder.Services.AddScoped<IPartidoQuery, PartidoQuery>();


//Horario Cancha
builder.Services.AddScoped<IHorarioCanchaService, HorarioCanchaService>();
builder.Services.AddScoped<IHorarioCanchaQuery, HorarioCanchaQuery>();
builder.Services.AddScoped<IHorarioCanchaCommand, HorarioCanchaCommand>();
//Asistencia
builder.Services.AddScoped<IAsistenciaService, AsistenciaService>();
builder.Services.AddScoped<IAsistenciaQuery, AsistenciaQuery>();
builder.Services.AddScoped<IAsistenciaCommand, AsistenciaCommand>();
//Entrenamiento
builder.Services.AddScoped<IEntrenamientoService, EntrenamientoService>();
builder.Services.AddScoped<IEntrenamientoQuery, EntrenamientoQuery>();
builder.Services.AddScoped<IEntrenamientoCommand, EntrenamientoCommand>();
//Clase
builder.Services.AddScoped<IClaseService, ClaseService>();
builder.Services.AddScoped<IClaseQuery, ClaseQuery>();
builder.Services.AddScoped<IClaseCommand, ClaseCommand>();
//Inscripción
builder.Services.AddScoped<IInscripcionService, InscripcionService>();
builder.Services.AddScoped<IInscripcionQuery, InscripcionQuery>();
builder.Services.AddScoped<IInscripcionCommand, InscripcionCommand>();
//Descuento
builder.Services.AddScoped<IDescuentoCommand, DescuentoCommand>();
builder.Services.AddScoped<IDescuentoQuery, DescuentoQuery>();
builder.Services.AddScoped<IDescuentoService, DescuentoService>();
//Metricas
builder.Services.AddScoped<IMetricasService, MetricasService>();
//Recibos
builder.Services.AddScoped<IReciboCommand, ReciboCommand>();
builder.Services.AddScoped<IReciboQuery, ReciboQuery>();
builder.Services.AddScoped<IReciboService, ReciboService>();


//CORS
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowFront", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFront");


app.UseAuthorization();

app.MapControllers();

app.Run();
