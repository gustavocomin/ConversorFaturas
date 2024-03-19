using Financeiro.Domain.ContasMensais;
using Financeiro.Repository.Common;
using Financeiro.Repository.Configurations.Db;

namespace Financeiro.Repository.Entities.ContasMensais
{
    public class RepContaMensal : RepBase<ContaMensal>, IRepContaMensal
    {
        public RepContaMensal(DataContext contexto) : base(contexto)
        {
        }
    }
}