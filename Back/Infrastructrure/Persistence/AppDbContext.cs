using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Profesional> Profesionales { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Entrenador> Entrenadores { get; set; }
        public DbSet<Cancha> Canchas { get; set; }
        public DbSet<HorarioCancha>HorarioCancha { get; set; }
        public DbSet<TipoCancha> TiposCancha { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Cobro> Cobros { get; set; }
        public DbSet<Recibo> Recibos { get; set; }
        public DbSet<Descuento> Descuentos { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<Entrenamiento> Entrenamientos { get; set; }
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Liga> Ligas { get; set; }
        public DbSet<Torneo> Torneos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // HERENCIA

            modelBuilder.Entity<Administrador>()
                .HasBaseType<Usuario>();

            modelBuilder.Entity<Cliente>()
                .HasBaseType<Usuario>();

            modelBuilder.Entity<Profesional>()
                .HasBaseType<Usuario>();

            modelBuilder.Entity<Profesor>()
                .HasBaseType<Profesional>();

            modelBuilder.Entity<Entrenador>()
                .HasBaseType<Profesional>();

            modelBuilder.Entity<Liga>()
                .HasBaseType<Competencia>();

            modelBuilder.Entity<Torneo>()
                .HasBaseType<Competencia>();

            // USUARIO

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.HasKey(u => u.Dni);

                entity.Property(u => u.Dni)
                    .ValueGeneratedNever();

                entity.Property(u => u.Nombre)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(u => u.Apellido)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(u => u.Localidad)
                    .HasMaxLength(30);

                entity.Property(u => u.Pais)
                    .HasMaxLength(30);

                entity.Property(u => u.Correo)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.Password)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.FechaNac)
                    .HasColumnType("date")
                    .IsRequired();
            });

            // PROFESIONAL

            modelBuilder.Entity<Profesional>(entity =>
            {
                entity.ToTable("Profesional");

                entity.Property(p => p.Certificado)
                    .HasMaxLength(100);
            });

            // TIPO CANCHA

            modelBuilder.Entity<TipoCancha>(entity =>
            {
                entity.ToTable("TipoCancha");

                entity.HasKey(t => t.IdTipoCancha);

                entity.Property(t => t.IdTipoCancha)
                    .ValueGeneratedOnAdd();

                entity.Property(t => t.Nombre)
                    .HasMaxLength(20)
                    .IsRequired();
            });

            // CANCHA

            modelBuilder.Entity<Cancha>(entity =>
            {
                entity.ToTable("Cancha");

                entity.HasKey(c => c.IdCancha);

                entity.Property(c => c.IdCancha)
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.Nombre)
                    .HasMaxLength(20);

                entity.HasOne(c => c.TipoCancha)
                    .WithMany()
                    .HasForeignKey(c => c.TipoCanchaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //HORARIO CANCHA
            
            modelBuilder.Entity<HorarioCancha>(entity =>
            {
                entity.ToTable("HorarioCancha");

                entity.HasKey(h => h.Id);

                entity.Property(h => h.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(h => h.Dia)
                    .IsRequired();

                entity.Property(h => h.HoraInicio)
                    .HasColumnType("time")
                    .IsRequired();

                entity.Property(h => h.HoraFin)
                    .HasColumnType("time")
                    .IsRequired();

                entity.HasOne(h => h.Cancha)
                    .WithMany(c => c.Disponibilidad)
                    .HasForeignKey(h => h.IdCancha)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CLASE

            modelBuilder.Entity<Clase>(entity =>
            {
                entity.ToTable("Clase");

                entity.HasKey(c => c.IdClase);

                entity.Property(c => c.IdClase)
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.Precio)
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(c => c.Profesor)
                    .WithMany(p => p.Clases)
                    .HasForeignKey(c => c.DniProfesor)
                    .HasPrincipalKey(p => p.Dni)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ENTRENAMIENTO

            modelBuilder.Entity<Entrenamiento>(entity =>
            {
                entity.ToTable("Entrenamiento");

                entity.HasKey(e => e.IdEntrenamiento);

                entity.Property(e => e.IdEntrenamiento)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.Entrenador)
                    .WithMany()
                    .HasForeignKey(e => e.DniEntrenador)
                    .HasPrincipalKey(e => e.Dni)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // COMPETENCIA

            modelBuilder.Entity<Competencia>(entity =>
            {
                entity.ToTable("Competencia");

                entity.HasKey(c => c.IdCompetencia);

                entity.Property(c => c.IdCompetencia)
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.Nombre)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(c => c.Descripcion)
                    .HasMaxLength(200);

                entity.Property(c => c.Precio)
                    .HasColumnType("decimal(10,2)");


                entity.HasDiscriminator<string>("TipoCompetencia")
                .HasValue<Liga>("Liga")
                .HasValue<Torneo>("Torneo");
            });

            // TORNEO

            modelBuilder.Entity<Torneo>(entity =>
            {

                entity.Property(t => t.FaseAct)
                    .HasMaxLength(30);

                entity.Ignore(t => t.Llaves);
            });
            // EQUIPO

            modelBuilder.Entity<Equipo>(entity =>
            {
                entity.ToTable("Equipo");

                entity.HasKey(e => e.IdEquipo);

                entity.Property(e => e.IdEquipo)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .IsRequired();
                entity.HasOne(e => e.Competencia)
                .WithMany(c => c.Equipos)
                .HasForeignKey(e => e.IdCompetencia);

                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.Equipos)
                    .HasForeignKey(e => e.DniCliente);
            
            });

            // PARTIDO

            modelBuilder.Entity<Partido>(entity => { entity.ToTable("Partido");

                entity.HasKey(p => p.IdPartido);

                entity.Property(p => p.IdPartido)
                .ValueGeneratedOnAdd();

                entity.Property(p => p.GolesLocal).
                HasColumnType("int");

                entity.Property(p => p.GolesVis).
                HasColumnType("int");

                entity.Property(p => p.Estado)
                .HasMaxLength(30);

                entity.Property(p => p.HoraInicio)
                .HasColumnType("datetime");

                entity.Property(p => p.HoraFin)
                .HasColumnType("datetime");

                entity.HasOne(p => p.EquipoLocal)
                .WithMany().HasForeignKey(p => p.IdEquipoLocal)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.EquipoVis)
                .WithMany().HasForeignKey(p => p.IdEquipoVis)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Competencia)
                .WithMany(c => c.Partidos).HasForeignKey(p => p.IdCompetencia)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.SigPartido)
                .WithMany()
                .HasForeignKey(p => p.IdSigPartido)
                .OnDelete(DeleteBehavior.Restrict);
            });

            // RESERVA

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("Reserva");

                entity.HasKey(r => r.IdReserva);

                entity.Property(r => r.IdReserva)
                    .ValueGeneratedOnAdd();

                entity.Property(r => r.IdCanchaHorario)
                    .HasColumnType("int");

                entity.Property(r => r.MontoTotal)
                    .HasColumnType("decimal(10,2)");
                entity.Property(r => r.Fecha)
                    .HasColumnType("date");

                entity.HasOne(r => r.Cliente)
                    .WithMany(c => c.Reservas)
                    .HasForeignKey(r => r.DniCliente)
                    .HasPrincipalKey(c => c.Dni)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Cancha)
                    .WithMany()
                    .HasForeignKey(r => r.IdCancha)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.HorarioCancha)
                .WithMany()
                .HasForeignKey(r => r.IdCanchaHorario)
                .OnDelete(DeleteBehavior.Restrict);
                });

            // COBRO

            modelBuilder.Entity<Cobro>(entity =>
            {
                entity.ToTable("Cobro");

                entity.HasKey(c => c.IdCobro);

                entity.Property(c => c.IdCobro)
                    .ValueGeneratedOnAdd();

                entity.Property(c => c.MontoTotal)
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(c => c.Reserva)
                    .WithOne(r=>r.Cobro)
                    .HasForeignKey<Cobro>(c => c.IdReserva)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Inscripcion)
               .WithOne(i=>i.Cobro)
               .HasForeignKey<Cobro>(c => c.IdInscripcion)
               .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Cliente)
                    .WithMany(cli => cli.Cobros)
                    .HasForeignKey(c => c.DniCliente)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // RECIBO

            modelBuilder.Entity<Recibo>(entity =>
            {
                entity.ToTable("Recibo");

                entity.HasKey(r => r.IdRecibo);

                entity.Property(r => r.IdRecibo)
                    .ValueGeneratedOnAdd();

                entity.Property(r => r.MontoTotal)
                    .HasColumnType("decimal(10,2)");

                entity.Property(r => r.FechaEmision)
                    .HasColumnType("datetime");

                entity.HasOne(r => r.Cobro)
                    .WithMany()
                    .HasForeignKey(r => r.IdCobro)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Reserva)
                    .WithMany()
                    .HasForeignKey(r => r.IdReserva)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // DESCUENTO

            modelBuilder.Entity<Descuento>(entity =>
            {
                entity.ToTable("Descuento");

                entity.HasKey(d => d.IdDescuento);

                entity.Property(d => d.IdDescuento)
                    .ValueGeneratedOnAdd();

                entity.Property(d => d.Descripcion)
                    .HasMaxLength(100);

                entity.Property(d => d.FechaInicio)
                    .HasColumnType("datetime");

                entity.Property(d => d.FechaFin)
                    .HasColumnType("datetime");

                entity.Property(d => d.TipoDescuento)
                    .HasMaxLength(20).IsRequired();
                    
                entity.Property(d => d.Activo)
                    .IsRequired();
                    
            });

            // INSCRIPCION

            modelBuilder.Entity<Inscripcion>(entity =>
            {
                entity.ToTable("Inscripcion");

                entity.HasKey(i => i.IdInscripcion);

                entity.Property(i => i.IdInscripcion)
                    .ValueGeneratedOnAdd();

                entity.Property(i => i.Horario)
                    .HasColumnType("datetime");

                entity.Property(i => i.PrecioInscr)
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(i => i.cliente)
                    .WithMany(c => c.Inscripciones)
                    .HasForeignKey(i => i.DniCliente)
                    .HasPrincipalKey(c => c.Dni)
                    .OnDelete(DeleteBehavior.Restrict);
                    
            });

            // ASISTENCIA

            modelBuilder.Entity<Asistencia>(entity =>
            {
                entity.ToTable("Asistencia");

                entity.HasKey(a => a.IdAsistencia);

                entity.Property(a => a.IdAsistencia)
                    .ValueGeneratedOnAdd();

                entity.HasOne(a => a.Cliente)
                .WithMany(c => c.Asistencias)
                .HasForeignKey(a => a.DniCliente)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a=>a.Clase)
                .WithMany(c=>c.Asistencias)
                .HasForeignKey(a=>a.IdClase)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Entrenamiento)
                .WithMany(e => e.Asistencias)
                .HasForeignKey(a => a.IdEntrenamiento)
                .OnDelete(DeleteBehavior.Restrict);


            });



            // REPORTE

            modelBuilder.Entity<Reporte>(entity =>
            {
                entity.ToTable("Reporte");

                entity.HasKey(r => r.IdReporte);

                entity.Property(r => r.IdReporte)
                    .ValueGeneratedOnAdd();

                entity.Property(r => r.TipoReporte)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(r => r.FechaEmision)
                    .HasColumnType("datetime");
            });
        }
    }
}
