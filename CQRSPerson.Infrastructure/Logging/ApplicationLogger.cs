using CQRSPerson.Domain.Logging;
using Microsoft.Extensions.Logging;
using System;

namespace CQRSPerson.Infrastructure.Logging
{
    public class ApplicationLogger<T> : IApplicationLogger<T>
    {
        private readonly ILogger applicationLogger;
        public ApplicationLogger(ILogger<T> logger)
        {
            applicationLogger = logger;
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            applicationLogger.LogError(exception, message, args);
        }
        
        public void LogError(string message)
        {
            applicationLogger.LogError(message);
        }
        public void LogInformation(string message, params object[] args)
        {
            applicationLogger.LogInformation(message, args);
        }
    }
}
