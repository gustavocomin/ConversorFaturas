using Financeiro.Domain.Faturas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Repository.Entities.Faturas
{
    public class FaturaConfig : IEntityTypeConfiguration<Fatura>
    {
        public void Configure(EntityTypeBuilder<Fatura> builder)
        {
            builder.ToTable("FATURA");

            builder.HasKey(x => x.Codigo);

            builder.Property(a => a.Codigo)
                   .HasColumnName("CodigoFatura");

            builder.Property(x => x.Descricao)
                   .HasColumnName("DESCRICAO")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Categoria)
                   .HasColumnName("CATEGORIA")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.Data)
                  .HasColumnName("DATA")
                  .HasColumnType("DATE")
                  .IsRequired();

            builder.Property(x => x.Valor)
                   .HasColumnName("VALOR")
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(x => x.CodigoFaturaMesAno)
                   .HasColumnName("CODIGOFATURAMESANO")
                   .HasColumnType("INT")
                   .IsRequired();

            builder.HasOne(f => f.FaturaMesAno)
                   .WithMany()
                   .HasForeignKey(f => f.CodigoFaturaMesAno)
                   .IsRequired();
        }
    }
}