using System.Globalization;
using ConversorFaturas.Aplicacao.Dto;
using ConversorFaturas.Domain.Faturas;
using OfficeOpenXml;

namespace ConversorFaturas.Common
{
    public static class Functions
    {
        private static int _linhaAtual = 0;
        private static int _codigoColuna = ObterCodigoAsciiLetra('A');

        internal static void CriarCabecalho(ExcelWorksheet worksheet)
        {
            string[] headers = { "Mês", "Ano", "Categoria", "Nome", "Valor" };
            for (int j = 0; j < headers.Length; j++)
            {
                var celula = worksheet.Cells[1, j + 1];
                celula.Value = headers[j];
                FormatarCelulasDestaque(celula, false, false);
            }
        }

        internal static ExcelWorksheet CriarPlanilha(ExcelPackage package, string nomeDoArquivo, bool tratarNome)
        {
            if (tratarNome)
                nomeDoArquivo = TratarNomePlanilha(nomeDoArquivo);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(nomeDoArquivo);
            return worksheet;
        }

        internal static string TratarNomePlanilha(string nomeDoArquivo)
        {
            nomeDoArquivo = nomeDoArquivo.ToUpper().Replace("NUBANK-", "");
            ConverterData(nomeDoArquivo, out DateTime dataConvertida);

            return dataConvertida.ToString("MM-yyyy");
        }

        internal static bool ConverterData(string data, out DateTime dataConvertida)
        {
            string[] formatos = { "yyyy-MM-dd", "dd/MM/yyyy", "yyyyMMdd", "yyyy-MM", "DD/YYYY", "D/YYYY" };
            return DateTime.TryParseExact(data, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataConvertida);
        }

        internal static bool ConverterDecimal(string valor, out decimal valorConvertido)
        {
            return decimal.TryParse(valor, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out valorConvertido);
        }

        internal static void FormatarCelulasDestaque(ExcelRange celula, bool numero, bool menorQueZero)
        {
            celula.Style.Font.Size = 12;
            celula.Style.Font.Name = "Arial";
            celula.Style.Font.Bold = true;
            if (numero)
                FormatarComoNumero(celula, menorQueZero);

            celula.AutoFitColumns();
        }

        internal static void FormatarTotalizador(ExcelWorksheet planilha, List<int> colunas)
        {
            colunas.ForEach(x =>
            {
                planilha.Column(x).Width = 18;
            });
        }

        internal static void FormatarComoNumero(ExcelRange celula, bool menorQueZero)
        {
            if (menorQueZero)
                celula.Style.Numberformat.Format = "_-R$* #,##0.00_-";
            else
                celula.Style.Numberformat.Format = "R$ #,##0.00";

            celula.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        }

        internal static void DefinirComoFiltro(this ExcelWorksheet planilha)
        {
            int rowCount = planilha.Dimension.Rows;
            int colCount = planilha.Dimension.Columns;
            planilha.Cells[1, 1, rowCount, colCount].AutoFilter = true;
            planilha.Cells.AutoFitColumns();
        }

        internal static string Captilize(this string texto)
        {
            return texto.First().ToString().ToUpper() + texto[1..].ToLower();
        }

        internal static int ObterCodigoAsciiLetra(char letra)
        {
            if (char.IsLetter(letra))
            {
                bool isMaiuscula = char.IsUpper(letra);

                int codigoAscii = letra;

                if ((isMaiuscula && codigoAscii > 'Z') || (!isMaiuscula && codigoAscii > 'z'))
                {
                    codigoAscii = isMaiuscula ? 'A' : 'a';
                }

                return codigoAscii;
            }

            return letra;
        }

        internal static int ObterAnoDaPlanilha(string nomePlanilha)
        {
            if (int.TryParse(nomePlanilha.AsSpan(nomePlanilha.Length - 4), out int ano))
            {
                return ano;
            }

            return 0;
        }

        internal static string CriarArquivo(string pastaDeDestino, string nomeDoArquivo)
        {
            if (!Directory.Exists(pastaDeDestino))
                Directory.CreateDirectory(pastaDeDestino);

            string excelFilePath = Path.Combine(pastaDeDestino, nomeDoArquivo);
            if (File.Exists(excelFilePath))
                File.Delete(excelFilePath);

            return excelFilePath;
        }

        internal static void CriarTotalizador(ExcelWorksheet planilha, List<TotalizadorDto> listaCelulas)
        {
            listaCelulas.ForEach(x =>
            {
                var celulaDescricao = x.Descricao;
                var celulaValor = x.Totalizador;

                if (celulaValor != null && celulaDescricao != null)
                {
                    ExcelRange totalizadorDescricao = planilha.Cells[$"{(char) celulaDescricao.Coluna}{celulaDescricao.Linha}"];
                    totalizadorDescricao.Value = "Total geral";
                    FormatarCelulasDestaque(totalizadorDescricao, false, false);

                    ExcelRange totalizadorValor = planilha.Cells[$"{(char) celulaValor.Coluna}{celulaValor.Linha}"];
                    totalizadorValor.Formula = "SUM(E:E)";
                    FormatarCelulasDestaque(totalizadorValor, true, false);
                }
            });
        }

        internal static void CriarTotalizadoresPorCategoria(List<TotalizadorCategoria> totaisPorCategoria, ExcelWorksheet planilha, out int linhaTotalizadores)
        {
            linhaTotalizadores = 2;
            int linhaTotalizadoresCopy = linhaTotalizadores;

            totaisPorCategoria.OrderBy(c => c.Categoria).ToList()
            .ForEach(x =>
            {
                ExcelRange celulaCategoriaAtual = planilha.Cells[linhaTotalizadoresCopy, 7];
                ExcelRange celulaTotalizadorAtual = planilha.Cells[linhaTotalizadoresCopy, 8];

                celulaCategoriaAtual.Value = $"Total {x.Categoria}";
                FormatarCelulasDestaque(celulaCategoriaAtual, false, false);

                celulaTotalizadorAtual.Formula = $"SUMIFS(E:E, C:C, \"{x.Categoria}\")";
                FormatarCelulasDestaque(celulaTotalizadorAtual, true, false);

                linhaTotalizadoresCopy++;
            });

            linhaTotalizadores = linhaTotalizadoresCopy;
        }

        internal static List<TotalizadorCategoria> AtualizarCategoria(List<TotalizadorCategoria> totaisPorCategoria, string categoria, decimal valor)
        {
            if (!string.IsNullOrWhiteSpace(categoria))
            {
                var categoriaExistente = totaisPorCategoria.Find(x => x.Categoria == categoria);

                if (categoriaExistente != null)
                {
                    categoriaExistente.Valor += valor;
                }
                else
                {
                    totaisPorCategoria.Add(new(categoria, valor));
                }
            }

            return totaisPorCategoria;
        }

        internal static List<TotalizadorAno> MontarDadosTotalizadores(List<ConteudoDto> conteudoDtos)
        {
            var resultado = conteudoDtos.GroupBy(x => new { x.Data.Year, x.Categoria })
                                         .Select(grupo => new TotalizadorAno
                                         {
                                             Categoria = grupo.Key.Categoria,
                                             Valor = grupo.Sum(x => x.Valor),
                                             Ano = grupo.Key.Year.ToString()
                                         })
                                         .OrderByDescending(x => x.Ano)
                                         .ThenByDescending(x => x.Valor)
                                         .ToList();

            return resultado;
        }

        internal static List<TotalizadorCategoria> CriarDados(ExcelWorksheet planilha, List<Fatura> faturas)
        {
            List<TotalizadorCategoria> totaisPorCategoria = new();
            _linhaAtual = 2;
            _codigoColuna = ObterCodigoAsciiLetra('A');
            faturas.ForEach(x =>
            {
                CriarCelulaMes(planilha, x.Data);
                CriarCelulaAno(planilha, x.Data);
                CriarCelulaCategoria(planilha, x.Categoria);
                CriarCelulaDescricao(planilha, x.Descricao);
                CriarCelulaValor(planilha, x.Valor);

                totaisPorCategoria = AtualizarCategoria(totaisPorCategoria, x.Categoria, x.Valor);
                _linhaAtual++;
                _codigoColuna = ObterCodigoAsciiLetra('A');
            });

            return totaisPorCategoria;
        }

        private static void CriarCelulaMes(ExcelWorksheet planilha, DateTime data)
        {
            var celulaData = planilha.Cells[$"{(char) _codigoColuna}{_linhaAtual}"];
            celulaData.Value = data.Month;
            _codigoColuna++;
        }

        private static void CriarCelulaAno(ExcelWorksheet planilha, DateTime data)
        {
            var celulaData = planilha.Cells[$"{(char) _codigoColuna}{_linhaAtual}"];
            celulaData.Value = data.Year;
            _codigoColuna++;
        }

        private static void CriarCelulaCategoria(ExcelWorksheet planilha, string categoria)
        {
            var celulaCategoria = planilha.Cells[$"{(char) _codigoColuna}{_linhaAtual}"];
            celulaCategoria.Value = categoria;
            _codigoColuna++;
        }

        private static void CriarCelulaDescricao(ExcelWorksheet planilha, string descricao)
        {
            var celulaDescricao = planilha.Cells[$"{(char) _codigoColuna}{_linhaAtual}"];
            celulaDescricao.Value = descricao;
            _codigoColuna++;
        }

        private static void CriarCelulaValor(ExcelWorksheet planilha, decimal valor)
        {
            var celulaValor = planilha.Cells[$"{(char) _codigoColuna}{_linhaAtual}"];
            celulaValor.Value = valor;
            FormatarComoNumero(celulaValor, (valor < 0));
            _codigoColuna++;
        }
    }
}