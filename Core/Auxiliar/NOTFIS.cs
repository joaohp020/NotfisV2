using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Auxiliar
{
    /// <summary>
    /// Classe conversora do arquivo NOTFIS
    /// </summary>
    public class NOTFIS
    {
        private readonly IRepositorioCEP _repositorioCEP;

        public NOTFIS(IRepositorioCEP repositorioCEP)
        {
            _repositorioCEP = repositorioCEP;
        }

        public async Task<Intercambio> ConverterParaIntercambioAsync(string arquivo)
        {
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            var intercambio = new Intercambio();

            var linhas = arquivo.Split('\n');

            for (var idx = 0; idx < linhas.Count(); idx++)
            {
                var linha = linhas[idx].Trim();

                if (linha.StartsWith("000"))
                    ObterIntercambio(intercambio, linha, idx);

                if (linha.StartsWith("313"))
                {
                    var notaFiscal = await ObterDadosNotaFiscalAsync(linha, linhas, idx);
                    if (notaFiscal != null)
                        intercambio.NotasFiscais.Add(notaFiscal);
                }

                if (linha.StartsWith("314"))
                    if (intercambio.NotasFiscais.Count > 0)
                        ObterItensNotaFiscal(intercambio, linha, idx);
            }

            return intercambio;
        }

        private void ObterIntercambio(Intercambio intercambio, string linha, int posicao)
        {
            intercambio.GUID = Utilitario.RecuperarCampo(linha, 83, 12, true, posicao, "[000] Código do intercâmbio");
            intercambio.Remetente = Utilitario.RecuperarCampo(linha, 3, 35, true, posicao, "[000] Remetente");
            intercambio.Destinatario = Utilitario.RecuperarCampo(linha, 38, 35, true, posicao, "[000] Destinatário");
        }

        private async Task<NotaFiscal> ObterDadosNotaFiscalAsync(string linha, string[] linhas, int posicao)
        {
            var notaFiscal = new NotaFiscal();
            notaFiscal.Inclusao = DateTime.Now;
            notaFiscal.Ativo = true;
            notaFiscal.Romaneio = Utilitario.RecuperarCampo(linha, 3, 15, false, posicao, "[313] Número de Romaneio.");
            notaFiscal.CondicaoFrete = Utilitario.RecuperarCampo(linha, 28, 1, true, posicao, "[313] Condição de frete");
            notaFiscal.Serie = Utilitario.RecuperarCampo(linha, 29, 3, true, posicao, "[313] Série da nota fiscal");
            notaFiscal.Numero = Utilitario.RecuperarCampo(linha, 32, 8, true, posicao, "[313] Número da nota fiscal");
            notaFiscal.Emissao = Utilitario.ObterDataHora(Utilitario.RecuperarCampo(linha, 40, 8, true, posicao, "[313] Data da emissão da nota fiscal"), "DDMMAAAA");
            notaFiscal.Natureza = Utilitario.RecuperarCampo(linha, 48, 15, false, posicao, "[313] Natureza");
            notaFiscal.Acondicionamento = Utilitario.RecuperarCampo(linha, 63, 15, false, posicao, "[313] Acondicionamento");
            notaFiscal.Embalagens = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(linha, 78, 7, true, posicao, "[313] Embalagens."), 2));
            notaFiscal.ValorTotal = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(linha, 85, 15, true, posicao, "[313] Valor total"), 2);
            notaFiscal.PesoBruto = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(linha, 100, 7, true, posicao, "[313] Peso bruto."), 2);
            notaFiscal.PesoCubagem = Utilitario.ObterDecimal(Utilitario.RecuperarCampo(linha, 107, 5, false, posicao, "[313] Peso Cubagem."), 2);

            var chavePedido = GerenciarChavePedido(notaFiscal, linhas, posicao);
            if (!chavePedido)
                return null;

            ObterVolumesNotaFiscal(notaFiscal);

            await ObterDadosDestinatarioNotaFiscalAsync(notaFiscal, linhas, posicao);
            await ObterDadosEmitenteNotaFiscalAsync(notaFiscal, linhas, posicao);

            return notaFiscal;
        }

        private bool GerenciarChavePedido(NotaFiscal notaFiscal, string[] linhas, int posicao)
        {
            ObterChaveNotaFiscal(notaFiscal, linhas, posicao);

            if (string.IsNullOrEmpty(notaFiscal.ChaveNFE))
                return false;

            if (notaFiscal.ChaveNFE.Length.Equals(44))
                ObterNumeroSerieNotaPelaChave(notaFiscal);

            return true;
        }

        private void ObterItensNotaFiscal(Intercambio intercambio, string linha, int posicao)
        {
            var notaFiscal = intercambio?.NotasFiscais.Last();
            if (notaFiscal != null)
            {
                var itemNota = new NotaFiscalItem
                {
                    Inclusao = DateTime.Now,
                    Ativo = true,
                    Codigo = Utilitario.RecuperarCampo(linha, 25, 30, false, posicao, "[314] Código/Descrição"),
                    Volumes = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(linha, 3, 7, false, posicao, "[314] Qtde. de volumes do produto"), 2)),
                    Acondicionamento = Utilitario.RecuperarCampo(linha, 10, 15, false, posicao, "[314] Acondicionamento do produto ")
                };

                itemNota.Descricao = itemNota.Codigo;
                notaFiscal.NotaFiscalItens.Add(itemNota);
            }
        }

        private void ObterVolumesNotaFiscal(NotaFiscal notaFiscal, string linha, int posicao)
        {
            notaFiscal.Volumes = Convert.ToInt32(Utilitario.ObterDecimal(Utilitario.RecuperarCampo(linha, 78, 7, true, posicao, "[313] Volumes."), 2));

            int idx = 0;

            while (idx < notaFiscal.Volumes)
            {
                var volume = new NotaFiscalVolume();
                volume.Volume = ++idx;
                volume.Chave = notaFiscal.ChaveNFE;
                volume.Pedido = notaFiscal.Pedido;
                volume.Impresso = false;
                volume.Inclusao = DateTime.Now;
                volume.Ativo = true;

                notaFiscal.NotaFiscalVolumes.Add(volume);
            }
        }

        private void ObterVolumesNotaFiscal(NotaFiscal notaFiscal)
        {
            for (int idx = 0; idx < notaFiscal.Volumes; idx++)
            {
                var volume = new NotaFiscalVolume();
                volume.Volume = idx + 1;
                volume.Chave = notaFiscal.ChaveNFE;
                volume.Pedido = notaFiscal.Pedido;
                volume.Impresso = false;
                volume.Inclusao = DateTime.Now;
                volume.Ativo = true;

                notaFiscal.NotaFiscalVolumes.Add(volume);
            }
        }

        private void ObterChaveNotaFiscal(NotaFiscal notaFiscal, string[] linhas, int posicao)
        {
            var linhaChaveNfe = linhas[++posicao].Replace("\n", string.Empty).Replace("\r", string.Empty);

            if (linhaChaveNfe.StartsWith("319"))
            {
                var chaveNfe = Utilitario.RecuperarCampo(linhaChaveNfe, 129, 111, false, posicao, "[319] ChaveNFE.");

                if (!string.IsNullOrEmpty(chaveNfe))
                {
                    chaveNfe = Regex.Replace(chaveNfe, @"[^\d]", "");
                    if (!string.IsNullOrEmpty(chaveNfe))
                        notaFiscal.ChaveNFE = chaveNfe;
                    else
                        return;
                }
            }
        }

        private void ObterPedidoNotaFiscal(NotaFiscal notaFiscal, string[] linhas, int posicao)
        {
            var linhaPedido = linhas[++posicao].Replace("\n", string.Empty).Replace("\r", string.Empty);

            if (linhaPedido.StartsWith("319"))
            {
                var pedido = Utilitario.RecuperarCampo(linhaPedido, 65, 10, false, posicao, "[319] Pedido.");

                if (!string.IsNullOrEmpty(pedido))
                {
                    notaFiscal.ChaveNFE = pedido;
                    notaFiscal.Pedido = pedido;
                }
            }
        }

        private void ObterNumeroSerieNotaPelaChave(NotaFiscal notaFiscal)
        {
            notaFiscal.Serie = notaFiscal.ChaveNFE.Substring(22, 3);
            notaFiscal.Numero = notaFiscal.ChaveNFE.Substring(25, 9);
        }

        private async Task ObterDadosDestinatarioNotaFiscalAsync(NotaFiscal notaFiscal, string[] linhas, int posicao)
        {
            for (var idx = posicao - 1; idx > 0; idx--)
            {
                var linha = linhas[idx].Trim();

                if (!linha.StartsWith("312"))
                    continue;

                var linhaEmail = linhas[idx + 1].Trim();

                var participante = new NotaFiscalParticipante
                {
                    Inclusao = DateTime.Now,
                    Ativo = true,
                    Tipo = "Destinatário",
                    CNPJ = Utilitario.RecuperarCampo(linha, 43, 14, true, idx, "[312] CNPJ do emitente"),
                    Razao = Utilitario.RecuperarCampo(linha, 3, 40, true, idx, "[312] Razão social"),
                    IE = Utilitario.RecuperarCampo(linha, 57, 15, false, idx, "[312] Inscrição Estadual"),
                    Logradouro = Utilitario.RecuperarCampo(linha, 72, 40, true, idx, "[312] Logradouro"),
                    Numero = "SN",
                    Bairro = Utilitario.RecuperarCampo(linha, 112, 20, true, idx, "[312] Bairro"),
                    Cidade = Utilitario.RecuperarCampo(linha, 132, 35, true, idx, "[312] Cidade"),
                    CEP = Utilitario.RecuperarCampo(linha, 167, 9, true, idx, "[312] CEP"),
                    IBGE = Utilitario.RecuperarCampo(linha, 176, 9, false, idx, "[312] IBGE"),
                    Estado = Utilitario.RecuperarCampo(linha, 185, 9, true, idx, "[312] UF"),
                    Telefone = Utilitario.RecuperarCampo(linha, 194, 35, false, idx, "[312] Telefone"),
                    Email = Utilitario.RecuperarCampo(linhaEmail, 3, 60, false, idx, "[308] Email")
                };

                await ObterDadosEnderecoParticipanteAsync(participante);

                notaFiscal.NotaFiscalParticipante.Add(participante);
                break;
            }
        }

        private async Task ObterDadosEmitenteNotaFiscalAsync(NotaFiscal notaFiscal, string[] linhas, int posicao)
        {
            for (var idx = posicao - 1; idx > 0; idx--)
            {
                var linha = linhas[idx].Replace("\n", string.Empty).Replace("\r", string.Empty);

                if (!linha.StartsWith("311"))
                    continue;

                var participante = new NotaFiscalParticipante
                {
                    Inclusao = DateTime.Now,
                    Ativo = true,
                    Tipo = "Emitente",
                    CNPJ = Utilitario.RecuperarCampo(linha, 3, 14, true, idx, "[311] CNPJ"),
                    IE = Utilitario.RecuperarCampo(linha, 17, 14, true, idx, "[311] Inscrição Estadual"),
                    Logradouro = Utilitario.RecuperarCampo(linha, 32, 40, true, idx, "[311] Logradouro"),
                    Numero = "SN",
                    Bairro = "Centro",
                    Cidade = Utilitario.RecuperarCampo(linha, 72, 35, true, idx, "[311] Cidade"),
                    CEP = Utilitario.RecuperarCampo(linha, 107, 9, true, idx, "[311] CEP"),
                    Estado = Utilitario.RecuperarCampo(linha, 116, 9, true, idx, "[311] UF"),
                    Razao = Utilitario.RecuperarCampo(linha, 133, 40, true, idx, "[311] Razão social")
                };

                await ObterDadosEnderecoParticipanteAsync(participante);

                notaFiscal.NotaFiscalParticipante.Add(participante);
                break;
            }
        }

        private async Task ObterDadosEnderecoParticipanteAsync(NotaFiscalParticipante participante)
        {
            if (participante.CNPJ.Substring(0, 3) == "000")
                participante.CNPJ = participante.CNPJ.Substring(3, 11);

            if (participante.CEP.Contains("-"))
                participante.CEP.Replace("-", String.Empty).PadLeft(8, '0');

            if (!string.IsNullOrEmpty(participante.Telefone))
                participante.Telefone = participante.Telefone.RemoverMascaraTelefone();

            var cep = participante.CEP.Replace("-", string.Empty).Trim();
            var ibge = await _repositorioCEP.ConsultarIBGEPorCEPAsync(cep);
            participante.IBGE = ibge.ToString();

            if (!string.IsNullOrEmpty(participante.Logradouro))
            {
                var numero = Utilitario.RecuperarNumeros(participante.Logradouro);
                if (!string.IsNullOrEmpty(numero))
                {
                    var endereco = participante.Logradouro.Replace(numero, String.Empty);
                    endereco = endereco.Replace(",", String.Empty).Trim();
                    participante.Logradouro = endereco;

                    if (numero.Length > 10)
                        numero = numero.Substring(0, 10);

                    participante.Numero = numero;
                }
            }
        }
    }
}
