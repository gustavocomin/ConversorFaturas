﻿namespace Financeiro.Domain.Exceptions.Db
{
    public class DbConnectException : Exception
    {
        public DbConnectException(string? message) : base(message)
        {
        }
    }
}