using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Server.Models
{
    public class LogEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public LogLevel LogLevel { get; set; }
        public string EventId { get; set; }
        public string State { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStack { get; set; }
        public string ScopeStack { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
