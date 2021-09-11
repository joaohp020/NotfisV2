using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Infraestrutura.Entidades
{
    public class CEP
    {
        public Int32? ID { get; set; }
        public String Pais { get; set; }
        public String UF { get; set; }
        public String Cidade { get; set; }
        public Int32? CEPInicial { get; set; }
        public Int32? CEPFinal { get; set; }
        public Int32? IBGE { get; set; }
        public DateTime? Inclusao { get; set; }
        public DateTime? Alteracao { get; set; }
        public Boolean? Ativo { get; set; }

        public CEP() { }
    }

    public class CEPMap : EntityTypeConfiguration<CEP>
    {
        public CEPMap()
        {
            this.ToTable("LOCCEP");
            this.HasKey(k => k.ID);

            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            this.Property(p => p.Pais).IsRequired();
            this.Property(p => p.UF).IsRequired();
            this.Property(p => p.Cidade).IsRequired();
            this.Property(p => p.CEPInicial).IsRequired();
            this.Property(p => p.CEPFinal).IsRequired();
            this.Property(p => p.IBGE).IsRequired();
            this.Property(p => p.Inclusao).IsRequired();
            this.Property(p => p.Alteracao);
            this.Property(p => p.Ativo).IsRequired();
        }
    }
}
