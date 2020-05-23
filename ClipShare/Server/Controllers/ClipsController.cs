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
        public ClipsController(IDataService dataService, UserManager<ClipsUser> userManager)
        {
            DataService = dataService;
            UserManager = userManager;
        }

        private IDataService DataService { get; }
        private UserManager<ClipsUser> UserManager { get; }

        [HttpGet]
        public IEnumerable<Clip> Get()
        {
            var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return DataService.GetClips(userId);
        }

        [HttpPost]
        public async Task<Clip> Post([FromBody]string clipContents)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await DataService.AddClip(clipContents, userId);
        }
    }
}
