using ConversorFaturas.Domain.Faturas.MesAno;
using ConversorFaturas.Repository.Common;
using ConversorFaturas.Repository.Configurations.Db;

namespace ConversorFaturas.Repository.Entities.Faturas.MesAno
{
    public class RepFaturaMesAno : RepBase<FaturaMesAno>, IRepFaturaMesAno
    {
        public RepFaturaMesAno(DataContext contexto) : base(contexto)
        {
        }
    }
}