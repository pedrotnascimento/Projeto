﻿namespace Application.Exceptions.FolhaDePontoExceptions
{
    public class TimeAllocationLimitException : Exception
    {
        public TimeAllocationLimitException(): base("Não pode alocar tempo maior que o tempo trabalhado no dia")
        {

        }
    }
}
