using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Infraestrutura.Entidades
{
    public class NotaFiscalParticipante
    {
        public Int64? ID { get; set; }
        public Int64? ID_NotaFiscal { get; set; }
        public String Tipo { get; set; }
        public String CNPJ { get; set; }
        public String IE { get; set; }
        public String ContaCorrente { get; set; }
        public String Razao { get; set; }
        public String CodigoAzul { get; set; }
        public String Email { get; set; }
        public String Telefone { get; set; }
        public String Logradouro { get; set; }
        public String Numero { get; set; }
        public String Bairro { get; set; }
        public String CEP { get; set; }
        public String Cidade { get; set; }
        public String Estado { get; set; }
        public String IBGE { get; set; }
        public String Complemento { get; set; }
        public String Armazem { get; set; }
        public String BaseOperacao { get; set; }
        public String CodigoFrete { get; set; }
        public String CEPFrete { get; set; }
        public String TipoRetira { get; set; }
        public String TipoIdentificacaoDestinatario { get; set; }
        public DateTime? Inclusao { get; set; }
        public DateTime? Alteracao { get; set; }
        public Boolean? Ativo { get; set; }

        public virtual NotaFiscal NotaFiscal { get; set; }

        public NotaFiscalParticipante() { }

    }

    public class NotaFiscalParticipanteMap : EntityTypeConfiguration<NotaFiscalParticipante>
    {
        public NotaFiscalParticipanteMap()
        {
            this.ToTable("NotaFiscalParticipante");
            this.HasKey(k => k.ID);

            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            this.Property(p => p.ID_NotaFiscal).IsRequired();
            this.Property(p => p.Tipo).IsRequired();
            this.Property(p => p.CNPJ).IsRequired();
            this.Property(p => p.IE);
            this.Property(p => p.ContaCorrente);
            this.Property(p => p.Razao).IsRequired();
            this.Property(p => p.CodigoAzul);
            this.Property(p => p.Email);
            this.Property(p => p.Telefone);
            this.Property(p => p.Logradouro);
            this.Property(p => p.Bairro);
            this.Property(p => p.CEP).IsRequired();
            this.Property(p => p.Cidade);
            this.Property(p => p.Estado);
            this.Property(p => p.IBGE).IsRequired();
            this.Property(p => p.Complemento);
            this.Property(p => p.Armazem);
            this.Property(p => p.BaseOperacao);
            this.Property(p => p.CodigoFrete);
            this.Property(p => p.CEPFrete);
            this.Property(p => p.TipoRetira);
            this.Property(p => p.TipoIdentificacaoDestinatario);
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
