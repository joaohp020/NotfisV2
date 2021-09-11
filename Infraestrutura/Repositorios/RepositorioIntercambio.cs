using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorios
{
    public class RepositorioIntercambio : IRepositorioIntercambio
    {
        private readonly ContextoNotfis _contexto;

        public RepositorioIntercambio(ContextoNotfis contexto)
        {
            _contexto = contexto;
        }

        public async Task AdicionarAsync(Intercambio intercambio)
        {
            await _contexto.Intercambios.AddAsync(intercambio);
            await _contexto.SalvarAsync();
        }

        public async Task AtualizarAsync(Intercambio intercambio)
        {
            _contexto.Entry(intercambio).State = EntityState.Modified;
            await _contexto.SalvarAsync();
        }

        public async Task<List<Intercambio>> ConsultarAsync()
        {
            return await _contexto.Intercambios.ToListAsync();
        }

        public async Task<Intercambio> ConsultarPorIdAsync(long id)
        {
            return await _contexto.Intercambios.FirstOrDefaultAsync(n => n.ID == id);
        }

        public async Task ExcluirAsync(Intercambio intercambio)
        {
            _contexto.Intercambios.Remove(intercambio);
            
            await _contexto.SalvarAsync();
        }
    }
}
