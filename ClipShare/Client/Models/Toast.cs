using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Client.Models
{
    public class Toast
    {
        public Toast(string guid, string message, string classString, TimeSpan expiration)
        {
            Guid = guid;
            Message = message;
            ClassString = classString;
            Expiration = expiration;
        }

        public string ClassString { get; }
        public TimeSpan Expiration { get; }
        public string Guid { get; }
        public string Message { get; }
    }
}
