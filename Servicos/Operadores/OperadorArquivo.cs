using Core.Auxiliar;
using Infraestrutura.Interfaces;
using Servicos.Interfaces;
using System.Threading.Tasks;

namespace Servicos.Operadores
{
    public class OperadorArquivo : IOperadorArquivo
    {
        private readonly IRepositorioNotaFiscal _repositorioNotaFiscal;
        private readonly ConversorNOTFIS _notfis;

        public OperadorArquivo(IRepositorioNotaFiscal repositorioNotaFiscal, ConversorNOTFIS notfis)
        {
            _repositorioNotaFiscal = repositorioNotaFiscal;
            _notfis = notfis;
        }

        public async Task AdicionarAsync(string arquivo)
        {
            var notasfiscais = _notfis.ConverterParaNotasFiscais(arquivo);

            foreach (var notafiscal in notasfiscais)
                await _repositorioNotaFiscal.AdicionarAsync(notafiscal);
        }
    }
}
