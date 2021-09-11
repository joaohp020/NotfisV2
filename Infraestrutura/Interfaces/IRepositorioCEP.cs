using Infraestrutura.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Interfaces
{
    public interface IRepositorioCEP
    {
        Task AdicionarAsync(CEP cep);
        Task<CEP> ConsultarIBGEPorCEPAsync(string cep);
    }
}
