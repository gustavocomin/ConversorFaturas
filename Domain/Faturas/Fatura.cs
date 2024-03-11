using ConversorFaturas.Domain.Common;
using ConversorFaturas.Domain.Faturas.MesAno;

namespace ConversorFaturas.Domain.Faturas
{
    public class Fatura : Identificador
    {
        public DateTime Data { get; set; }
        public string Categoria { get; set; } = "";
        public string Descricao { get; set; } = "";
        public decimal Valor { get; set; }

        public int CodigoFaturaMesAno { get; set; }
        public virtual FaturaMesAno FaturaMesAno { get; set; }

        public Fatura()
        {
        }

        public Fatura(Fatura fatura)
        {
            Data = fatura.Data;
            Categoria = fatura.Categoria;
            Descricao = fatura.Descricao;
            Valor = fatura.Valor;
        }

        public List<Fatura> CriaListaFaturas(List<Fatura> faturasAtuais, List<Fatura> novasFaturas)
        {
            List<Fatura> faturas = new();
            novasFaturas.ForEach(nova =>
            {
                if (FaturaJaExiste(faturasAtuais, nova))
                    faturas.Add(new Fatura(nova));
            });

            return faturas;
        }

        private static bool FaturaJaExiste(List<Fatura> faturas, Fatura novaFatura)
        {
            return faturas.Exists(x =>
                string.Equals(x.Descricao.Trim(), novaFatura.Descricao.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.Categoria.Trim(), novaFatura.Categoria.Trim(), StringComparison.OrdinalIgnoreCase) &&
                x.Data == novaFatura.Data &&
                x.Valor == novaFatura.Valor);
        }
    }
}