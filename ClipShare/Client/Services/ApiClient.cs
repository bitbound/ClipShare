using ClipShare.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ClipShare.Client.Services;

public interface IApiClient
{
    Task<Clip?> CreateClip(string clipContent);
    Task<ArchiveFolder?> CreateFolder(string folderName);
    Task<bool> DeleteClip(int clipId);
    Task<bool> DeleteFolder(int folderId);

    Task<IEnumerable<Clip>> GetClips();
    Task<IEnumerable<ArchiveFolder>> GetFolders();
    Task<bool> UpdateClip(Clip clip);
}
public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private readonly ILogger<ApiClient> _logger;

    public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
    {
        _http = httpClient;
        _logger = logger;
    }

    public async Task<Clip?> CreateClip(string clipContent)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("Clips", clipContent);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Clip>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating clip.");
        }
        return null;
    }

    public async Task<ArchiveFolder?> CreateFolder(string folderName)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("Archives", folderName);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ArchiveFolder>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating folder.");
        }
        return null;
    }

    public async Task<bool> DeleteClip(int clipId)
    {
        try
        {
            var response = await _http.DeleteAsync($"Clips?id={clipId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting clip.");
            return false;
        }
    }

    public async Task<bool> DeleteFolder(int folderId)
    {
        try
        {
            var response = await _http.DeleteAsync($"Archives?folderId={folderId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting folder.");
            return false;
        }
    }

    public async Task<IEnumerable<Clip>> GetClips()
    {
        try
        {
            var clips = await _http.GetFromJsonAsync<Clip[]>("Clips");
            return clips ?? Array.Empty<Clip>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting clips.");
            return Array.Empty<Clip>();
        }
    }

    public async Task<IEnumerable<ArchiveFolder>> GetFolders()
    {
        try
        {
            var clips = await _http.GetFromJsonAsync<ArchiveFolder[]>("Archives");
            return clips ?? Array.Empty<ArchiveFolder>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting folders.");
            return Array.Empty<ArchiveFolder>();
        }
    }

    public async Task<bool> UpdateClip(Clip clip)
    {
        try
        {
            var response = await _http.PutAsJsonAsync("Clips", clip, System.Threading.CancellationToken.None);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating clip.");
        }
        return false;
    }
}
