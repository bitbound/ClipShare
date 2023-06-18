using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClipShare.Server.Services;
using ClipShare.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClipShare.Server.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ArchivesController : ControllerBase
{
    private readonly IDataService _dataService;

    public ArchivesController(IDataService dataService)
    {
        _dataService = dataService;
    }


    [HttpDelete]
    public async Task<IActionResult> Delete(int folderId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        await _dataService.DeleteArchive(folderId, userId);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArchiveFolder>>> Get()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }
        var folders = await _dataService.GetArchiveFolders(userId);
        return Ok(folders);
    }

    [HttpPost]
    public async Task<ActionResult<ArchiveFolder?>> Post([FromBody] string archiveFolderName)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }
        return await _dataService.AddArchiveFolder(archiveFolderName, userId);
    }
}
