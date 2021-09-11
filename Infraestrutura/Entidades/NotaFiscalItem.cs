using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Infraestrutura.Entidades
{
    public class NotaFiscalItem
    {
        public Int64? ID { get; set; }
        public Int64? ID_NotaFiscal { get; set; }
        public String Codigo { get; set; }
        public Int32? Volumes { get; set; }
        public String Acondicionamento { get; set; }
        public String Descricao { get; set; }
        public DateTime? Inclusao { get; set; }
        public DateTime? Alteracao { get; set; }
        public Boolean? Ativo { get; set; }

        public virtual NotaFiscal NotaFiscal { get; set; }

        public NotaFiscalItem() { }
    }

    public class NotaFiscalItemMap : EntityTypeConfiguration<NotaFiscalItem>
    {
        public NotaFiscalItemMap()
        {
            this.ToTable("NotaFiscalItem");
            this.HasKey(k => k.ID);

            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            this.Property(p => p.ID_NotaFiscal).IsRequired();
            this.Property(p => p.Codigo).IsRequired();
            this.Property(p => p.Volumes).IsRequired();
            this.Property(p => p.Acondicionamento).IsRequired();
            this.Property(p => p.Descricao).IsRequired();
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
