using ClipShare.Server.Data;
using ClipShare.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ClipShare.Server.Services
{
    public interface IDataService
    {
        Task<Clip> AddClip(string content, string userId);

        Task DeleteClip(int clipId, string userId);

        IEnumerable<ArchiveFolder> GetArchiveFolders(string userId);

        IEnumerable<Clip> GetClips(string userId);

        Task UpdateClip(Clip clip, string userId);

        void WriteLog(LogLevel logLevel, string category, EventId eventId, string state, Exception exception, List<string> scopeStack);
    }

    public class DataService : IDataService
    {
       public DataService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private ApplicationDbContext DbContext { get; }

        public async Task<Clip> AddClip(string content, string userId)
        {
            var user = DbContext.Users
                .Include(x => x.Clips)
                .FirstOrDefault(x => x.Id == userId);

            if (user != null)
            {
                var newClip = new Clip()
                {
                    Content = content,
                    Timestamp = DateTimeOffset.Now,
                    UserId = user.Id,
                    User = user
                };

                user.Clips.Add(newClip);
                await DbContext.SaveChangesAsync();

                return newClip;
            }

            return null;
        }

        public async Task DeleteClip(int clipId, string userId)
        {
            var user = DbContext.Users
                .Include(x => x.Clips)
                .FirstOrDefault(x => x.Id == userId);

            user?.Clips?.RemoveAll(x => x.Id == clipId);
            await DbContext.SaveChangesAsync();
        }

        public IEnumerable<Clip> GetClips(string userId)
        {
            var clips = DbContext.Users
                .Include(x => x.Clips)
                .FirstOrDefault(x => x.Id == userId)
                ?.Clips;

            if (clips == null)
            {
                return Array.Empty<Clip>();
            }
            return clips;
        }

        public IEnumerable<ArchiveFolder> GetArchiveFolders(string userId)
        {
            var archives = DbContext.Users
                 .Include(x => x.ArchiveFolders)
                 .FirstOrDefault(x => x.Id == userId)
                 ?.ArchiveFolders;

            if (archives == null)
            {
                return Array.Empty<ArchiveFolder>();
            }
            return archives;
        }

        public async Task UpdateClip(Clip clip, string userId)
        {
            var user = DbContext.Users
                  .Include(x => x.Clips)
                  .FirstOrDefault(x => x.Id == userId);

            if (user != null)
            {
                var savedClip = user.Clips.FirstOrDefault(x => x.Id == clip.Id);
                savedClip.Content = clip.Content;
                savedClip.ArchiveFolderId = clip.ArchiveFolderId;
                await DbContext.SaveChangesAsync();
            }
        }

        public void WriteLog(LogLevel logLevel, string category, EventId eventId, string state, Exception exception, List<string> scopeStack)
        {
            DbContext.Logs.Add(new Models.LogEntry()
            {
                Category = category,
                EventId = eventId.ToString(),
                ExceptionMessage = exception?.Message,
                ExceptionStack = exception?.StackTrace,
                LogLevel = logLevel,
                ScopeStack = string.Join(",", scopeStack),
                State = state,
                Timestamp = DateTimeOffset.Now
            });
            DbContext.SaveChanges();
        }
    }
}
