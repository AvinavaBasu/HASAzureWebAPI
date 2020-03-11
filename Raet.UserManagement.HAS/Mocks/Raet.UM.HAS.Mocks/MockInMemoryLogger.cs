using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raet.UM.HAS.Mocks
{
    public class MockInMemoryLogger : ILogger
    {
        public Queue<LogResult> Logs { get; private set; }

        public MockInMemoryLogger()
        {
            Logs = new Queue<LogResult>();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Logs.Enqueue(new LogResult(logLevel, state.ToString()));
        }

        public class LogResult
        {
            public LogLevel LogLevel { get; }

            public string Message { get; }

            public LogResult(LogLevel logLevel, string message)
            {
                LogLevel = logLevel;
                Message = message;
            }
        }
    }
}
