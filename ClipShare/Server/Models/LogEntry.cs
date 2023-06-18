using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Server.Models;

public class LogEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Category { get; set; } = string.Empty;

    public string EventId { get; set; } = string.Empty;

    public string ExceptionMessage { get; set; } = string.Empty;

    public string ExceptionStack { get; set; } = string.Empty;

    public LogLevel LogLevel { get; set; }
    public string ScopeStack { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
}
