using ClipShare.Server.Models;
using ClipShare.Shared.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Server.Data;

public class AppDb : ApiAuthorizationDbContext<ClipsUser>
{
    public AppDb(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    public DbSet<LogEntry> Logs { get; set; }
    public DbSet<Clip> Clips { get; set; }
    public DbSet<ArchiveFolder> ArchiveFolders { get; set; }

    public new DbSet<ClipsUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ClipsUser>().ToTable("ClipsUsers");

        builder.Entity<ClipsUser>()
            .HasMany(x => x.Clips)
            .WithOne(x => x.User);
        builder.Entity<ClipsUser>()
            .HasMany(x => x.ArchiveFolders)
            .WithOne(x => x.User);

      
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the SQLite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToBinaryConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
            // This only supports millisecond precision, but should be sufficient for most use cases.
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                {
                    continue;
                }
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                            || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties)
                {
                    builder
                         .Entity(entityType.Name)
                         .Property(property.Name)
                         .HasConversion(new DateTimeOffsetToStringConverter());
                }
            }
        }
    }
}
