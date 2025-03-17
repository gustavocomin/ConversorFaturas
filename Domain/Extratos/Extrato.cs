namespace Financeiro.Domain.Extratos
{
    public class Extrato
    {
        public int CodigoExtratoMesAno { get; set; }
        public string Descricao { get; set; } = "";
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }

        public string Categoria { get; set; } = "";
        public string Banco { get; set; } = "";
    }
}