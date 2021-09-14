using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Infraestrutura.Entidades
{
    public class NotaFiscalParticipante
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ID_NotaFiscal { get; set; }
        public string Tipo { get; set; }
        public string CNPJ { get; set; }
        public string IE { get; set; }
        public string ContaCorrente { get; set; }
        public string Razao { get; set; }
        public string CodigoAzul { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string IBGE { get; set; }
        public string Complemento { get; set; }
        public string Armazem { get; set; }
        public string BaseOperacao { get; set; }
        public string CodigoFrete { get; set; }
        public string CEPFrete { get; set; }
        public string TipoRetira { get; set; }
        public string TipoIdentificacaoDestinatario { get; set; }
        public DateTime Inclusao { get; set; }
        public DateTime? Alteracao { get; set; }
        public bool Ativo { get; set; }

        public virtual NotaFiscal NotaFiscal { get; set; }

        public NotaFiscalParticipante() { }

    }

    public class NotaFiscalParticipanteMap : EntityTypeConfiguration<NotaFiscalParticipante>
    {
        public NotaFiscalParticipanteMap()
        {
            ToTable("NotaFiscalParticipante");
            HasKey(k => k.ID);
            
            Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            Property(p => p.ID_NotaFiscal).IsRequired();
            Property(p => p.Tipo).IsRequired();
            Property(p => p.CNPJ).IsRequired();
            Property(p => p.IE);
            Property(p => p.ContaCorrente);
            Property(p => p.Razao).IsRequired();
            Property(p => p.CodigoAzul);
            Property(p => p.Email);
            Property(p => p.Telefone);
            Property(p => p.Logradouro);
            Property(p => p.Bairro);
            Property(p => p.CEP).IsRequired();
            Property(p => p.Cidade);
            Property(p => p.Estado);
            Property(p => p.IBGE).IsRequired();
            Property(p => p.Complemento);
            Property(p => p.Armazem);
            Property(p => p.BaseOperacao);
            Property(p => p.CodigoFrete);
            Property(p => p.CEPFrete);
            Property(p => p.TipoRetira);
            Property(p => p.TipoIdentificacaoDestinatario);
            Property(p => p.Inclusao).IsRequired();
            Property(p => p.Alteracao);
            Property(p => p.Ativo).IsRequired();

            //Nota Fiscal
            HasRequired(p => p.NotaFiscal)
                .WithMany()
                .HasForeignKey(p => p.ID_NotaFiscal);
        }
    }
}
