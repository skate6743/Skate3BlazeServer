using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using Servers.HTTP.Models;
using Servers.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servers.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserDbData> Users { get; set; }
        public DbSet<FilesDbData> Files { get; set; }

        public DbSet<Bookmark> Bookmarks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={Path.Combine(ServerGlobals.BaseDirectory, "skate.db")}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDbData>()
                .HasKey(u => u.BlazeId);

            modelBuilder.Entity<UserDbData>()
                .HasIndex(u => new { u.PsnId, u.Platform })
                .IsUnique();

            modelBuilder.Entity<FilesDbData>()
                .HasKey(f => f.FileId);

            modelBuilder.Entity<Rating>()
                .HasOne<FilesDbData>()
                .WithMany(f => f.Ratings)
                .HasForeignKey(r => r.FileId);

            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => new { b.UserId, b.FileId })
                .IsUnique();
        }
    }

    public class UserDbData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint BlazeId { get; set; }

        public ulong PsnId { get; set; }
        public UserPlatform Platform { get; set; }
        public string DisplayName { get; set; }
    }

    public class FilesDbData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }

        public FileType Type { get; set; }
        public string UploaderName { get; set; }
        public string Description { get; set; }
        public uint UploaderId { get; set; }
        public string Tags { get; set; }
        public int DownloadCount { get; set; }
        public long CreateDate { get; set; }
        public long LocationId { get; set; }
        public List<Rating> Ratings { get; set; }
        public string FileHash { get; set; }
    }

    public class Rating
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingId { get; set; }

        public int FileId { get; set; }
        public uint UserId { get; set; }
        public float Stars { get; set; }
    }

    public class Bookmark
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookmarkId { get; set; }
        public uint UserId { get; set; }
        public int FileId { get; set; }
    }
}