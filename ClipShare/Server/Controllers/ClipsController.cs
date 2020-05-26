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

namespace ClipShare.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ClipsController : ControllerBase
    {
        public ClipsController(IDataService dataService)
        {
            DataService = dataService;
        }

        private IDataService DataService { get; }

        [HttpGet]
        public IEnumerable<Clip> Get()
        {
            var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return DataService.GetClips(userId);
        }

        [HttpPost]
        public Task<Clip> Post([FromBody]string clipContents)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return DataService.AddClip(clipContents, userId);
        }

        [HttpDelete]
        public Task Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return DataService.DeleteClip(id, userId);
        }

        [HttpPut]
        public Task Put([FromBody]Clip clip)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return DataService.UpdateClip(clip, userId);
        }
    }
}
