using ClipShare.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ClipShare.Client.Services;

public interface IClipStore
{
    Task ClearStore();

    Task<Clip?> CreateClip(string clipContent);

    Task<ArchiveFolder?> CreateFolder(string folderName);

    Task<bool> DeleteClip(int clipId);

    Task<bool> DeleteFolder(int folderId);

    Task<IEnumerable<Clip>> GetArchivedClips();

    Task<IEnumerable<ArchiveFolder>> GetFolders();
    Task<IEnumerable<Clip>> GetUnarchivedClips();
    Task<bool> UpdateClip(Clip clip);
}

public class ClipStore : IClipStore
{
    private readonly ConcurrentDictionary<int, Clip> _clips = new();
    private readonly ConcurrentDictionary<int, ArchiveFolder> _folders = new();
    private readonly IServiceScopeFactory _scopeFactory;

    public ClipStore(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task ClearStore()
    {
        _clips.Clear();
        _folders.Clear();
        return Task.CompletedTask;
    }

    public async Task<Clip?> CreateClip(string clipContent)
    {
        return await UsingApiClient(async apiClient =>
        {
            var newClip = await apiClient.CreateClip(clipContent);
            if (newClip is not null)
            {
                _clips.AddOrUpdate(newClip.Id, newClip, (k, v) => newClip);
                return newClip;
            }
            return null;
        });
    }

    public async Task<ArchiveFolder?> CreateFolder(string folderName)
    {
        return await UsingApiClient(async apiClient =>
        {
            var newFolder = await apiClient.CreateFolder(folderName);
            if (newFolder is not null)
            {
                _folders.AddOrUpdate(newFolder.Id, newFolder, (k, v) => newFolder);
                return newFolder;
            }
            return null;
        });
    }

    public async Task<bool> DeleteClip(int clipId)
    {
        return await UsingApiClient(async apiClient =>
        {
            var result = await apiClient.DeleteClip(clipId);
            if (result)
            {
                _ = _clips.TryRemove(clipId, out _);
                return true;
            }

            return false;
        });
    }

    public async Task<bool> DeleteFolder(int folderId)
    {
        return await UsingApiClient(async apiClient =>
        {
            var result = await apiClient.DeleteFolder(folderId);
            if (result)
            {
                _ = _folders.TryRemove(folderId, out _);
                return true;
            }

            return false;
        });
    }
    public async Task<IEnumerable<Clip>> GetArchivedClips()
    {
        return await UsingApiClient(async apiClient =>
        {
            if (!_clips.Any())
            {
                var clips = await apiClient.GetClips();
                if (clips.Any())
                {
                    foreach (var clip in clips)
                    {
                        _clips.AddOrUpdate(clip.Id, clip, (k, v) => clip);
                    }
                }
            }
            return _clips.Values.Where(x => x.ArchiveFolderId is not null);
        });

    }

    public async Task<IEnumerable<ArchiveFolder>> GetFolders()
    {
        return await UsingApiClient(async apiClient =>
        {
            if (!_folders.Any())
            {
                var folders = await apiClient.GetFolders();
                if (folders.Any())
                {
                    foreach (var folder in folders)
                    {
                        _folders.AddOrUpdate(folder.Id, folder, (k, v) => folder);
                    }
                }
            }
            return _folders.Values;
        });
    }

    public async Task<IEnumerable<Clip>> GetUnarchivedClips()
    {
        return await UsingApiClient(async apiClient =>
        {
            if (!_clips.Any())
            {
                var clips = await apiClient.GetClips();
                if (clips.Any())
                {
                    foreach (var clip in clips)
                    {
                        _clips.AddOrUpdate(clip.Id, clip, (k, v) => clip);
                    }
                }
            }
            return _clips.Values.Where(x => x.ArchiveFolderId is null);
        });
    }

    public async Task<bool> UpdateClip(Clip clip)
    {
        return await UsingApiClient(async apiClient =>
        {
            return await apiClient.UpdateClip(clip);
        });
    }

    private async Task<T> UsingApiClient<T>(Func<IApiClient, Task<T>> func)
    {
        using var scope = _scopeFactory.CreateScope();
        var apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

        return await func(apiClient);
    }
}
