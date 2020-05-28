using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ClipShare.Shared.Models
{
    public class Clip
    {
        [StringLength(5_000)]
        public string Content { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public ClipsUser User { get; set; }

        public string UserId { get; set; }

        public ArchiveFolder ArchiveFolder { get; set; }

        public int? ArchiveFolderId { get; set; }
    }
}
