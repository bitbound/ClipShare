using ClipShare.Server.Services;
using ClipShare.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipShare.Tests
{
    [TestClass]
    public class DataServiceTests
    {
        private IDataService DataService { get; set; }

        [TestInitialize]
        public async Task TestInit()
        {
            await TestData.PopulateTestData();
            DataService = IoCActivator.ServiceProvider.GetRequiredService<IDataService>();
        }

        [TestMethod]
        [DoNotParallelize]
        public async Task AddClip()
        {
            // Both users should have no clips.
            var clips1 = await DataService.GetClips(TestData.User1.Id);
            Assert.AreEqual(0, clips1.Count());

            var clips2 = await DataService.GetClips(TestData.User2.Id);
            Assert.AreEqual(0, clips2.Count());

            // Add clip for user 1.
            await DataService.AddClip("Test content 1.", TestData.User1.Id);

            // User 1 should have one clip now.
            clips1 = await DataService.GetClips(TestData.User1.Id);
            Assert.AreEqual(1, clips1.Count());
            Assert.AreEqual("Test content 1.", clips1.First().Content);

            // User 2 should still have no clips.
            clips2 = await DataService.GetClips(TestData.User2.Id);
            Assert.AreEqual(0, clips2.Count());
        }

        [TestMethod]
        [DoNotParallelize]
        public async Task AddClip_TooLarge()
        {
            var clipContent = "";
            while (clipContent.Length < Clip.MaxContentLength)
            {
                clipContent += Guid.NewGuid().ToString();
            }
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => DataService.AddClip(clipContent, TestData.User1.Id));
        }

        [TestMethod]
        [DoNotParallelize]
        public async Task DeleteClip()
        {
            var clips = await DataService.GetClips(TestData.User1.Id);
            Assert.AreEqual(0, clips.Count());

            await DataService.AddClip("Test content 1.", TestData.User1.Id);

            clips = await DataService.GetClips(TestData.User1.Id);
            Assert.AreEqual(1, clips.Count());

            // User the wrong user ID.
            await DataService.DeleteClip(clips.First().Id, TestData.User2.Id);

            // Clip should still be there.
            await DataService.GetClips(TestData.User1.Id);
            Assert.AreEqual(1, clips.Count());

            // User the correct user.
            await DataService.DeleteClip(clips.First().Id, TestData.User1.Id);

            // Clip should be deleted now.
            await DataService.GetClips(TestData.User1.Id);
            Assert.AreEqual(0, clips.Count());
        }

        [TestMethod]
        [DoNotParallelize]
        public async Task UpdateClip()
        {
            await DataService.AddClip("Test content 1.", TestData.User1.Id);
            var clips = await DataService.GetClips(TestData.User1.Id);
            clips.First().Content = "Different content.";
            await DataService.UpdateClip(clips.First(), TestData.User1.Id);
            clips = await DataService.GetClips(TestData.User1.Id);
            Assert.AreEqual("Different content.", clips.First().Content);
        }

        [TestMethod]
        [DoNotParallelize]
        public async Task AddArchiveFolder()
        {
            await DataService.AddArchiveFolder("Backup Folder", TestData.User1.Id);
            var folders = DataService.GetArchiveFolders(TestData.User1.Id);
            Assert.AreEqual("Backup Folder", folders.First().Name);
        }

        [TestMethod]
        [DoNotParallelize]
        public async Task AddClipToFolder()
        {
            await DataService.AddArchiveFolder("Backup Folder", TestData.User1.Id);
            var folders = DataService.GetArchiveFolders(TestData.User1.Id);
            
            await DataService.AddClip("Test content 1.", TestData.User1.Id);

            var clips = await DataService.GetClips(TestData.User1.Id);
            clips.First().ArchiveFolderId = folders.First().Id;
            await DataService.UpdateClip(clips.First(), TestData.User1.Id);

            clips = await DataService.GetClips(TestData.User1.Id);

            Assert.AreEqual("Backup Folder", clips.First().ArchiveFolder.Name);
        }

        [TestMethod]
        [DoNotParallelize]
        public async Task DeleteArchiveFolder()
        {
            await DataService.AddArchiveFolder("Backup Folder", TestData.User1.Id);
            var folders = DataService.GetArchiveFolders(TestData.User1.Id);

            await DataService.AddClip("Test content 1.", TestData.User1.Id);

            var clips = await DataService.GetClips(TestData.User1.Id);
            clips.First().ArchiveFolderId = folders.First().Id;
            await DataService.UpdateClip(clips.First(), TestData.User1.Id);

            clips = await DataService.GetClips(TestData.User1.Id);

            Assert.AreEqual("Backup Folder", clips.First().ArchiveFolder.Name);

            await DataService.DeleteArchive(folders.First().Id, TestData.User1.Id);

            Assert.AreEqual(0, (await DataService.GetClips(TestData.User1.Id)).Count());
            Assert.AreEqual(0, (DataService.GetArchiveFolders(TestData.User1.Id)).Count());
        }
    }
}
