using Financeiro.Domain.Common;
using Financeiro.Domain.Extratos;

namespace Financeiro.Domain.Extratos.MesAno
{
    public class ExtratoMesAno : Identificador
    {
        public string MesAno { get; set; } = "";
        public virtual List<Extrato> Extratos { get; set; } = new List<Extrato>();

        public ExtratoMesAno()
        {
        }

        public List<ExtratoMesAno> CriarListaExtratoMesAno(List<ExtratoMesAno> faturasAtuais, List<ExtratoMesAno> novasFaturas)
        {
            List<Extrato> listaFaturas = new();

            faturasAtuais.ForEach(faturaAtual =>
            {
                listaFaturas = novasFaturas.Where(novaFatura => novaFatura.MesAno == faturaAtual.MesAno).SelectMany(x => x.Extratos).ToList();

                if (listaFaturas.Any())
                {
                    var faturasValidas = new Extrato().CriaListalistaExtrato(faturaAtual.Extratos, listaFaturas);
                    if (faturasValidas.Any())
                        faturaAtual.Extratos = faturasValidas;
                }
            });

            var faturasSemAgrupador = novasFaturas.Where(x => !faturasAtuais.Select(p => p.MesAno).Contains(x.MesAno))
                                                  .SelectMany(x => x.Extratos)
                                                  .ToList();
            if (faturasSemAgrupador.Any())
                faturasAtuais.AddRange(AgruparExtratosPorMesEAno(faturasSemAgrupador));

            return faturasAtuais;
        }

        public List<ExtratoMesAno> AgruparExtratosPorMesEAno(List<Extrato> novasFaturas)
        {
            var listaExtratoMesAno = novasFaturas.GroupBy(x => new { x.Data.Month, x.Data.Year })
            .Select(x => new ExtratoMesAno
            {
                MesAno = $"{x.Key.Month:D2}/{x.Key.Year}",
                Extratos = novasFaturas.Where(p => p.Data.Month == x.Key.Month
                                               && p.Data.Year == x.Key.Year).ToList()
            }).ToList();

            return listaExtratoMesAno;
        }
    }
}