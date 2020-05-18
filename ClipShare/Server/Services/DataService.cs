using ClipShare.Server.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Server.Services
{
    public interface IDataService
    {
        void WriteLog(LogLevel logLevel, EventId eventId, string state, Exception exception, List<string> scopeStack);
    }

    public class DataService : IDataService
    {
       public DataService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private ApplicationDbContext DbContext { get; }

        public void WriteLog(LogLevel logLevel, EventId eventId, string state, Exception exception, List<string> scopeStack)
        {
            DbContext.Logs.Add(new Models.LogEntry()
            {
                EventId = eventId.ToString(),
                ExceptionMessage = exception?.Message,
                ExceptionStack = exception?.StackTrace,
                LogLevel = logLevel,
                ScopeStack = string.Join(",", scopeStack),
                State = state,
                Timestamp = DateTimeOffset.Now
            });
            DbContext.SaveChanges();
        }
    }
}
