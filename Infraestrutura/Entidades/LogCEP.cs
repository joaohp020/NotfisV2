using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Infraestrutura.Entidades
{
    public class LogCEP
    {
        public Int64? ID { get; set; }
        public Int32 ID_Empresa { get; set; }
        public Int64 ID_NotaFiscal { get; set; }
        public String CEP { get; set; }
        public DateTime? Inclusao { get; set; }
        public Boolean? Ativo { get; set; }

        public LogCEP() { }
    }

    public class LogCEPMap : EntityTypeConfiguration<LogCEP>
    {
        public LogCEPMap()
        {
            this.ToTable("LogCep");
            this.HasKey(k => k.ID);

            this.Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            this.Property(p => p.ID_Empresa).IsRequired();
            this.Property(p => p.ID_NotaFiscal).IsRequired();
            this.Property(p => p.CEP).IsRequired();
            this.Property(p => p.Inclusao).IsRequired();
            this.Property(p => p.Ativo).IsRequired();
        }
    }
}
