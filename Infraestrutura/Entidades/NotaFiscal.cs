using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Infraestrutura.Entidades
{
    public class NotaFiscal
    {
        public Int64? ID { get; set; }
        public Int64? ID_Intercambio { get; set; }
        public Int64? ID_Empresa { get; set; }
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


        public virtual List<NotaFiscalArquivo> NotaFiscalArquivos { get; set; }
        public virtual Intercambio Intercambio { get; set; }
        public virtual List<NotaFiscalItem> NotaFiscalItens { get; set; }
        public virtual List<NotaFiscalParticipante> NotaFiscalParticipante { get; set; }
        public virtual List<NotaFiscalVolume> NotaFiscalVolumes { get; set; }
        public virtual List<LogCEP> LogCEP { get; set; }


        public NotaFiscal()
        {
            this.NotaFiscalItens = new List<NotaFiscalItem>();
            this.NotaFiscalArquivos = new List<NotaFiscalArquivo>();
            this.NotaFiscalParticipante = new List<NotaFiscalParticipante>();
            this.NotaFiscalVolumes = new List<NotaFiscalVolume>();
            this.LogCEP = new List<LogCEP>();
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

            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            this.Property(p => p.ID_Intercambio).IsRequired();
            //this.Property(p => p.CodigoWebcargo);
            this.Property(p => p.CodigoWebcargo).HasColumnType("varchar").HasMaxLength(50).IsUnicode(false);
            //this.Property(p => p.CodigoSAP);
            this.Property(p => p.CodigoSAP).HasColumnType("varchar").HasMaxLength(50).IsUnicode(false);
            //this.Property(p => p.Numero);
            this.Property(p => p.Numero).HasColumnType("varchar").HasMaxLength(20).IsUnicode(false);
            //this.Property(p => p.Serie);
            this.Property(p => p.Serie).HasColumnType("varchar").HasMaxLength(10).IsUnicode(false);
            this.Property(p => p.Emissao).IsRequired();
            //this.Property(p => p.Natureza);
            this.Property(p => p.Natureza).HasColumnType("varchar").HasMaxLength(400).IsUnicode(false);
            this.Property(p => p.Acondicionamento);
            this.Property(p => p.CondicaoFrete);
            this.Property(p => p.Pedido);
            this.Property(p => p.Embalagens).IsRequired();
            this.Property(p => p.PesoBruto).IsRequired().HasPrecision(16, 4);
            this.Property(p => p.ValorTotal).IsRequired();
            this.Property(p => p.Volumes).IsRequired();
            //this.Property(p => p.ChaveNFE);
            this.Property(p => p.ChaveNFE).HasColumnType("varchar").HasMaxLength(100).IsUnicode(false);
            //this.Property(p => p.CodigoNFE);
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

            //Intercâmbio
            this.HasRequired(p => p.Intercambio)
                .WithMany()
                .HasForeignKey(p => p.ID_Intercambio);

            //Participante
            this.HasMany(p => p.NotaFiscalParticipante)
                .WithRequired()
                .HasForeignKey(k => k.ID_NotaFiscal);

            //Itens
            this.HasMany(p => p.NotaFiscalItens)
                .WithOptional()
                .HasForeignKey(k => k.ID_NotaFiscal);

            //Status
            //this.HasMany(p => p.NotaFiscalStatus)
            //    .WithOptional()
            //    .HasForeignKey(k => k.ID_NotaFiscal);

            //Arquivo
            this.HasMany(p => p.NotaFiscalArquivos)
                .WithOptional()
                .HasForeignKey(k => k.ID_NotaFiscal);

            //Conhecimento Arquivo
            //this.HasMany(p => p.ConhecimentoArquivos)
            //    .WithOptional()
            //    .HasForeignKey(k => k.ID_NotaFiscal);

            //Volumes
            this.HasMany(p => p.NotaFiscalVolumes)
                .WithOptional()
                .HasForeignKey(k => k.ID_NotaFiscal);

            //Log CEP
            this.HasMany(p => p.LogCEP)
                .WithOptional()
                .HasForeignKey(k => k.ID_NotaFiscal);
        }
    }
}
