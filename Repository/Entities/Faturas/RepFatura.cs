using Financeiro.Domain.Faturas;
using Financeiro.Repository.Common;
using Financeiro.Repository.Configurations.Db;

namespace Financeiro.Repository.Entities.Faturas
{
    public class RepFatura : RepBase<Fatura>, IRepFatura
    {
        public RepFatura(DataContext contexto) : base(contexto)
        {
        }
    }
}