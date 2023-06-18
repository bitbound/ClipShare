using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ClipShare.Shared.Models;

public class Clip
{
    public const int MaxContentLength = 5_000;

    [StringLength(MaxContentLength)]
    public string Content { get; set; } = string.Empty;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public DateTimeOffset Timestamp { get; set; }

    public ClipsUser User { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public ArchiveFolder? ArchiveFolder { get; set; }

    public int? ArchiveFolderId { get; set; }
}
