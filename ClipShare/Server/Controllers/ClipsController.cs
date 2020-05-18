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
    }
}
