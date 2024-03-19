namespace Financeiro.Domain.Exceptions.Fatura
{
    public class FaturaException : Exception
    {
        public FaturaException(string? message) : base(message)
        {
        }
    }
}