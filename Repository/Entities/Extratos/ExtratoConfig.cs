using Financeiro.Domain.Extratos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Repository.Entities.Extratos
{
    public class ExtratoConfig : IEntityTypeConfiguration<Extrato>
    {
        public void Configure(EntityTypeBuilder<Extrato> builder)
        {
            builder.ToTable("EXTRATO");

            builder.HasKey(x => x.Codigo);

            builder.Property(a => a.Codigo)
                   .HasColumnName("CodigoExtrato");

            builder.Property(x => x.Descricao)
                   .HasColumnName("DESCRICAO")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Data)
                  .HasColumnName("DATA")
                  .HasColumnType("DATE")
                  .IsRequired();

            builder.Property(x => x.Valor)
                   .HasColumnName("VALOR")
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(x => x.CodigoExtratoMesAno)
                   .HasColumnName("CODIGOEXTRATOMESANO")
                   .HasColumnType("INT")
                   .IsRequired();

            builder.HasOne(f => f.ExtratoMesAno)
                   .WithMany()
                   .HasForeignKey(f => f.CodigoExtratoMesAno)
                   .IsRequired();
        }
    }
}