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
        private readonly IRepositorioNotaFiscal _repositorioNotaFiscal;

        public OperadorArquivo(IRepositorioNotaFiscal repositorioNotaFiscal)
        {
            _repositorioNotaFiscal = repositorioNotaFiscal;
        }

        public async Task AdicionarAsync(string nomeArquivo, string arquivo)
        {
            //TODO: Ler arquivo
            //TODO: Ler campo a ca,po e atribuir os valores na nota fiscal

            var intercambio = new Intercambio();
            intercambio.ArquivoNome = nomeArquivo;
            //intercambio.Destinatario = AuxiliarArquivos.LerCampo(arquivo, 0, 10);
            var notaFiscal = new NotaFiscal();
            //notaFiscal.ChaveNFE = AuxiliarArquivos.LerCampo();

            await _repositorioNotaFiscal.AdicionarAsync(notaFiscal);
        }
    }
}
