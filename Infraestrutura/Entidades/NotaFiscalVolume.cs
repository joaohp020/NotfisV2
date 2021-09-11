using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Infraestrutura.Entidades
{
    public class NotaFiscalVolume
    {
        public Int64? ID { get; set; }
        public Int64? ID_NotaFiscal { get; set; }
        public String Chave { get; set; }
        public String Pedido { get; set; }
        public Int32? Volume { get; set; }
        public String UnidadeFrete { get; set; }
        public Boolean? Impresso { get; set; }
        public DateTime? DataImpressao { get; set; }
        public String Observacao { get; set; }
        public decimal? Comprimento { get; set; }
        public decimal? Largura { get; set; }
        public decimal? Altura { get; set; }
        public decimal? PesoUnitario { get; set; }
        public Boolean? ColetaRegistrada { get; set; }
        public DateTime? Inclusao { get; set; }
        public DateTime? Alteracao { get; set; }
        public Boolean? Ativo { get; set; }

        public virtual NotaFiscal NotaFiscal { get; set; }

        public NotaFiscalVolume() { }
    }

    public class NotaFiscalVolumeMap : EntityTypeConfiguration<NotaFiscalVolume>
    {
        public NotaFiscalVolumeMap()
        {
            this.ToTable("NotaFiscalVolume");
            this.HasKey(k => k.ID);

            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            this.Property(p => p.ID_NotaFiscal).IsRequired();
            this.Property(p => p.Chave).IsRequired();
            this.Property(p => p.Pedido);
            this.Property(p => p.Volume).IsRequired();
            this.Property(p => p.UnidadeFrete);
            this.Property(p => p.Impresso).IsRequired();
            this.Property(p => p.DataImpressao);
            this.Property(p => p.Observacao);
            this.Property(p => p.Inclusao).IsRequired();
            this.Property(p => p.Alteracao);
            this.Property(p => p.Ativo).IsRequired();

            //Nota Fiscal
            this.HasRequired(p => p.NotaFiscal)
                .WithMany()
                .HasForeignKey(p => p.ID_NotaFiscal);
        }
    }
}
