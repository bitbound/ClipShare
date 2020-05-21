using ClipShare.Server.Services;
using ClipShare.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return DataService.GetClips(HttpContext.User.Identity.Name);
        }
    }
}
