using System;

namespace CQRSPerson.Domain.Logging
{
    public interface IApplicationLogger<T>
    {
        void LogError(Exception exception, string message, params object[] args);
        void LogError(string message);
        void LogInformation(string message, params object[] args);
    }
}
