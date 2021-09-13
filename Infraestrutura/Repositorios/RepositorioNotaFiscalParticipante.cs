using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorios
{
    public class RepositorioNotaFiscalParticipante : IRepositorioNotaFiscalParticipante
    {
        private readonly ContextoNotfis _contexto;

        public RepositorioNotaFiscalParticipante(ContextoNotfis contexto)
        {
            _contexto = contexto;
        }

        public async Task AdicionarAsync(NotaFiscalParticipante notaFiscalParticipante)
        {
            await _contexto.NotaFiscalParticipantes.AddAsync(notaFiscalParticipante);
            await _contexto.SalvarAsync();
        }

        public async Task AtualizarAsync(NotaFiscalParticipante notaFiscalParticipante)
        {
            _contexto.Entry(notaFiscalParticipante).State = EntityState.Modified;
            await _contexto.SalvarAsync();
        }

        public async Task<List<NotaFiscalParticipante>> ConsultarAsync()
        {
            return await _contexto.NotaFiscalParticipantes.ToListAsync();
        }

        public async Task<NotaFiscalParticipante> ConsultarPorIdAsync(long id)
        {
            return await _contexto.NotaFiscalParticipantes.FirstOrDefaultAsync(n => n.ID == id);
        }

        public async Task ExcluirAsync(NotaFiscalParticipante notaFiscalParticipante)
        {
            _contexto.NotaFiscalParticipantes.Remove(notaFiscalParticipante);
            
            await _contexto.SalvarAsync();
        }
    }
}
