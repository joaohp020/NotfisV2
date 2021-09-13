using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Auxiliar
{
    public class NOTFIS //AuxiliarArquivos
    {
        private readonly IRepositorioCEP _repositorioCEP;
        private readonly IRepositorioIntercambio _repositorioIntercambio;
        private readonly IRepositorioNotaFiscal _repositorioNotaFiscal;
        private readonly IRepositorioNotaFiscalVolume _repositorioNotaFiscalVolume;

        public NOTFIS(IRepositorioCEP repositorioCEP, IRepositorioIntercambio repositorioIntercambio, IRepositorioNotaFiscal repositorioNotaFiscal, IRepositorioNotaFiscalVolume repositorioNotaFiscalVolume)
        {
            _repositorioCEP = repositorioCEP;
            _repositorioIntercambio = repositorioIntercambio;
            _repositorioNotaFiscal = repositorioNotaFiscal;
            _repositorioNotaFiscalVolume = repositorioNotaFiscalVolume;
        }

        public async Task ConverterParaIntercambioAsync(Intercambio objIntercambioMDL)
        {
            //Conversão
            var strArquivo = Encoding.GetEncoding("iso-8859-1").GetString(objIntercambioMDL.ArquivoConteudo);
            var arrLinhas = strArquivo.Split('\n');

            //Tratativa para remover linhas com erro de dados cadastrais
            var lstLinhasRemover = await this.ValidarEstruturaArquivo(arrLinhas);
            var itensArray = arrLinhas.ToList();

            var lstItens = new List<String>();

            if (lstLinhasRemover.Any())
                foreach (var itemRemover in lstLinhasRemover)
                    lstItens.Add(itensArray[int.Parse(itemRemover)]);

            foreach (var parcial in lstItens)
            {
                itensArray.Remove(parcial);

                if (!parcial.StartsWith("313")) continue;

                var strNumero = parcial.Substring(32, 8);
                var strSerie = parcial.Substring(29, 1);
                var strMensagem =
                    string.Format(
                        "NOTAFISCAL_INTEGRAR -> Não foi possível importar a nota fiscal: {0}-{1}, pois os dados da nota fiscal ou destinatário estão inválidos.",
                        strNumero, strSerie);

                //Queries.Mensagem.NovaMensagem(this.ConnectionString, objEmpresa.ID.Value, strMensagem, true, objEmpresa.Razao, null, null);
            }

            arrLinhas = itensArray.ToArray();

            for (var intIndex = 0; intIndex < arrLinhas.Count(); intIndex++)
            {
                var strLinha = arrLinhas[intIndex].Replace("\n", string.Empty).Replace("\r", string.Empty);

                /// ************************************************************************** ///
                /// *** 000 - Obter dados do intercâmbio                                   *** ///
                /// ************************************************************************** ///
                if (strLinha.StartsWith("000"))
                {
                    objIntercambioMDL.GUID = Utilitario.RecuperarCampo(strLinha, 83, 12, true, intIndex, "[000] Código do intercâmbio");

                    objIntercambioMDL.Remetente = Utilitario.RecuperarCampo(strLinha, 3, 35, true, intIndex, "[000] Remetente");

                    objIntercambioMDL.Destinatario = Utilitario.RecuperarCampo(strLinha, 38, 35, true, intIndex, "[000] Destinatário");
                }

                /// ************************************************************************** ///
                /// *** 313 - Obter dados da nota fiscal                                   *** ///
                /// ************************************************************************** ///
                if (strLinha.StartsWith("313"))
                {
                    var objNotaFiscalMDL = new NotaFiscal();

                    objNotaFiscalMDL.Inclusao = DateTime.Now;
                    objNotaFiscalMDL.Ativo = true;

                    objNotaFiscalMDL.Numero = Utilitario.RecuperarCampo(strLinha, 32, 8, true, intIndex, "[313] Número da nota fiscal");

                    objNotaFiscalMDL.Serie = Utilitario.RecuperarCampo(strLinha, 29, 3, false, intIndex, "[313] Série da nota fiscal");

                    objNotaFiscalMDL.Emissao = Utilitario.ObterDataHora(Utilitario.RecuperarCampo(strLinha, 40, 8, false, intIndex, "[313] Data da emissão da nota fiscal"), "DDMMAAAA");

                    objNotaFiscalMDL.Natureza = Utilitario.RecuperarCampo(strLinha, 48, 15, false, intIndex, "[313] Natureza");

                    objNotaFiscalMDL.Acondicionamento = Utilitario.RecuperarCampo(strLinha, 63, 15, false, intIndex, "[313] Tipo de acondicionamento");

                    objNotaFiscalMDL.CondicaoFrete = Utilitario.RecuperarCampo(strLinha, 28, 1, true, intIndex, "[313] Condição de frete");

                    objNotaFiscalMDL.PesoCubagem = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 100, 7, true, intIndex, "[313] Peso bruto."), 2);

                    objNotaFiscalMDL.Pedido = objNotaFiscalMDL.Numero + "/" + objNotaFiscalMDL.Serie;

                    objNotaFiscalMDL.Embalagens = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 78, 7, true, intIndex, "[313] Embalagens."), 2));

                    objNotaFiscalMDL.Volumes = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 78, 7, true, intIndex, "[313] Volumes."), 2));

                    objNotaFiscalMDL.PesoBruto = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 100, 7, true, intIndex, "[313] Peso bruto."), 2);

                    objNotaFiscalMDL.ValorTotal = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 85, 15, true, intIndex, "[313] Valor total"), 2);

                    objNotaFiscalMDL.ChaveNFE = Utilitario.RecuperarCampo(strLinha, 240, 44, true, intIndex, "[313] Chave da NFE");

                    objNotaFiscalMDL.CodigoNFE = "N/A";

                    //Forçar número e série pela chave se houver
                    if (!String.IsNullOrEmpty(objNotaFiscalMDL.ChaveNFE) && objNotaFiscalMDL.ChaveNFE.Length.Equals(44))
                    {
                        objNotaFiscalMDL.Serie = objNotaFiscalMDL.ChaveNFE.Substring(22, 3);
                        objNotaFiscalMDL.Numero = objNotaFiscalMDL.ChaveNFE.Substring(25, 9);
                    }

                    //Adicionar Volumes
                    for (Int32 intVolumes = 0; intVolumes < objNotaFiscalMDL.Volumes; intVolumes++)
                    {
                        var objVolume = new NotaFiscalVolume();
                        objVolume.Volume = intVolumes + 1;
                        objVolume.Chave = objNotaFiscalMDL.ChaveNFE;
                        objVolume.Pedido = objNotaFiscalMDL.Pedido;
                        objVolume.Impresso = false;
                        objVolume.Inclusao = DateTime.Now;
                        objVolume.Ativo = true;
                        objNotaFiscalMDL.NotaFiscalVolumes.Add(objVolume);
                    }


                    /// ************************************************************************** ///
                    /// *** 312 - Obter dados do destinatário                                  *** ///
                    /// ************************************************************************** ///
                    for (var intSubIndex = (intIndex - 1); intSubIndex > 0; intSubIndex--)
                    {
                        var strSubLinha = arrLinhas[intSubIndex].Replace("\n", string.Empty)
                            .Replace("\r", string.Empty);

                        if (strSubLinha.StartsWith("312"))
                        {
                            var objParticipanteMDL = new NotaFiscalParticipante();
                            objParticipanteMDL.Inclusao = DateTime.Now;
                            objParticipanteMDL.Ativo = true;
                            objParticipanteMDL.Tipo = "Destinatário";

                            objParticipanteMDL.CNPJ = Utilitario.RecuperarCampo(strSubLinha, 43, 14, true, intSubIndex, "[312] CNPJ do emitente");

                            objParticipanteMDL.Razao = Utilitario.RecuperarCampo(strSubLinha, 3, 40, false, intSubIndex, "[312] Razão social");

                            objParticipanteMDL.Logradouro = Utilitario.RecuperarCampo(strSubLinha, 72, 40, false, intSubIndex, "[312] Logradouro");

                            objParticipanteMDL.Numero = "SN";

                            objParticipanteMDL.Bairro = Utilitario.RecuperarCampo(strSubLinha, 112, 20, false, intSubIndex, "[312] Bairro");

                            objParticipanteMDL.CEP = Utilitario.RecuperarCampo(strSubLinha, 167, 9, true, intSubIndex, "[312] CEP");

                            objParticipanteMDL.Cidade = Utilitario.RecuperarCampo(strSubLinha, 132, 35, true, intSubIndex, "[312] Cidade");

                            objParticipanteMDL.Estado = Utilitario.RecuperarCampo(strSubLinha, 185, 9, true, intSubIndex, "[312] UF");

                            objParticipanteMDL.Telefone = Utilitario.RecuperarCampo(strSubLinha, 198, 35, false, intSubIndex, "[312] Telefone").Replace("-", string.Empty).Trim();

                            //Procurar IBGE
                            var strCEP = objParticipanteMDL.CEP.Replace("-", string.Empty).Trim();
                            var intIBGE = await _repositorioCEP.ConsultarIBGEPorCEPAsync(strCEP);
                            objParticipanteMDL.IBGE = intIBGE.ToString();

                            //Obter número do logradouro
                            if (!String.IsNullOrEmpty(objParticipanteMDL.Logradouro))
                            {
                                var strNumero = Utilitario.RecuperarNumeros(objParticipanteMDL.Logradouro);
                                if (!String.IsNullOrEmpty(strNumero))
                                {
                                    var strEndereco =
                                        objParticipanteMDL.Logradouro.Replace(strNumero, String.Empty);
                                    strEndereco = strEndereco.Replace(",", String.Empty).Trim();
                                    objParticipanteMDL.Logradouro = strEndereco;
                                    objParticipanteMDL.Numero = strNumero;
                                }
                            }

                            objNotaFiscalMDL.NotaFiscalParticipante.Add(objParticipanteMDL);
                            break;
                        }
                    }


                    /// ************************************************************************** ///
                    /// *** 311 - Obter dados do emitente                                      *** ///
                    /// ************************************************************************** ///
                    for (var intSubIndex = (intIndex - 1); intSubIndex > 0; intSubIndex--)
                    {
                        var strSubLinha = arrLinhas[intSubIndex].Replace("\n", string.Empty).Replace("\r", string.Empty);
                        if (strSubLinha.StartsWith("311"))
                        {
                            var objParticipanteMDL = new NotaFiscalParticipante();
                            objParticipanteMDL.Inclusao = DateTime.Now;
                            objParticipanteMDL.Ativo = true;
                            objParticipanteMDL.Tipo = "Emitente";

                            objParticipanteMDL.CNPJ = Utilitario.RecuperarCampo(strSubLinha, 3, 14, true, intSubIndex, "[311] CNPJ");

                            objParticipanteMDL.Razao = Utilitario.RecuperarCampo(strSubLinha, 133, 40, false, intSubIndex, "[311] Razão social");

                            objParticipanteMDL.Logradouro = Utilitario.RecuperarCampo(strSubLinha, 32, 40, true, intSubIndex, "[311] Logradouro");

                            objParticipanteMDL.Numero = "SN";

                            objParticipanteMDL.Bairro = "Centro";

                            objParticipanteMDL.CEP = Utilitario.RecuperarCampo(strSubLinha, 107, 9, true, intSubIndex, "[311] CEP");

                            objParticipanteMDL.Cidade = Utilitario.RecuperarCampo(strSubLinha, 72, 35, true, intSubIndex, "[311] Cidade");

                            objParticipanteMDL.Estado = Utilitario.RecuperarCampo(strSubLinha, 116, 9, true, intSubIndex, "[311] UF");

                            //Procurar IBGE
                            var strCEP = objParticipanteMDL.CEP.Replace("-", string.Empty).Trim();
                            var intIBGE = await _repositorioCEP.ConsultarIBGEPorCEPAsync(strCEP);
                            objParticipanteMDL.IBGE = intIBGE.ToString();

                            //Obter número do logradouro
                            if (!String.IsNullOrEmpty(objParticipanteMDL.Logradouro))
                            {
                                var strNumero = Utilitario.RecuperarNumeros(objParticipanteMDL.Logradouro);
                                if (!String.IsNullOrEmpty(strNumero))
                                {
                                    var strEndereco = objParticipanteMDL.Logradouro.Replace(strNumero, String.Empty);
                                    strEndereco = strEndereco.Replace(",", String.Empty).Trim();
                                    objParticipanteMDL.Logradouro = strEndereco;
                                    objParticipanteMDL.Numero = strNumero;
                                }
                            }

                            objNotaFiscalMDL.NotaFiscalParticipante.Add(objParticipanteMDL);
                            break;
                        }
                    }

                    if (!objIntercambioMDL.NotasFiscais.Any(p => p.ChaveNFE == objNotaFiscalMDL.ChaveNFE))
                        objIntercambioMDL.NotasFiscais.Add(objNotaFiscalMDL);
                }


                /// ************************************************************************** ///
                /// *** 314 - Obter Itens da nota fiscal                                   *** ///
                /// ************************************************************************** ///
                if (strLinha.StartsWith("314"))
                {
                    var objNotaFiscalMDL = objIntercambioMDL.NotasFiscais.Last();
                    if (objNotaFiscalMDL != null)
                    {
                        var objItemMDL = new NotaFiscalItem();
                        objItemMDL.Inclusao = DateTime.Now;
                        objItemMDL.Ativo = true;

                        objItemMDL.Codigo = Utilitario.RecuperarCampo(strLinha, 25, 30, false, intIndex, "[314] Código/Descrição");

                        objItemMDL.Volumes = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 3, 7, true, intIndex, "[314] Qtde. de volumes do produto"), 2));

                        objItemMDL.Acondicionamento = Utilitario.RecuperarCampo(strLinha, 10, 15, false, intIndex, "[314] Acondicionamento do produto ");

                        objItemMDL.Descricao = objItemMDL.Codigo;

                        objNotaFiscalMDL.NotaFiscalItens.Add(objItemMDL);
                    }
                }
            }
        }

        private async Task<List<string>> ValidarEstruturaArquivo(IReadOnlyList<string> arrLinhas)
        {
            if (arrLinhas == null) throw new ArgumentNullException("arrLinhas");

            var lstLinhasRemovidas = new List<string>();

            for (var intIndex = 0; intIndex < arrLinhas.Count(); intIndex++)
            {
                var intIdentificador = 0;

                var strLinha = arrLinhas[intIndex].Replace("\n", string.Empty).Replace("\r", string.Empty);

                try
                {
                    /// ************************************************************************** ///
                    /// *** 000 - Obter dados do intercâmbio                                   *** ///
                    /// ************************************************************************** ///
                    if (strLinha.StartsWith("000"))
                    {
                        intIdentificador = int.Parse(strLinha.Substring(0, 3));

                        Utilitario.RecuperarCampo(strLinha, 83, 12, true, intIndex, "[000] Código do intercâmbio");

                        Utilitario.RecuperarCampo(strLinha, 3, 35, true, intIndex, "[000] Remetente");

                        Utilitario.RecuperarCampo(strLinha, 38, 35, true, intIndex, "[000] Destinatário");
                    }


                    /// ************************************************************************** ///
                    /// *** 313 - Obter dados da nota fiscal                                   *** ///
                    /// ************************************************************************** ///
                    if (strLinha.StartsWith("313"))
                    {
                        intIdentificador = int.Parse(strLinha.Substring(0, 3));

                        var objNotaFiscalMDL = new Infraestrutura.Entidades.NotaFiscal();

                        objNotaFiscalMDL.Inclusao = DateTime.Now;
                        objNotaFiscalMDL.Ativo = true;

                        objNotaFiscalMDL.Numero = Utilitario.RecuperarCampo(strLinha, 32, 8, true, intIndex, "[313] Número da nota fiscal");

                        objNotaFiscalMDL.Serie = Utilitario.RecuperarCampo(strLinha, 29, 3, true, intIndex, "[313] Série da nota fiscal");

                        objNotaFiscalMDL.Emissao = Utilitario.ObterDataHora(Utilitario.RecuperarCampo(strLinha, 40, 8, true, intIndex, "[313] Data da emissão da nota fiscal"), "DDMMAAAA");

                        objNotaFiscalMDL.Natureza = Utilitario.RecuperarCampo(strLinha, 48, 15, false, intIndex, "[313] Natureza");

                        objNotaFiscalMDL.Acondicionamento = Utilitario.RecuperarCampo(strLinha, 63, 15, false, intIndex, "[313] Tipo de acondicionamento");

                        objNotaFiscalMDL.CondicaoFrete = Utilitario.RecuperarCampo(strLinha, 28, 1, true, intIndex, "[313] Condição de frete");

                        objNotaFiscalMDL.Pedido = objNotaFiscalMDL.Numero + "/" + objNotaFiscalMDL.Serie;

                        objNotaFiscalMDL.Embalagens = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 78, 7, true, intIndex, "[313] Embalagens."), 2));

                        objNotaFiscalMDL.Volumes = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 78, 7, true, intIndex, "[313] Volumes."), 2));

                        objNotaFiscalMDL.PesoBruto = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 100, 7, true, intIndex, "[313] Peso bruto."), 2);

                        objNotaFiscalMDL.ValorTotal = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 85, 15, true, intIndex, "[313] Valor total"), 2);

                        objNotaFiscalMDL.ChaveNFE = Utilitario.RecuperarCampo(strLinha, 238, 44, true, intIndex, "[313] Chave da NFE");

                        objNotaFiscalMDL.CodigoNFE = Utilitario.RecuperarCampo(strLinha, 238, 8, true, intIndex, "[313] Código da chave da NFE");

                        objNotaFiscalMDL.CodigoNFE = "N/A";

                        //Forçar número e série pela chave se houver
                        if (!String.IsNullOrEmpty(objNotaFiscalMDL.ChaveNFE) &&
                            objNotaFiscalMDL.ChaveNFE.Length.Equals(44))
                        {
                            objNotaFiscalMDL.Serie = objNotaFiscalMDL.ChaveNFE.Substring(22, 3);
                            objNotaFiscalMDL.Numero = objNotaFiscalMDL.ChaveNFE.Substring(25, 9);
                        }

                        //Adicionar Volumes
                        for (Int32 intVolumes = 0; intVolumes < objNotaFiscalMDL.Volumes; intVolumes++)
                        {
                            var objVolume = new NotaFiscalVolume();
                            objVolume.Volume = intVolumes + 1;
                            objVolume.Chave = objNotaFiscalMDL.ChaveNFE;
                            objVolume.Pedido = objNotaFiscalMDL.Pedido;
                            objVolume.Impresso = false;
                            objVolume.Inclusao = DateTime.Now;
                            objVolume.Ativo = true;
                            objNotaFiscalMDL.NotaFiscalVolumes.Add(objVolume);
                        }


                        /// ************************************************************************** ///
                        /// *** 312 - Obter dados do destinatário                                  *** ///
                        /// ************************************************************************** ///
                        for (var intSubIndex = (intIndex - 1); intSubIndex > 0; intSubIndex--)
                        {
                            var strSubLinha = arrLinhas[intSubIndex].Replace("\n", string.Empty)
                                .Replace("\r", string.Empty);

                            intIdentificador = int.Parse(strSubLinha.Substring(0, 3));

                            if (strSubLinha.StartsWith("312"))
                            {
                                var objParticipanteMDL = new NotaFiscalParticipante();
                                objParticipanteMDL.Inclusao = DateTime.Now;
                                objParticipanteMDL.Ativo = true;
                                objParticipanteMDL.Tipo = "Destinatário";

                                objParticipanteMDL.CNPJ = Utilitario.RecuperarCampo(strSubLinha, 43, 14, true, intSubIndex, "[312] CNPJ do emitente");

                                objParticipanteMDL.Razao = Utilitario.RecuperarCampo(strSubLinha, 3, 40, true, intSubIndex, "[312] Razão social");

                                objParticipanteMDL.Logradouro = Utilitario.RecuperarCampo(strSubLinha, 72, 40, true, intSubIndex, "[312] Logradouro");

                                objParticipanteMDL.Numero = "SN";

                                objParticipanteMDL.Bairro = Utilitario.RecuperarCampo(strSubLinha, 112, 20, true, intSubIndex,"[312] Bairro");

                                objParticipanteMDL.CEP = Utilitario.RecuperarCampo(strSubLinha, 167, 9, true, intSubIndex, "[312] CEP");

                                objParticipanteMDL.Cidade = Utilitario.RecuperarCampo(strSubLinha, 132, 35, true, intSubIndex, "[312] Cidade");

                                objParticipanteMDL.Estado = Utilitario.RecuperarCampo(strSubLinha, 185, 9, true, intSubIndex, "[312] UF");

                                //Procurar IBGE
                                var strCEP = objParticipanteMDL.CEP.Replace("-", string.Empty).Trim();
                                var intIBGE = await _repositorioCEP.ConsultarIBGEPorCEPAsync(strCEP);
                                objParticipanteMDL.IBGE = intIBGE.ToString();

                                //Obter número do logradouro
                                if (!String.IsNullOrEmpty(objParticipanteMDL.Logradouro))
                                {
                                    var strNumero = Utilitario.RecuperarNumeros(objParticipanteMDL.Logradouro);
                                    if (!String.IsNullOrEmpty(strNumero))
                                    {
                                        var strEndereco =
                                            objParticipanteMDL.Logradouro.Replace(strNumero, String.Empty);
                                        strEndereco = strEndereco.Replace(",", String.Empty).Trim();
                                        objParticipanteMDL.Logradouro = strEndereco;
                                        objParticipanteMDL.Numero = strNumero;
                                    }
                                }
                                else
                                {
                                    objNotaFiscalMDL.NotaFiscalParticipante.Add(objParticipanteMDL);
                                    break;
                                }
                            }
                        }


                        /// ************************************************************************** ///
                        /// *** 311 - Obter dados do emitente                                      *** ///
                        /// ************************************************************************** ///
                        for (var intSubIndex = (intIndex - 1); intSubIndex > 0; intSubIndex--)
                        {
                            var strSubLinha = arrLinhas[intSubIndex].Replace("\n", string.Empty)
                                .Replace("\r", string.Empty);

                            intIdentificador = int.Parse(strSubLinha.Substring(0, 3));

                            if (strSubLinha.StartsWith("311"))
                            {
                                var objParticipanteMDL = new NotaFiscalParticipante();
                                objParticipanteMDL.Inclusao = DateTime.Now;
                                objParticipanteMDL.Ativo = true;
                                objParticipanteMDL.Tipo = "Emitente";

                                objParticipanteMDL.CNPJ = Utilitario.RecuperarCampo(strSubLinha, 3, 14, true, intSubIndex, "[311] CNPJ");

                                objParticipanteMDL.Razao = Utilitario.RecuperarCampo(strSubLinha, 133, 40, true, intSubIndex, "[311] Razão social");

                                objParticipanteMDL.Logradouro = Utilitario.RecuperarCampo(strSubLinha, 32, 40, true, intSubIndex, "[311] Logradouro");

                                objParticipanteMDL.Numero = "SN";

                                objParticipanteMDL.Bairro = "Centro";

                                objParticipanteMDL.CEP = Utilitario.RecuperarCampo(strSubLinha, 107, 9, true, intSubIndex, "[311] CEP");

                                objParticipanteMDL.Cidade = Utilitario.RecuperarCampo(strSubLinha, 72, 35, true, intSubIndex, "[311] Cidade");

                                objParticipanteMDL.Estado = Utilitario.RecuperarCampo(strSubLinha, 116, 9, true, intSubIndex, "[311] UF");

                                //Procurar IBGE
                                var strCEP = objParticipanteMDL.CEP.Replace("-", string.Empty).Trim();
                                var intIBGE = await _repositorioCEP.ConsultarIBGEPorCEPAsync(strCEP);
                                objParticipanteMDL.IBGE = intIBGE.ToString();

                                //Obter número do logradouro
                                if (!String.IsNullOrEmpty(objParticipanteMDL.Logradouro))
                                {
                                    var strNumero = Utilitario.RecuperarNumeros(objParticipanteMDL.Logradouro);
                                    if (!String.IsNullOrEmpty(strNumero))
                                    {
                                        var strEndereco =
                                            objParticipanteMDL.Logradouro.Replace(strNumero, String.Empty);
                                        strEndereco = strEndereco.Replace(",", String.Empty).Trim();
                                        objParticipanteMDL.Logradouro = strEndereco;
                                        objParticipanteMDL.Numero = strNumero;
                                    }
                                }

                                objNotaFiscalMDL.NotaFiscalParticipante.Add(objParticipanteMDL);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    switch (intIdentificador)
                    {
                        case 312:
                            var strRegistroNota = intIndex;
                            lstLinhasRemovidas.Add((intIndex - 1).ToString());
                            lstLinhasRemovidas.Add(strRegistroNota.ToString());
                            break;
                        case 313:
                            var strRegistroDestinatario = intIndex - 1;
                            lstLinhasRemovidas.Add(strRegistroDestinatario.ToString());
                            break;
                    }
                }
            }
            return lstLinhasRemovidas;
        }
    }
}
