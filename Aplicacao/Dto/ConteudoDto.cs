namespace ConversorFaturas.Aplicacao.Dto
{
    public class ConteudoDto
    {
        public DateTime Data { get; set; }
        public string Categoria { get; set; } = "";
        public string Descricao { get; set; } = "";
        public decimal Valor { get; set; }
    }
}
