using Financeiro.Domain.Faturas.MesAno;
using Financeiro.Repository.Common;
using Financeiro.Repository.Configurations.Db;

namespace Financeiro.Repository.Entities.Faturas.MesAno
{
    public class RepFaturaMesAno : RepBase<FaturaMesAno>, IRepFaturaMesAno
    {
        public RepFaturaMesAno(DataContext contexto) : base(contexto)
        {
        }
    }
}