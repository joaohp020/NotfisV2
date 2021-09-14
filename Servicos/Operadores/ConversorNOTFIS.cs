using Infraestrutura.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Auxiliar
{
    public class ConversorNOTFIS
    {
        public List<NotaFiscal> ConverterParaNotasFiscais(string arquivo)
        {
            var notasfiscais = new List<NotaFiscal>();

            var linhas = arquivo.Split('\n');

            for (var index = 0; index < linhas.Count(); index++)
            {
                var linha = linhas[index].Replace("\n", string.Empty).Replace("\r", string.Empty);

                if (!linha.StartsWith("313"))
                    continue;

                var notafiscal = ObterDadosNotaFiscal(linhas, index, linha);
                if (notafiscal == null)
                    continue;

                notasfiscais.Add(notafiscal);
            }

            return notasfiscais;
        }

        private NotaFiscal ObterDadosNotaFiscal(string[] linhas, int intIndex, string strLinha)
        {
            try
            {
                var notafiscal = new NotaFiscal();

                notafiscal.Inclusao = DateTime.Now;
                notafiscal.Ativo = true;

                notafiscal.Numero = Utilitario.RecuperarCampo(strLinha, 32, 8, true, intIndex, "[313] Número da nota fiscal");
                notafiscal.Serie = Utilitario.RecuperarCampo(strLinha, 29, 3, false, intIndex, "[313] Série da nota fiscal");
                notafiscal.Emissao = Utilitario.ObterDataHora(Utilitario.RecuperarCampo(strLinha, 40, 8, false, intIndex, "[313] Data da emissão da nota fiscal"), "DDMMAAAA");
                notafiscal.Natureza = Utilitario.RecuperarCampo(strLinha, 48, 15, false, intIndex, "[313] Natureza");
                notafiscal.Acondicionamento = Utilitario.RecuperarCampo(strLinha, 63, 15, false, intIndex, "[313] Tipo de acondicionamento");
                notafiscal.CondicaoFrete = Utilitario.RecuperarCampo(strLinha, 28, 1, true, intIndex, "[313] Condição de frete");
                notafiscal.PesoCubagem = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 100, 7, true, intIndex, "[313] Peso bruto."), 2);
                notafiscal.Pedido = notafiscal.Numero + "/" + notafiscal.Serie;
                notafiscal.Embalagens = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 78, 7, true, intIndex, "[313] Embalagens."), 2));
                notafiscal.Volumes = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 78, 7, true, intIndex, "[313] Volumes."), 2));
                notafiscal.PesoBruto = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 100, 7, true, intIndex, "[313] Peso bruto."), 2);
                notafiscal.ValorTotal = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(strLinha, 85, 15, true, intIndex, "[313] Valor total"), 2);
                notafiscal.ChaveNFE = Utilitario.RecuperarCampo(strLinha, 240, 44, true, intIndex, "[313] Chave da NFE");
                notafiscal.CodigoNFE = "N/A";

                //Forçar número e série pela chave se houver
                if (!string.IsNullOrEmpty(notafiscal.ChaveNFE) && notafiscal.ChaveNFE.Length.Equals(44))
                {
                    notafiscal.Serie = notafiscal.ChaveNFE.Substring(22, 3);
                    notafiscal.Numero = notafiscal.ChaveNFE.Substring(25, 9);
                }

                ObterDadosDestinatario(linhas, intIndex, notafiscal);
                ObterDadosEmitente(linhas, intIndex, notafiscal);

                return notafiscal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void ObterDadosEmitente(string[] linhas, int index, NotaFiscal notafiscal)
        {
            for (var subIndex = (index - 1); subIndex > 0; subIndex--)
            {
                var subLinha = linhas[subIndex].Replace("\n", string.Empty).Replace("\r", string.Empty);
                if (!subLinha.StartsWith("311"))
                    continue;

                var participante = new NotaFiscalParticipante();
                participante.Inclusao = DateTime.Now;
                participante.Ativo = true;
                participante.Tipo = "Emitente";

                participante.CNPJ = Utilitario.RecuperarCampo(subLinha, 3, 14, true, subIndex, "[311] CNPJ");
                participante.Razao = Utilitario.RecuperarCampo(subLinha, 133, 40, false, subIndex, "[311] Razão social");
                participante.Logradouro = Utilitario.RecuperarCampo(subLinha, 32, 40, true, subIndex, "[311] Logradouro");
                participante.Numero = "SN";
                participante.Bairro = "Centro";
                participante.CEP = Utilitario.RecuperarCampo(subLinha, 107, 9, true, subIndex, "[311] CEP");
                participante.Cidade = Utilitario.RecuperarCampo(subLinha, 72, 35, true, subIndex, "[311] Cidade");
                participante.Estado = Utilitario.RecuperarCampo(subLinha, 116, 9, true, subIndex, "[311] UF");


                //Obter número do logradouro
                if (!string.IsNullOrEmpty(participante.Logradouro))
                {
                    var numero = Utilitario.RecuperarNumeros(participante.Logradouro);
                    if (!string.IsNullOrEmpty(numero))
                    {
                        var endereco = participante.Logradouro.Replace(numero, string.Empty);
                        endereco = endereco.Replace(",", string.Empty).Trim();
                        participante.Logradouro = endereco;
                        participante.Numero = numero;
                    }
                }

                notafiscal.NotaFiscalParticipante.Add(participante);
            }
        }

        private void ObterDadosDestinatario(string[] linhas, int intIndex, NotaFiscal notafiscal)
        {
            for (var intSubIndex = (intIndex - 1); intSubIndex > 0; intSubIndex--)
            {
                var subLinha = linhas[intSubIndex].Replace("\n", string.Empty)
                    .Replace("\r", string.Empty);

                if (!subLinha.StartsWith("312"))
                    continue;

                var participante = new NotaFiscalParticipante();
                participante.Inclusao = DateTime.Now;
                participante.Ativo = true;
                participante.Tipo = "Destinatário";
                participante.CNPJ = Utilitario.RecuperarCampo(subLinha, 43, 14, true, intSubIndex, "[312] CNPJ do emitente");
                participante.Razao = Utilitario.RecuperarCampo(subLinha, 3, 40, false, intSubIndex, "[312] Razão social");
                participante.Logradouro = Utilitario.RecuperarCampo(subLinha, 72, 40, false, intSubIndex, "[312] Logradouro");
                participante.Numero = "SN";
                participante.Bairro = Utilitario.RecuperarCampo(subLinha, 112, 20, false, intSubIndex, "[312] Bairro");
                participante.CEP = Utilitario.RecuperarCampo(subLinha, 167, 9, true, intSubIndex, "[312] CEP");
                participante.Cidade = Utilitario.RecuperarCampo(subLinha, 132, 35, true, intSubIndex, "[312] Cidade");
                participante.Estado = Utilitario.RecuperarCampo(subLinha, 185, 9, true, intSubIndex, "[312] UF");
                participante.Telefone = Utilitario.RecuperarCampo(subLinha, 198, 35, false, intSubIndex, "[312] Telefone").Replace("-", string.Empty).Trim();

                //Obter número do logradouro
                if (!string.IsNullOrEmpty(participante.Logradouro))
                {
                    var numero = Utilitario.RecuperarNumeros(participante.Logradouro);
                    if (!string.IsNullOrEmpty(numero))
                    {
                        var endereco = participante.Logradouro.Replace(numero, string.Empty).Replace(",", string.Empty).Trim();
                        participante.Logradouro = endereco;
                        participante.Numero = numero;
                    }
                }

                notafiscal.NotaFiscalParticipante.Add(participante);
            }
        }
    }
}
