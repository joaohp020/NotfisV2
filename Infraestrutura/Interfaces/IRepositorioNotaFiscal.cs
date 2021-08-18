using Infraestrutura.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestrutura.Interfaces
{
    public interface IRepositorioNotaFiscal
    {
        Task AdicionarAsync(NotaFiscal notaFiscal);
        Task AtualizarAsync(NotaFiscal notaFiscal);
        Task<List<NotaFiscal>> ConsultarAsync();
        Task<NotaFiscal> ConsultarPorIdAsync(long id);
        Task ExcluirAsync(NotaFiscal notaFiscal);
    }
}
