using Financeiro.Domain.Common;
using Financeiro.Domain.Extratos.MesAno;

namespace Financeiro.Domain.Extratos
{
    public class Extrato : IdentificadorComposto
    {
        public Guid Identificador { get; set; }
        public int CodigoExtratoMesAno { get; internal set; }

        public virtual ExtratoMesAno ExtratoMesAno { get; set; } = new ExtratoMesAno();

        public Extrato()
        {
        }

        public Extrato(Extrato extrato)
        {
            Data = extrato.Data;
            Identificador = extrato.Identificador;
            Descricao = extrato.Descricao;
            Valor = extrato.Valor;
        }

        internal List<Extrato> CriaListalistaExtrato(List<Extrato> extratosAtuais, List<Extrato> novosExtratos)
        {
            List<Extrato> extratos = new();
            novosExtratos.ForEach(nova =>
            {
                if (!ExtratoJaExiste(novosExtratos, nova))
                    extratos.Add(new Extrato(nova));
            });

            return extratos;
        }

        private static bool ExtratoJaExiste(List<Extrato> extratos, Extrato novoExtrato)
        {
            return extratos.Exists(x =>
                string.Equals(x.Descricao.Trim(), novoExtrato.Descricao.Trim(), StringComparison.OrdinalIgnoreCase) &&
                Equals(x.Identificador, novoExtrato.Identificador) &&
                x.Data == novoExtrato.Data &&
                x.Valor == novoExtrato.Valor);
        }
    }
}