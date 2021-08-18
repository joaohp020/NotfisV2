using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Web.Models.Arquivos
{
    public class ArquivoModel
    {
        public int IdEmpresa { get; set; }
        public List<IFormFile> Arquivos { get; set; }
    }
}
