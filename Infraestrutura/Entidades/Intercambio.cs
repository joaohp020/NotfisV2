using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Infraestrutura.Entidades
{
    public class Intercambio
    {
        public Int64? ID { get; set; }
        public Int32? ID_Empresa { get; set; }
        public String GUID { get; set; }
        public String Remetente { get; set; }
        public String Destinatario { get; set; }
        public String ArquivoNome { get; set; }
        public Byte[] ArquivoConteudo { get; set; }
        public DateTime? Inclusao { get; set; }
        public DateTime? Alteracao { get; set; }
        public Boolean? Ativo { get; set; }


        public List<NotaFiscal> NotasFiscais { get; set; }

        public Intercambio()
        {
            this.NotasFiscais = new List<NotaFiscal>();
        }

        public class IntercambioMap : EntityTypeConfiguration<Intercambio>
    {
        public IntercambioMap()
        {
            this.ToTable("NOT_Intercambio");
            this.HasKey(k => k.ID);

            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            this.Property(p => p.ID_Empresa).IsRequired();
            this.Property(p => p.GUID).IsRequired();
            this.Property(p => p.Remetente).IsRequired();
            this.Property(p => p.Destinatario).IsRequired();
            this.Property(p => p.ArquivoNome).IsRequired();
            this.Property(p => p.ArquivoConteudo).IsRequired();
            this.Property(p => p.Inclusao).IsRequired();
            this.Property(p => p.Alteracao);
            this.Property(p => p.Ativo).IsRequired();

            //Empresa
            //this.HasRequired(p => p.Empresa)
            //    .WithMany()
            //    .HasForeignKey(k => k.ID_Empresa);

            //NotasFiscais
            this.HasMany(p => p.NotasFiscais)
                .WithRequired()
                .HasForeignKey(k => k.ID_Intercambio);

            //Status
            //this.HasMany(p => p.Status)
            //    .WithRequired()
            //    .HasForeignKey(k => k.ID_Intercambio);

        }
    }
    }
}
