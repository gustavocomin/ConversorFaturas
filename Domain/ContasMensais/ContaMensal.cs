using Financeiro.Domain.Common;

namespace Financeiro.Domain.ContasMensais
{
    public class ContaMensal : IdentificadorComposto
    {
        public int NumeroParcela { get; set; }
        public string MesAnoConta { get; set; } = "";

        public ContaMensal()
        {
        }

        public List<ContaMensal> CriarListaContas(List<ContaMensal> contasAtuais, List<ContaMensal> novasContas)
        {
            List<ContaMensal> contas = new();
            novasContas.ForEach(novaConta =>
            {
                if (!ContaJaExiste(contasAtuais, novaConta))
                    contas.Add(novaConta);
            });

            return contas;
        }

        private static bool ContaJaExiste(List<ContaMensal> contas, ContaMensal novaConta)
        {
            return contas.Exists(x =>
                string.Equals(x.Descricao.Trim(), novaConta.Descricao.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.MesAnoConta.Trim(), novaConta.MesAnoConta.Trim(), StringComparison.OrdinalIgnoreCase) &&
                x.Valor == novaConta.Valor);
        }
    }
}