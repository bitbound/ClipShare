using ClipShare.Server.Data;
using ClipShare.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ClipShare.Server.Services;

public interface IDataService
{
    Task<ArchiveFolder?> AddArchiveFolder(string archiveFolderName, string userId);

    Task<Clip?> AddClip(string content, string userId);

    Task DeleteArchive(int id, string userId);

    Task DeleteClip(int clipId, string userId);

    Task<IEnumerable<ArchiveFolder>> GetArchiveFolders(string userId);

    Task<IEnumerable<Clip>> GetClips(string userId);

    Task<List<Models.LogEntry>> GetLogs();

    Task UpdateClip(Clip clip, string userId);

    void WriteLog(LogLevel logLevel, string category, EventId eventId, string state, Exception? exception, List<string> scopeStack);
}

public class DataService : IDataService
{
   public DataService(AppDb dbContext)
    {
        DbContext = dbContext;
    }

    private AppDb DbContext { get; }

    public async Task<ArchiveFolder?> AddArchiveFolder(string archiveFolderName, string userId)
    {
        if (archiveFolderName.Length > ArchiveFolder.MaxNameLength)
        {
            throw new InvalidOperationException($"Folder name is longer than the max allowed ({ArchiveFolder.MaxNameLength}).");
        }


        var user = DbContext.Users
           .Include(x => x.ArchiveFolders)
           .FirstOrDefault(x => x.Id == userId);

        if (user != null)
        {
            var newFolder = new ArchiveFolder()
            {
                Name = archiveFolderName,
                User = user,
                UserId = user.Id
            };

            user.ArchiveFolders.Add(newFolder);
            await DbContext.SaveChangesAsync();

            return newFolder;
        }

        return null;
    }

    public async Task<Clip?> AddClip(string content, string userId)
    {
        if (content.Length > Clip.MaxContentLength)
        {
            throw new InvalidOperationException($"Clip content length is larger than the max allowed ({Clip.MaxContentLength}).");
        }

        var user = DbContext.Users
            .Include(x => x.Clips)
            .FirstOrDefault(x => x.Id == userId);

        if (user != null)
        {
            var newClip = new Clip()
            {
                Content = content,
                Timestamp = DateTimeOffset.Now,
                UserId = user.Id,
                User = user
            };

            user.Clips.Add(newClip);
            await DbContext.SaveChangesAsync();

            return newClip;
        }

        return null;
    }

    public async Task DeleteArchive(int archiveId, string userId)
    {
        var user = DbContext.Users
          .Include(x => x.ArchiveFolders)
          .ThenInclude(x => x.Clips)
          .FirstOrDefault(x => x.Id == userId);

        var archive = user?.ArchiveFolders?.FirstOrDefault(x => x.Id == archiveId);
        if (archive == null)
        {
            return;
        }
        if (archive.Clips?.Any() == true)
        {
            DbContext.Clips.RemoveRange(archive.Clips);
        }
        DbContext.ArchiveFolders.Remove(archive);
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteClip(int clipId, string userId)
    {
        var user = DbContext.Users
            .Include(x => x.Clips)
            .FirstOrDefault(x => x.Id == userId);

        user?.Clips?.RemoveAll(x => x.Id == clipId);
        await DbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ArchiveFolder>> GetArchiveFolders(string userId)
    {
        var user = await DbContext.Users
            .AsNoTracking()
            .Include(x => x.ArchiveFolders)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user?.ArchiveFolders is null)
        {
            return Array.Empty<ArchiveFolder>();    
        }
        
        return user.ArchiveFolders.ToArray();
    }

    public async Task<IEnumerable<Clip>> GetClips(string userId)
    {
        var clips = DbContext.Users
            .AsNoTracking()
            .Include(x => x.Clips)
            .ThenInclude(x => x.ArchiveFolder)
            .FirstOrDefault(x => x.Id == userId)
            ?.Clips;

        if (clips == null)
        {
            return Array.Empty<Clip>();
        }

        clips.RemoveAll(x => DateTimeOffset.Now - x.Timestamp > TimeSpan.FromDays(30));

        await DbContext.SaveChangesAsync();

        return clips;
    }

    public async Task<List<Models.LogEntry>> GetLogs()
    {
        return await DbContext.Logs.ToListAsync();
    }

    public async Task UpdateClip(Clip clip, string userId)
    {
        if (clip.Content.Length > Clip.MaxContentLength)
        {
            throw new InvalidOperationException($"Clip content length is larger than the max allowed ({Clip.MaxContentLength}).");
        }

        var user = DbContext.Users
              .Include(x => x.Clips)
              .FirstOrDefault(x => x.Id == userId);

        if (user != null)
        {
            var savedClip = user.Clips.FirstOrDefault(x => x.Id == clip.Id);
            if (savedClip is null)
            {
                return;
            }

            savedClip.Content = clip.Content;
            savedClip.ArchiveFolderId = clip.ArchiveFolderId;
            await DbContext.SaveChangesAsync();
        }
    }

    public void WriteLog(LogLevel logLevel, string category, EventId eventId, string state, Exception? exception, List<string> scopeStack)
    {
        // Prevent re-entrancy.
        if (eventId.Name?.Contains("EntityFrameworkCore") == true)
        {
            return;
        }

        var expiredLogs = DbContext.Logs
            .ToList()
            .Where(x => DateTime.Now - x.Timestamp > TimeSpan.FromDays(30));

        if (expiredLogs?.Any() == true)
        {
            DbContext.Logs.RemoveRange(expiredLogs);
        }

        DbContext.Logs.Add(new Models.LogEntry()
        {
            Category = category,
            EventId = eventId.ToString(),
            ExceptionMessage = exception?.Message ?? string.Empty,
            ExceptionStack = exception?.StackTrace ?? string.Empty,
            LogLevel = logLevel,
            ScopeStack = string.Join(",", scopeStack),
            State = state,
            Timestamp = DateTimeOffset.Now
        });
        DbContext.SaveChanges();
    }
}
