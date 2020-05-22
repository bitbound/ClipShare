using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Shared.Models
{
    public class ClipsUser : IdentityUser
    {
        public List<Clip> Clips { get; set; }
    }
}
