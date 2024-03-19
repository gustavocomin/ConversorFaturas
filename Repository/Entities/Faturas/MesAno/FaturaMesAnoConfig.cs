using Financeiro.Domain.Faturas.MesAno;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Repository.Entities.Faturas.MesAno
{
    public class FaturaMesAnoConfig : IEntityTypeConfiguration<FaturaMesAno>
    {
        public void Configure(EntityTypeBuilder<FaturaMesAno> builder)
        {
            builder.ToTable("FATURAMESANO");

            builder.HasKey(x => x.Codigo);

            builder.Property(a => a.Codigo)
                   .HasColumnName("CodigoFaturaMesAno");

            builder.Property(x => x.MesAno)
                   .HasColumnName("MesAno")
                   .HasMaxLength(10)
                   .IsRequired();

            builder.HasMany(fma => fma.Faturas)
                   .WithOne(f => f.FaturaMesAno)
                   .HasForeignKey(f => f.CodigoFaturaMesAno)
                   .IsRequired();

            builder.Navigation(fma => fma.Faturas).UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction)
                   .AutoInclude();
        }
    }
}