using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorios
{
    public class RepositorioNotaFiscal : IRepositorioNotaFiscal
    {
        private readonly ContextoNotfis _contexto;

        public RepositorioNotaFiscal(ContextoNotfis contexto)
        {
            _contexto = contexto;
        }

        public async Task AdicionarAsync(NotaFiscal notaFiscal)
        {
            await _contexto.NotasFiscais.AddAsync(notaFiscal);
            await _contexto.SalvarAsync();
        }

        public async Task AtualizarAsync(NotaFiscal notaFiscal)
        {
            _contexto.Entry(notaFiscal).State = EntityState.Modified;
            await _contexto.SalvarAsync();
        }

        public async Task<List<NotaFiscal>> ConsultarAsync()
        {
            return await _contexto.NotasFiscais.ToListAsync();
        }

        public async Task<NotaFiscal> ConsultarPorIdAsync(long id)
        {
            return await _contexto.NotasFiscais.FirstOrDefaultAsync(n => n.ID == id);
        }

        public async Task ExcluirAsync(NotaFiscal notaFiscal)
        {
            _contexto.NotasFiscais.Remove(notaFiscal);
            
            await _contexto.SalvarAsync();
        }
    }
}
