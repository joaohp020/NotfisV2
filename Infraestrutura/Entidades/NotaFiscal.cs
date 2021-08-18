using System;

namespace Infraestrutura.Entidades
{
    public class NotaFiscal
    {
        public Int64? ID { get; set; }
        public Int64? ID_Intercambio { get; set; }
        public String CodigoWebcargo { get; set; }
        public String CodigoSAP { get; set; }
        public String Numero { get; set; }
        public String Serie { get; set; }
        public DateTime? Emissao { get; set; }
        public String Natureza { get; set; }
        public String Acondicionamento { get; set; }
        public String CondicaoFrete { get; set; }
        public String Pedido { get; set; }
        public Decimal? Embalagens { get; set; }
        public Decimal? PesoBruto { get; set; }
        public Decimal? ValorTotal { get; set; }
        public Decimal? Volumes { get; set; }
        public String ChaveNFE { get; set; }
        public String CodigoNFE { get; set; }
        public int? ComprimentoCaixa { get; set; }
        public int? LarguraCaixa { get; set; }
        public int? AlturaCaixa { get; set; }
        public string Produto { get; set; }
        public string Manuseio { get; set; }
        public DateTime? DataPedido { get; set; }
        public DateTime? DataFaturado { get; set; }
        public DateTime? DataCheckout { get; set; }
        public DateTime? DataCancelado { get; set; }
        public DateTime? Inclusao { get; set; }
        public DateTime? Alteracao { get; set; }
        public Boolean? Ativo { get; set; }
        public Decimal? PesoCubagem { get; set; }
        public string Romaneio { get; set; }
        public bool? CargaPerigosa { get; set; }
        public String TipoEntrega { get; set; }
        public string SiglaServico { get; set; }
    }
}
