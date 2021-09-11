using Infraestrutura.Entidades;
using Infraestrutura.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Interfaces
{
    public interface IRepositorioIntercambio
    {
        Task AdicionarAsync(Intercambio intercambio);
        Task AtualizarAsync(Intercambio intercambio);
        Task<List<Intercambio>> ConsultarAsync();
        Task<Intercambio> ConsultarPorIdAsync(long id);
        Task ExcluirAsync(Intercambio intercambio);
    }
}
