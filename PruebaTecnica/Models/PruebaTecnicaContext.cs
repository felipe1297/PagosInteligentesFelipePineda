using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PruebaTecnica.Models
{
    public partial class PruebaTecnicaContext : DbContext
    {
        public PruebaTecnicaContext()
        {
        }

        public PruebaTecnicaContext(DbContextOptions<PruebaTecnicaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Persona> Personas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-Q5FONPJ\\SQLEXPRESS; Database=PruebaTecnica; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPersona)
                    .HasName("PK__Persona__2EC8D2ACE3762C5E");

                entity.ToTable("Persona");

                entity.HasIndex(e => new { e.NombresPersona, e.ApellidosPersona }, "indexNombresApellidos");

                entity.Property(e => e.ApellidosPersona)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.CelularPersona)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CiudadPersona)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.CorreoPersona)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.DireccionPersona)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IdentificacionPersona)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.NombresPersona)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
