using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorios
{
    public class RepositorioCEP : IRepositorioCEP
    {
        private readonly ContextoNotfis _contexto;

        public RepositorioCEP(ContextoNotfis contexto)
        {
            _contexto = contexto;
        }

        public async Task AdicionarAsync(CEP cep)
        {
            await _contexto.CEP.AddAsync(cep);
            await _contexto.SalvarAsync();
        }

        public async Task AtualizarAsync(CEP cep)
        {
            _contexto.Entry(cep).State = EntityState.Modified;
            await _contexto.SalvarAsync();
        }

        public async Task<List<CEP>> ConsultarAsync()
        {
            return await _contexto.CEP.ToListAsync();
        }

        public async Task<CEP> ConsultarIBGEPorCEPAsync(string cep)
        {
            return await _contexto.CEP.FirstOrDefaultAsync(n => n.CEPInicial == int.Parse(cep));
        }

        public async Task ExcluirAsync(NotaFiscal notaFiscal)
        {
            _contexto.NotasFiscais.Remove(notaFiscal);

            await _contexto.SalvarAsync();
        }
    }
}
