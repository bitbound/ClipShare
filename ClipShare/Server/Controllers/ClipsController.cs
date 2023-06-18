using ClipShare.Server.Services;
using ClipShare.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClipShare.Server.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ClipsController : ControllerBase
{
    private readonly IDataService _dataService;

    public ClipsController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }
        await _dataService.DeleteClip(id, userId);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Clip>>> Get()
    {
        var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }
        var clips = await _dataService.GetClips(userId);
        return Ok(clips);
    }

    [HttpPost]
    public async Task<ActionResult<Clip?>> Post([FromBody]string clipContents)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }
        return await _dataService.AddClip(clipContents, userId);
    }
    [HttpPut]
    public async Task<IActionResult> Put([FromBody]Clip clip)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }
        await _dataService.UpdateClip(clip, userId);
        return NoContent();
    }
}
