namespace Financeiro.Domain.Faturas
{
    public class Fatura
    {
        public string Categoria { get; set; } = "";
        public string Descricao { get; set; } = "";
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Banco { get; set; } = "";
        public string Tipo { get; set; } = "";
    }
}