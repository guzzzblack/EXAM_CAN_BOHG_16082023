using API_REST.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace API_REST.Data
{
    public class _context : DbContext
    {
        public _context(DbContextOptions<_context> options)
            : base(options)
        {
        }

        public DbSet<Persona> Personas { get; set; }

        public virtual async Task<int> SP_NuevaPersona(string nombre, string paterno, string materno, string rfc, DateTime fNacimiento, string usuario)
        {
            var parametros = new[]
            {
                new SqlParameter("@Nombre", nombre),
                new SqlParameter("@Paterno", paterno),
                new SqlParameter("@Materno", materno),
                new SqlParameter("@RFC", rfc),
                new SqlParameter("@FNacimiento", fNacimiento),
                new SqlParameter("@Usuario", usuario)
            };

            return await Database.ExecuteSqlRawAsync("EXEC SP_NuevaPersona @Nombre, @Paterno, @Materno, @RFC, @FNacimiento, @Usuario", parametros);
        }

        public virtual async Task<int> SP_Actualizar(int idPer, string nombre, string paterno, string materno, string rfc, DateTime fNacimiento, int usuario)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdPer", idPer),
                new SqlParameter("@Nombre", nombre),
                new SqlParameter("@Paterno", paterno),
                new SqlParameter("@Materno", materno),
                new SqlParameter("@RFC", rfc),
                new SqlParameter("@FNacimiento", fNacimiento),
                new SqlParameter("@Usuario", usuario)
            };

            return await Database.ExecuteSqlRawAsync("EXEC SP_Actualizar @IdPer, @Nombre, @Paterno, @Materno, @RFC, @FNacimiento, @Usuario", parametros);
        }

        public virtual async Task<int> SP_Eliminar(int idPer, int usuario)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdPer", idPer),
                new SqlParameter("@Usuario", usuario)
            };

            return await Database.ExecuteSqlRawAsync("EXEC SP_Eliminar @IdPer, @Usuario", parametros);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPer)
                    .HasName("PK__Persona__2ACE59B515AC71BB");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Paterno)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Materno)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RFC)
                    .HasMaxLength(13)
                    .IsUnicode(false);
                entity.ToTable("Persona");
            });

            // Otros mapeos si los tienes
        }
    }
}
