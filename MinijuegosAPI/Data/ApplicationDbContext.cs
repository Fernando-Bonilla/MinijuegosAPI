using Microsoft.EntityFrameworkCore;
using MinijuegosAPI;
using MinijuegosAPI.Models;
using System.Diagnostics.CodeAnalysis;

namespace MinijuegosAPI.Data
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
        }

        // aca agregar las tablas a mapear

        public DbSet<Pregunta> Preguntas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pregunta>()
                .HasKey(p => p.Id);


        }
    }


}
