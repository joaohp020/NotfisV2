using Infraestrutura.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Reflection;
using System.Threading.Tasks;
using static Infraestrutura.Entidades.Intercambio;

namespace Infraestrutura
{
    public class ContextoNotfis : DbContext
    {
        public ContextoNotfis(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }


        public DbSet<NotaFiscal> NotasFiscais { get; set; }

        public DbSet<NotaFiscalVolume> NotasFiscaisVolumes { get; set; }

        public DbSet<NotaFiscalItem> NotasFiscaisItens { get; set; }

        public DbSet<NotaFiscalParticipante> NotaFiscalParticipantes { get; set; }

        public DbSet<Intercambio> Intercambios { get; set; }

        public DbSet<CEP> CEP { get; set; }

        public async Task SalvarAsync()
        {
            await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IntercambioMap).Assembly);
        }
    }

    public interface IEntityMappingConfiguration<T> where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }

    public static class EntityMappingExtensions
    {
        public static ModelBuilder RegisterEntityMapping<TEntity, TMapping>(this ModelBuilder builder)
           where TMapping : IEntityMappingConfiguration<TEntity>
           where TEntity : class
        {
            var mapper = (IEntityMappingConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping));
            mapper.Map(builder.Entity<TEntity>());
            return builder;
        }
    }
}
