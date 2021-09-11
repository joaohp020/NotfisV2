using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Infraestrutura.Entidades
{
    public class Intercambio
    {
        public long? ID { get; set; }
        public int? ID_Empresa { get; set; }
        public string GUID { get; set; }
        public string Remetente { get; set; }
        public string Destinatario { get; set; }
        public string ArquivoNome { get; set; }
        public byte[] ArquivoConteudo { get; set; }
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
                this.ToTable("Intercambios");
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
