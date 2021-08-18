using Infraestrutura.Entidades;
using Infraestrutura.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Infraestrutura.Interfaces
{
    public interface IRepositorioIntercambio
    {
        public List<Entidades.Intercambio> ObterListaIntercambio(string strConnectionString, 
            DateTime datInclusaoDe, DateTime datInclusaoAte, Int32? ID_Empresa)
        {
            //using (var objDBC = new ContextoNotfis())
            //{
            //    using (var tran = objDBC.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            //    {
            //        var queQuery = objDBC.Intercambios
            //            .Where(p => p.Inclusao >= datInclusaoDe &&
            //                        p.Inclusao <= datInclusaoAte &&
            //                        (ID_Empresa == null || p.ID_Empresa == ID_Empresa)
            //            )
            //            //.Include(p => p.Status)
            //            .Include(p => p.NotasFiscais);
            //            //.Where(p => p.NotasFiscais.Any(
            //            //    nota => nota.NotaFiscalStatus.Any(status => status.ID_Status == 1000)));

            //        var result = queQuery.ToList();
            //        tran.Commit();
            //        return result;
            //    }
            //}

            return null;
        }

        public Entidades.Intercambio ObterIntercambioPorNomeArquivo(string strConnectionString, string nomeArquivo)
        {
            //using (var obj = new Contexto(strConnectionString))
            //{
            //    var intercambio = obj.Intercambios.Where(x => x.ArquivoNome.Equals(nomeArquivo)).FirstOrDefault();

            //    return intercambio;
            //}

            return null;
        }
    }
}
