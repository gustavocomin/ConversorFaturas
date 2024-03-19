namespace Financeiro.Domain.Common
{
    public class IdentificadorComposto : Identificador
    {
        public string Descricao { get; set; } = "";
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}