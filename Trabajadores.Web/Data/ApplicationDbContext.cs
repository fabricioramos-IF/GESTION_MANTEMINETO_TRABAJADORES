using Microsoft.EntityFrameworkCore;
using Trabajadores.Web.Models;

namespace Trabajadores.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trabajador> Trabajadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trabajador>(entity =>
            {
                entity.ToTable("Trabajador");
                entity.HasKey(e => e.IdTrabajador);
                entity.Property(e => e.IdTrabajador).ValueGeneratedOnAdd();
                
                entity.Property(e => e.Nombres).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellidos).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TipoDocumento).IsRequired().HasMaxLength(20);
                entity.Property(e => e.NumeroDocumento).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Sexo).IsRequired().HasMaxLength(1).IsFixedLength();
                entity.Property(e => e.FechaNacimiento).IsRequired();
                entity.Property(e => e.Foto).HasMaxLength(255);
                entity.Property(e => e.Direccion).HasMaxLength(200);

                entity.HasIndex(e => new { e.TipoDocumento, e.NumeroDocumento })
                    .IsUnique()
                    .HasDatabaseName("UQ_Trabajador_Documento");
            });
        }
    }
}
