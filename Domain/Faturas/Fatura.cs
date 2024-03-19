using Financeiro.Domain.Common;
using Financeiro.Domain.Faturas.MesAno;

namespace Financeiro.Domain.Faturas
{
    public class Fatura : IdentificadorComposto
    {
        public string Categoria { get; set; } = "";

        public int CodigoFaturaMesAno { get; set; }
        public virtual FaturaMesAno FaturaMesAno { get; set; } = new FaturaMesAno();

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
                if (!FaturaJaExiste(faturasAtuais, nova))
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