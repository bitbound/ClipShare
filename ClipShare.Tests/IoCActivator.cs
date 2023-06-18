using ClipShare.Server.Data;
using ClipShare.Server.Services;
using ClipShare.Shared.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClipShare.Tests;

[TestClass]
public class IoCActivator
{
    public static IServiceProvider ServiceProvider { get; set; } = null!;
    private static IHostBuilder? _builder;


    public static void Activate()
    {
        if (_builder is null)
        {
            _builder = Host.CreateDefaultBuilder(Array.Empty<string>())
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.CaptureStartupErrors(true);
                 webBuilder.UseStartup<Startup>();
             });

            _builder.Build();

          
        }
    }


    [AssemblyInitialize]
    public static void AssemblyInit(TestContext _)
    {
        Activate();
    }
}

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDb>(options =>
         options.UseInMemoryDatabase("Remotely"));

        services.AddIdentity<ClipsUser, IdentityRole>(options => options.Stores.MaxLengthForKeys = 128)
         .AddEntityFrameworkStores<AppDb>()
         .AddDefaultUI()
         .AddDefaultTokenProviders();

        services.AddTransient<IDataService, DataService>();
        IoCActivator.ServiceProvider = services.BuildServiceProvider();

        var loggerFactory = IoCActivator.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var env = IoCActivator.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        loggerFactory.AddProvider(new DbLoggerProvider(env, IoCActivator.ServiceProvider));
    }

}
