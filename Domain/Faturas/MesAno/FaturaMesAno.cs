using ConversorFaturas.Domain.Common;

namespace ConversorFaturas.Domain.Faturas.MesAno
{
    public class FaturaMesAno : Identificador
    {
        public string MesAno { get; set; }
        public virtual List<Fatura> Faturas { get; set; }

        public FaturaMesAno()
        {
        }

        public List<FaturaMesAno> CriarListaFaturaMesAno(List<FaturaMesAno> faturasMesAno, List<Fatura> novasFaturas)
        {
            faturasMesAno.ForEach(x =>
            {
                var faturasValidas = new Fatura().CriaListaFaturas(x.Faturas, novasFaturas);
                x.Faturas = faturasValidas;
            });

            var listaFaturaMesAno = novasFaturas.GroupBy(x => new { x.Data.Month, x.Data.Year })
            .Select(x => new FaturaMesAno
            {
                MesAno = $"{x.Key.Month:D2}/{x.Key.Year}",
                Faturas = novasFaturas.Where(p => p.Data.Month == x.Key.Month
                                               && p.Data.Year == x.Key.Year).ToList()
            }).ToList();

            return listaFaturaMesAno;
        }
    }
}