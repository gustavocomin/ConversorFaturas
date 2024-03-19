namespace Financeiro.Aplicacao.Dto
{
    public class TotalizadorDto
    {
        public CelulaDto Descricao { get; set; } = new CelulaDto();
        public CelulaDto Totalizador { get; set; } = new CelulaDto();

        public TotalizadorDto()
        {
        }

        public TotalizadorDto(CelulaDto descricao, CelulaDto totalizador)
        {
            Descricao = descricao;
            Totalizador = totalizador;
        }
    }
}