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

namespace ClipShare.Tests;

[TestClass]
public class DataServiceTests
{
    private IDataService _dataService = null!;

    [TestInitialize]
    public async Task TestInit()
    {
        await TestData.PopulateTestData();
        _dataService = IoCActivator.ServiceProvider.GetRequiredService<IDataService>();
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task AddClip()
    {
        // Both users should have no clips.
        var clips1 = await _dataService.GetClips(TestData.User1.Id);
        Assert.AreEqual(0, clips1.Count());

        var clips2 = await _dataService.GetClips(TestData.User2.Id);
        Assert.AreEqual(0, clips2.Count());

        // Add clip for user 1.
        await _dataService.AddClip("Test content 1.", TestData.User1.Id);

        // User 1 should have one clip now.
        clips1 = await _dataService.GetClips(TestData.User1.Id);
        Assert.AreEqual(1, clips1.Count());
        Assert.AreEqual("Test content 1.", clips1.First().Content);

        // User 2 should still have no clips.
        clips2 = await _dataService.GetClips(TestData.User2.Id);
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
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _dataService.AddClip(clipContent, TestData.User1.Id));
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task DeleteClip()
    {
        var clips = await _dataService.GetClips(TestData.User1.Id);
        Assert.AreEqual(0, clips.Count());

        await _dataService.AddClip("Test content 1.", TestData.User1.Id);

        clips = await _dataService.GetClips(TestData.User1.Id);
        Assert.AreEqual(1, clips.Count());

        // Use the wrong user ID.
        await _dataService.DeleteClip(clips.First().Id, TestData.User2.Id);

        // Clip should still be there.
        clips = await _dataService.GetClips(TestData.User1.Id);
        Assert.AreEqual(1, clips.Count());

        // Use the correct user.
        await _dataService.DeleteClip(clips.First().Id, TestData.User1.Id);

        // Clip should be deleted now.
        clips = await _dataService.GetClips(TestData.User1.Id);
        Assert.AreEqual(0, clips.Count());
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task UpdateClip()
    {
        await _dataService.AddClip("Test content 1.", TestData.User1.Id);
        var clips = await _dataService.GetClips(TestData.User1.Id);
        clips.First().Content = "Different content.";
        await _dataService.UpdateClip(clips.First(), TestData.User1.Id);
        clips = await _dataService.GetClips(TestData.User1.Id);
        Assert.AreEqual("Different content.", clips.First().Content);
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task AddArchiveFolder()
    {
        await _dataService.AddArchiveFolder("Backup Folder", TestData.User1.Id);
        var folders = await _dataService.GetArchiveFolders(TestData.User1.Id);
        Assert.AreEqual("Backup Folder", folders.First().Name);
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task AddClipToFolder()
    {
        await _dataService.AddArchiveFolder("Backup Folder", TestData.User1.Id);
        var folders = await _dataService.GetArchiveFolders(TestData.User1.Id);
        
        await _dataService.AddClip("Test content 1.", TestData.User1.Id);

        var clips = await _dataService.GetClips(TestData.User1.Id);
        clips.First().ArchiveFolderId = folders.First().Id;
        await _dataService.UpdateClip(clips.First(), TestData.User1.Id);

        clips = await _dataService.GetClips(TestData.User1.Id);

        Assert.AreEqual("Backup Folder", clips.First().ArchiveFolder?.Name);
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task DeleteArchiveFolder()
    {
        await _dataService.AddArchiveFolder("Backup Folder", TestData.User1.Id);
        var folders = await _dataService.GetArchiveFolders(TestData.User1.Id);

        await _dataService.AddClip("Test content 1.", TestData.User1.Id);

        var clips = await _dataService.GetClips(TestData.User1.Id);
        clips.First().ArchiveFolderId = folders.First().Id;
        await _dataService.UpdateClip(clips.First(), TestData.User1.Id);

        clips = await _dataService.GetClips(TestData.User1.Id);

        Assert.AreEqual("Backup Folder", clips.First().ArchiveFolder?.Name);

        await _dataService.DeleteArchive(folders.First().Id, TestData.User1.Id);

        Assert.AreEqual(0, (await _dataService.GetClips(TestData.User1.Id)).Count());
        Assert.AreEqual(0, (await _dataService.GetArchiveFolders(TestData.User1.Id)).Count());
    }
}
