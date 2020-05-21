using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClipShare.Shared.Models
{
    public class Clip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Contents { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Username { get; set; }
    }
}
