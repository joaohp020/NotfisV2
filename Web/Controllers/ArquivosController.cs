using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Text;
using System.Web;
using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using Web.Models.Arquivos;
using Servicos.Interfaces;
using Infraestrutura.Repositorios;

namespace Web.Controllers
{
    public class ArquivosController : Controller
    {
        private readonly IOperadorArquivo _operadorArquivo;

        public ArquivosController(IOperadorArquivo operadorArquivo)
        {
            _operadorArquivo = operadorArquivo;


        }

        // GET: ArquivosController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ArquivosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ArquivosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArquivosController/Create
        [HttpPost("SalvarArquivo")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarArquivoAsync(ArquivoModel arquivoModel)
        {
            try
            {
                foreach (var arquivo in arquivoModel.Arquivos)
                {
                    if (arquivo.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            arquivo.CopyTo(ms);

                            var conteudo = Encoding.UTF8.GetString(ms.ToArray());
                            await _operadorArquivo.AdicionarAsync(arquivo.Name, conteudo);

                            return Ok(conteudo);
                        }
                    }
                }

                return Ok();
            }
            catch
            {
                return View();
            }
        }

        // GET: ArquivosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ArquivosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArquivosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ArquivosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
