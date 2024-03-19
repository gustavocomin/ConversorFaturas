using Financeiro.Domain.Common;
using Financeiro.Domain.Faturas;

namespace Financeiro.Domain.Faturas.MesAno
{
    public class FaturaMesAno : Identificador
    {
        public string MesAno { get; set; } = "";
        public virtual List<Fatura> Faturas { get; set; } = new List<Fatura>();

        public FaturaMesAno()
        {
        }

        public List<FaturaMesAno> CriarListaFaturaMesAno(List<FaturaMesAno> faturasAtuais, List<FaturaMesAno> novasFaturas)
        {
            List<Fatura> listaFaturas = new();

            faturasAtuais.ForEach(faturaAtual =>
            {
                listaFaturas = novasFaturas.Where(novaFatura => novaFatura.MesAno == faturaAtual.MesAno).SelectMany(x => x.Faturas).ToList();

                if (listaFaturas.Any())
                {
                    var faturasValidas = new Fatura().CriaListaFaturas(faturaAtual.Faturas, listaFaturas);
                    if (faturasValidas.Any())
                        faturaAtual.Faturas = faturasValidas;
                }
            });

            var faturasSemAgrupador = novasFaturas.Where(x => !faturasAtuais.Select(p => p.MesAno).Contains(x.MesAno))
                                                  .SelectMany(x => x.Faturas)
                                                  .ToList();
            if (faturasSemAgrupador.Any())
                faturasAtuais.AddRange(AgruparFaturasPorMesEAno(faturasSemAgrupador));

            return faturasAtuais;
        }

        public List<FaturaMesAno> AgruparFaturasPorMesEAno(List<Fatura> novasFaturas)
        {
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