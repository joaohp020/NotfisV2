using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorios
{
    public class RepositorioNotaFiscalVolume : IRepositorioNotaFiscalVolume
    {
        private readonly ContextoNotfis _contexto;

        public RepositorioNotaFiscalVolume(ContextoNotfis contexto)
        {
            _contexto = contexto;
        }

        public async Task AdicionarAsync(NotaFiscalVolume notaFiscalVolume)
        {
            await _contexto.NotasFiscaisVolumes.AddAsync(notaFiscalVolume);
            await _contexto.SalvarAsync();
        }

        public async Task AtualizarAsync(NotaFiscalVolume notaFiscalVolume)
        {
            _contexto.Entry(notaFiscalVolume).State = EntityState.Modified;
            await _contexto.SalvarAsync();
        }

        public async Task<List<NotaFiscalVolume>> ConsultarAsync()
        {
            return await _contexto.NotasFiscaisVolumes.ToListAsync();
        }

        public async Task<NotaFiscalVolume> ConsultarPorIdAsync(long id)
        {
            return await _contexto.NotasFiscaisVolumes.FirstOrDefaultAsync(n => n.ID == id);
        }

        public async Task ExcluirAsync(NotaFiscalVolume notaFiscalVolume)
        {
            _contexto.NotasFiscaisVolumes.Remove(notaFiscalVolume);

            await _contexto.SalvarAsync();
        }
    }
}
