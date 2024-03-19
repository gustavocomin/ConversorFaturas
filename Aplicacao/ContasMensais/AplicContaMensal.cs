using Financeiro.Common;
using Financeiro.Domain.ContasMensais;
using OfficeOpenXml;

namespace Financeiro.Aplicacao.ContasMensais
{
    public class AplicContaMensal : IAplicContaMensal
    {
        private readonly IRepContaMensal _repContaMensal;

        public AplicContaMensal(IRepContaMensal contaMensal)
        {
            _repContaMensal = contaMensal;
        }

        public async Task Teste()
        {
            var package = new ExcelPackage(new FileInfo(@"C:\Users\Gustavo Fagundes\Downloads\Financeiro\Contas\Gastos fixos da casa.xlsx"));
            // Obtém a primeira planilha do arquivo Excel
            var planilhas = package.Workbook.Worksheets.OrderByDescending(x => x.Name).ToList();

            var novasContas = new List<ContaMensal>();
            planilhas.ForEach(planilha =>
            {

                int totalLinhas = planilha.Dimension.Rows;
                int totalColunas = planilha.Dimension.Columns;

                // Itera sobre as células da planilha
                for (int linha = 2; linha <= totalLinhas; linha++)
                {
                    Functions.ConverterData(planilha.Name, out DateTime dataConvertida);
                    var conta = new ContaMensal
                    {
                        Data = dataConvertida,
                        MesAnoConta = dataConvertida.ToString("MM/yyyy")
                    };

                    for (int coluna = 1; coluna <= totalColunas; coluna++)
                    {
                        // Obtém o valor da célula atual
                        string valorCelula = planilha.Cells[linha, coluna].Value?.ToString() ?? "";

                        if (valorCelula == "Total")
                            return;

                        switch (coluna)
                        {
                            case 1:
                                conta.Descricao = valorCelula;
                                break;
                            case 2:
                                conta.Valor = decimal.Parse(valorCelula);
                                break;
                            case 4:
                                conta.NumeroParcela = string.IsNullOrWhiteSpace(valorCelula) ? 0 : MontaP(valorCelula);
                                break;
                            default:
                                break;
                        }
                    }
                    novasContas.Add(conta);
                }
            });

            await InsertAsync(novasContas);
        }

        public static int MontaP(string valorCelula)
        {
            string[] partes = valorCelula.Split('/');
            int parcelaAtual = int.Parse(partes[0]);

            return parcelaAtual;
        }

        public async Task<List<ContaMensal>> InsertAsync(List<ContaMensal> novasContas)
        {
            List<ContaMensal> contasAtuais = await _repContaMensal.FindAllAsync();
            List<ContaMensal> contas = new ContaMensal().CriarListaContas(contasAtuais, novasContas);
            await _repContaMensal.SaveChangesRangeAsync(contas);
            contas = await _repContaMensal.FindAllAsync();
            return contas;
        }
    }
}