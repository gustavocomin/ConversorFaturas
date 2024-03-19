using Financeiro.Aplicacao.Extratos.MesAno;
using Financeiro.Common;
using Financeiro.Domain.Extratos;
using Financeiro.Domain.Extratos.MesAno;

namespace Financeiro.Aplicacao.Conversor.Extratos
{
    public class AplicConversorExtratos : IAplicConversorExtratos
    {
        private readonly IAplicExtratoMesAno _aplicExtratoMesAno;

        public AplicConversorExtratos(IAplicExtratoMesAno aplicExtratoMesAno)
        {
            _aplicExtratoMesAno = aplicExtratoMesAno ?? throw new ArgumentNullException(nameof(aplicExtratoMesAno));
        }

        public async Task<List<ExtratoMesAno>> ConverterArquivosCsvParaExtratosAsync(string origem)
        {
            var novosExtratos = ConverterArquivos(origem);
            var extratos = await SalvarExtratos(novosExtratos);
            return extratos;
        }

        private async Task<List<ExtratoMesAno>> SalvarExtratos(List<ExtratoMesAno> novasExtratos)
        {
            return await _aplicExtratoMesAno.InsertAsync(novasExtratos);
        }

        public List<ExtratoMesAno> ConverterArquivos(string origem)
        {
            List<string> pastas = Directory.GetDirectories(origem).ToList();

            List<Extrato> extratos = new();
            pastas.ForEach(x =>
            {
                List<string> arquivosCsv = Directory.GetFiles(x, "*.csv")
                                                    .OrderByDescending(x => x)
                                                    .ToList();

                foreach (string arquivoCsv in arquivosCsv)
                {
                    extratos.AddRange(ConverterArquivoCsvParaExtrato(arquivoCsv));
                }
            });
            List<ExtratoMesAno> extratosMesAno = new ExtratoMesAno().AgruparExtratosPorMesEAno(extratos);

            return extratosMesAno;
        }

        private static List<Extrato> ConverterArquivoCsvParaExtrato(string csvFilePath)
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            List<Extrato> extratos = TransformaDados(lines);

            return extratos;
        }

        private static List<Extrato> TransformaDados(string[] lines)
        {
            List<Extrato> extratos = new();

            var linhaAtual = 2;
            for (int linha = 1; linha < lines.Length; linha++)
            {
                var extrato = new Extrato();
                string[] values = lines[linha].Split(',');

                for (int coluna = 0; coluna < values.Length; coluna++)
                {
                    var valorCelula = values[coluna];

                    if (Functions.ConverterData(valorCelula, out DateTime dataConvertida))
                    {
                        extrato.Data = dataConvertida;
                    }
                    else if (Functions.ConverterDecimal(valorCelula, out decimal valorConvertido))
                    {
                        extrato.Valor = valorConvertido;
                    }
                    else
                    {
                        switch (coluna)
                        {
                            case 2:
                                extrato.Identificador = Guid.Parse(valorCelula);
                                break;
                            case 3:
                                extrato.Descricao = valorCelula.Captilize();
                                break;

                        }
                    }
                }
                extratos.Add(extrato);

                linhaAtual++;
            }

            return extratos;
        }
    }
}