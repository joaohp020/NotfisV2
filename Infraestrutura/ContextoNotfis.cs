using Infraestrutura.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infraestrutura
{
    public class ContextoNotfis : DbContext
    {
        public ContextoNotfis(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<NotaFiscal> NotasFiscais { get; set; }

        public DbSet<NotaFiscalParticipante> NotaFiscalParticipantes { get; set; }

        public async Task SalvarAsync()
        {
            await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotaFiscalMap).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotaFiscalParticipanteMap).Assembly);

            modelBuilder.Entity<NotaFiscal>()
            .Property(f => f.ID)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<NotaFiscalParticipante>()
            .Property(f => f.ID)
            .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
