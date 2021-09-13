using Infraestrutura.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Interfaces
{
    public interface IRepositorioNotaFiscalParticipante
    {
        Task AdicionarAsync(NotaFiscalParticipante notaFiscalParticipante);
        Task AtualizarAsync(NotaFiscalParticipante notaFiscalParticipante);
        Task<List<NotaFiscalParticipante>> ConsultarAsync();
        Task<NotaFiscalParticipante> ConsultarPorIdAsync(long id);
        Task ExcluirAsync(NotaFiscalParticipante notaFiscalParticipante);
    }
}
