using ClipShare.Server.Data;
using ClipShare.Server.Services;
using ClipShare.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipShare.Tests
{
    public class TestData
    {
        public static ClipsUser User1 { get; private set; }

        public static ClipsUser User2 { get; private set; }


        public static async Task PopulateTestData()
        {
            ClearData();

            User1 = new ClipsUser()
            {
                UserName = "testuser1@test.com"
            };
            User2 = new ClipsUser()
            {
                UserName = "testuser2@test.com"
            };

            var userManager = IoCActivator.ServiceProvider.GetRequiredService<UserManager<ClipsUser>>();


            await userManager.CreateAsync(User1);
            await userManager.CreateAsync(User2);
        }

        private static void ClearData()
        {
            var dbContext = IoCActivator.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Clips.RemoveRange(dbContext.Clips.ToList());
            dbContext.ArchiveFolders.RemoveRange(dbContext.ArchiveFolders.ToList());
            dbContext.Users.RemoveRange(dbContext.Users.ToList());
            dbContext.SaveChanges();

        }
    }
}
