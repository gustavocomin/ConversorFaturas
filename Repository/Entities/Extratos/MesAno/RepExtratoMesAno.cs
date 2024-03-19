using Financeiro.Domain.Extratos.MesAno;
using Financeiro.Repository.Common;
using Financeiro.Repository.Configurations.Db;

namespace Financeiro.Repository.Entities.Extratos.MesAno
{
    public class RepExtratoMesAno : RepBase<ExtratoMesAno>, IRepExtratoMesAno
    {
        public RepExtratoMesAno(DataContext contexto) : base(contexto)
        {
        }
    }
}