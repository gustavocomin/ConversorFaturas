using Financeiro.Domain.Extratos;
using Financeiro.Repository.Common;
using Financeiro.Repository.Configurations.Db;

namespace Financeiro.Repository.Entities.Extratos
{
    public class RepExtrato : RepBase<Extrato>, IRepExtrato
    {
        public RepExtrato(DataContext contexto) : base(contexto)
        {
        }
    }
}