using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClipShare.Shared.Models;

public class ClipsUser : IdentityUser
{
    [JsonIgnore]
    public List<Clip> Clips { get; set; } = new();

    [JsonIgnore]
    public List<ArchiveFolder> ArchiveFolders { get; set; } = new();
}
