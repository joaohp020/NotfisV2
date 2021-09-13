using Core.Auxiliar;
using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using Servicos.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.Operadores
{
    public class OperadorArquivo : IOperadorArquivo
    {
        private readonly IRepositorioIntercambio _repositorioIntercambio;
        private readonly NOTFIS _notfis;

        public OperadorArquivo(IRepositorioIntercambio repositorioIntercambio, NOTFIS notfis)
        {
            _repositorioIntercambio = repositorioIntercambio;
            _notfis = notfis;
        }

        public async Task AdicionarAsync(string nomeArquivo, string arquivo)
        {
            try
            {
                var intercambio = await _notfis.ConverterParaIntercambioAsync(arquivo);

                await _repositorioIntercambio.AdicionarAsync(intercambio);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
