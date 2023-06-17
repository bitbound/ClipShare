using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClipShare.Client;
using ClipShare.Server.Data;
using ClipShare.Server.Services;
using ClipShare.Shared.Models;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDatabaseDeveloperPageExceptionFilter();
//services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        Configuration.GetConnectionString("DefaultConnection")));

services.AddDbContext<AppDb>(options =>
   options.UseSqlite(
       builder.Configuration.GetConnectionString("SQLite")));

services
    .AddDefaultIdentity<ClipsUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDb>();

services.AddIdentityServer()
    .AddApiAuthorization<ClipsUser, AppDb>();

services.AddAuthentication()
    .AddIdentityServerJwt();

services.AddControllersWithViews();
services.AddRazorPages();

services.AddScoped<IDataService, DataService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.Use(async (ctx, next) =>
    {
        ctx.SetIdentityServerOrigin(app.Configuration["PublicOrigin"]);
        await next();
    });
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    try
    {
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDb>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Debug.Fail("Failed to migrate database.", ex.Message);
    }
}

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
loggerFactory.AddProvider(new DbLoggerProvider(app.Environment, app.Services));

await app.RunAsync();