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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Contents { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        public ClipsUser User { get; set; }
    }
}
