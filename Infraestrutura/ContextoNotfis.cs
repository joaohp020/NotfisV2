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

        public DbSet<Intercambio> Intercambios { get; set; }

        public async Task SalvarAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
