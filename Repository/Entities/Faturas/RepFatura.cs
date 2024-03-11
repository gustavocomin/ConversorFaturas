using ConversorFaturas.Domain.Faturas;
using ConversorFaturas.Repository.Common;
using ConversorFaturas.Repository.Configurations.Db;

namespace ConversorFaturas.Repository.Entities.Faturas
{
    public class RepFatura : RepBase<Fatura>, IRepFatura
    {
        public RepFatura(DataContext contexto) : base(contexto)
        {
        }
    }
}