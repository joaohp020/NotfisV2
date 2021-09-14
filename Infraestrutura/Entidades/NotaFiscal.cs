using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Infraestrutura.Entidades
{
    public class NotaFiscal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ID_Intercambio { get; set; }
        public int ID_Empresa { get; set; }
        public string CodigoWebcargo { get; set; }
        public string CodigoSAP { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public DateTime? Emissao { get; set; }
        public string Natureza { get; set; }
        public string Acondicionamento { get; set; }
        public string CondicaoFrete { get; set; }
        public string Pedido { get; set; }
        public Decimal? Embalagens { get; set; }
        public Decimal? PesoBruto { get; set; }
        public Decimal? ValorTotal { get; set; }
        public Decimal? Volumes { get; set; }
        public string ChaveNFE { get; set; }
        public string CodigoNFE { get; set; }
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


        public virtual List<NotaFiscalParticipante> NotaFiscalParticipante { get; set; }

        public NotaFiscal()
        {
            this.NotaFiscalParticipante = new List<NotaFiscalParticipante>();
        }

        public string BuscarTipoEmissao()
        {
            const string Normal = "Normal";
            const string Redespacho = "Redespacho Intermediario";
            const string ModalidadeCTe = "57";

            if (string.IsNullOrEmpty(ChaveNFE)) return Normal;

            if (ChaveNFE.Length < 44) return Normal;

            if (ChaveNFE.Substring(20, 2) == ModalidadeCTe) return Redespacho;

            return Normal;
        }

    }

    public class NotaFiscalMap : EntityTypeConfiguration<NotaFiscal>
    {

        public NotaFiscalMap()
        {
            this.ToTable("NotasFiscais");
            this.HasKey(k => k.ID);

            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired().HasColumnName("ID"); ;
            this.Property(p => p.ID_Intercambio).IsRequired();
            this.Property(p => p.CodigoWebcargo);
            this.Property(p => p.CodigoWebcargo).HasColumnType("varchar").HasMaxLength(50).IsUnicode(false);
            this.Property(p => p.CodigoSAP);
            this.Property(p => p.CodigoSAP).HasColumnType("varchar").HasMaxLength(50).IsUnicode(false);
            this.Property(p => p.Numero);
            this.Property(p => p.Numero).HasColumnType("varchar").HasMaxLength(20).IsUnicode(false);
            this.Property(p => p.Serie);
            this.Property(p => p.Serie).HasColumnType("varchar").HasMaxLength(10).IsUnicode(false);
            this.Property(p => p.Emissao).IsRequired();
            this.Property(p => p.Natureza);
            this.Property(p => p.Natureza).HasColumnType("varchar").HasMaxLength(400).IsUnicode(false);
            this.Property(p => p.Acondicionamento);
            this.Property(p => p.CondicaoFrete);
            this.Property(p => p.Pedido);
            this.Property(p => p.Embalagens).IsRequired();
            this.Property(p => p.PesoBruto).IsRequired().HasPrecision(16, 4);
            this.Property(p => p.ValorTotal).IsRequired();
            this.Property(p => p.Volumes).IsRequired();
            this.Property(p => p.ChaveNFE);
            this.Property(p => p.ChaveNFE).HasColumnType("varchar").HasMaxLength(100).IsUnicode(false);
            this.Property(p => p.CodigoNFE);
            this.Property(p => p.CodigoNFE).HasColumnType("varchar").HasMaxLength(100).IsUnicode(false);
            this.Property(p => p.ComprimentoCaixa);
            this.Property(p => p.LarguraCaixa);
            this.Property(p => p.AlturaCaixa);
            this.Property(p => p.Produto);
            this.Property(p => p.Manuseio);
            this.Property(p => p.Inclusao).IsRequired();
            this.Property(p => p.Alteracao);
            this.Property(p => p.Ativo).IsRequired();
            this.Property(p => p.PesoCubagem).HasPrecision(16, 4);
            this.Property(p => p.Romaneio);
            this.Property(p => p.TipoEntrega);
            this.Property(p => p.SiglaServico);

            //Participante
            this.HasMany(p => p.NotaFiscalParticipante)
                .WithRequired()
                .HasForeignKey(k => k.ID_NotaFiscal);
        }
    }
}
