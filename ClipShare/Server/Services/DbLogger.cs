using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClipShare.Server.Services
{
    public class DbLogger : ILogger
    {
        private readonly string categoryName;
        private readonly IDataService dataService;
        private readonly IWebHostEnvironment hostEnvironment;
        protected static ConcurrentStack<string> ScopeStack { get; } = new ConcurrentStack<string>();

        public DbLogger(string categoryName, IDataService dataService, IWebHostEnvironment hostEnvironment)
        {
            this.categoryName = categoryName;
            this.dataService = dataService;
            this.hostEnvironment = hostEnvironment;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            ScopeStack.Push(state.ToString());
            return new NoopDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    break;
                case LogLevel.Debug:
                case LogLevel.Information:
                    if (hostEnvironment.IsDevelopment())
                    {
                        return true;
                    }
                    break;
                case LogLevel.Warning:
                case LogLevel.Error:
                case LogLevel.Critical:
                    return true;
                case LogLevel.None:
                    break;
                default:
                    break;
            }
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            dataService.WriteLog(logLevel, eventId, state.ToString(), exception, ScopeStack.ToList());
        }


        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
                while (!ScopeStack.TryPop(out _))
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
