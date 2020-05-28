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

namespace ClipShare.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ArchivesController : ControllerBase
    {
        public ArchivesController(IDataService dataService)
        {
            DataService = dataService;
        }

        private IDataService DataService { get; }

        [HttpGet]
        public IEnumerable<ArchiveFolder> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return DataService.GetArchiveFolders(userId);
        }
    }
}
