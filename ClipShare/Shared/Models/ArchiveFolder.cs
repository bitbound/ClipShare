using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ClipShare.Shared.Models;

public class ArchiveFolder
{
    public const int MaxNameLength = 300;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [JsonIgnore]
    public List<Clip> Clips { get; set; } = new();

    [StringLength(MaxNameLength)]
    public string Name { get; set; } = string.Empty;

    public ClipsUser User { get; set; } = null!;

    public string UserId { get; set; } = null!;
}
