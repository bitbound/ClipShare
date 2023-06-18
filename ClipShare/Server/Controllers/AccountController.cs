using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClipShare.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    [HttpGet("[action]")]
    public bool IsLoggedIn()
    {
        return User.Identity?.IsAuthenticated == true;
    }
}