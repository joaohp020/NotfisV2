using Infraestrutura.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Interfaces
{
    public interface IRepositorioNotaFiscalVolume
    {
        Task AdicionarAsync(NotaFiscalVolume notaFiscalVolume);
        Task AtualizarAsync(NotaFiscalVolume notaFiscalVolume);
        Task<List<NotaFiscalVolume>> ConsultarAsync();
        Task<NotaFiscalVolume> ConsultarPorIdAsync(long id);
        Task ExcluirAsync(NotaFiscalVolume notaFiscalVolume);
    }
}
