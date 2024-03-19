using Financeiro.Domain.Extratos.MesAno;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Repository.Entities.Extratos.MesAno
{
    public class ExtratoMesAnoConfig : IEntityTypeConfiguration<ExtratoMesAno>
    {
        public void Configure(EntityTypeBuilder<ExtratoMesAno> builder)
        {
            builder.ToTable("EXTRATOMESANO");

            builder.HasKey(x => x.Codigo);

            builder.Property(a => a.Codigo)
                   .HasColumnName("CodigoExtratoMesAno");

            builder.Property(x => x.MesAno)
                   .HasColumnName("MesAno")
                   .HasMaxLength(10)
                   .IsRequired();

            builder.HasMany(fma => fma.Extratos)
                   .WithOne(f => f.ExtratoMesAno)
                   .HasForeignKey(f => f.CodigoExtratoMesAno)
                   .IsRequired();

            builder.Navigation(fma => fma.Extratos).UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction)
                   .AutoInclude();
        }
    }
}