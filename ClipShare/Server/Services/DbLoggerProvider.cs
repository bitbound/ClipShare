using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Server.Services
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly IDataService dataService;
        private readonly IWebHostEnvironment hostEnvironment;

        public DbLoggerProvider(IDataService dataService, IWebHostEnvironment hostEnvironment)
        {
            this.dataService = dataService;
            this.hostEnvironment = hostEnvironment;
        }


        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(categoryName, dataService, hostEnvironment);
        }

        public void Dispose()
        {
        }
    }
}
