namespace ConversorFaturas.Aplicacao.Dto
{
    public class TotalizadorAno
    {
        public decimal Valor { get; set; } = 0;
        public string Ano { get; set; } = "";
        public string Categoria { get; set; } = "";

        public TotalizadorAno()
        {
        }

        public TotalizadorAno(List<TotalizadorAno> totalizadorAnos)
        {
            totalizadorAnos.AddRange(totalizadorAnos);
        }
    }
}