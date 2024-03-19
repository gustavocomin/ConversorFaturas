using Financeiro.Domain.ContasMensais;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financeiro.Repository.Entities.ContasMensais
{
    public class ContaMensalConfig : IEntityTypeConfiguration<ContaMensal>
    {
        public void Configure(EntityTypeBuilder<ContaMensal> builder)
        {
            builder.ToTable("CONTAMENSAL");

            builder.HasKey(x => x.Codigo);

            builder.Property(a => a.Codigo)
                   .HasColumnName("CODIGOCONTAMENSAL");

            builder.Property(x => x.Descricao)
                   .HasColumnName("DESCRICAO")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Valor)
                   .HasColumnName("VALOR")
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();
        }
    }
}