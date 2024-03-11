namespace ConversorFaturas.Aplicacao.Dto
{
    public class TotalizadorCategoria
    {
        public string Categoria { get; set; } = "";
        public decimal Valor { get; set; } = 0;

        public TotalizadorCategoria()
        {
        }

        public TotalizadorCategoria(string categoria, decimal valor)
        {
            Categoria = categoria;
            Valor = valor;
        }
    }
}