using ClipShare.Server.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClipShare.Tests;

[TestClass]
public class DbLoggerTests
{
    [TestMethod]
    public async Task LogTests()
    {
        var logger = IoCActivator.ServiceProvider.GetRequiredService<ILogger<DbLoggerTests>>();
        var db = IoCActivator.ServiceProvider.GetRequiredService<IDataService>();
        using (logger.BeginScope("Scope1"))
        {
            using (logger.BeginScope("Scope2"))
            {
                logger.LogInformation("Test message 2.");
            }
            logger.LogError(new Exception("Test error."), "Test message 1.");
        }
        var logs = await db.GetLogs();

        Assert.AreEqual(LogLevel.Information, logs[0].LogLevel);
        Assert.AreEqual("Test message 2.", logs[0].State);
        Assert.AreEqual("Scope2,Scope1", logs[0].ScopeStack);

        Assert.AreEqual(LogLevel.Error, logs[1].LogLevel);
        Assert.AreEqual("Test message 1.", logs[1].State);
        Assert.AreEqual("Scope1", logs[1].ScopeStack);
        Assert.AreEqual("Test error.", logs[1].ExceptionMessage);
    }
}
