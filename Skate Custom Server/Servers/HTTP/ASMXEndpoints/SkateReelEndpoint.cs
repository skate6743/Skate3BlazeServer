using HttpMultipartParser;
using Microsoft.EntityFrameworkCore;
using Servers.Blaze.Models;
using Servers.Database;
using Servers.HTTP.Models;
using System.Net;
using System.Xml.Linq;

namespace Servers.HTTP.ASMXEndpoints
{
    public class SkateReelEndpoint
    {
        private static (string extension, string pathName)? TypeIdToExtension(FileType type)
        {
            switch (type)
            {
                case FileType.SKATEPARK:
                    return (".bin", "SKATEPARK");
                /*case FileType.PHOTO:
                    return (".jpg", "PHOTO");
                case FileType.VIDEO:
                    return (".flv", "VIDEO");*/
                default:
                    return null;
            }
        }


        public static async Task<int> Upload(HttpListenerContext ctx)
        {
            User? user = HttpUtils.GetAuthenticatedUser(ctx);
            if (user == null)
                return -1;

            var parser = await MultipartFormDataParser.ParseAsync(ctx.Request.InputStream);

            if (!int.TryParse(parser.GetParameterValue("typeId"), out int typeId))
                return -1;

            var fileTypeInfo = TypeIdToExtension((FileType)typeId);
            if (fileTypeInfo == null)
                return 0;

            if (!long.TryParse(parser.GetParameterValue("locationId"), out long locationId))
                return -1;

            string tags = parser.GetParameterValue("tags");
            if (tags.Length > 20)
                return -1;

            string description = parser.GetParameterValue("description");
            if (description.Length > 100)
                return -1;

            var thumbnail = parser.Files.FirstOrDefault(f => f.Name == "thumbnail");
            var file = parser.Files.FirstOrDefault(f => f.Name == "file");

            if (thumbnail == null || file == null)
                return -1;

            long totalSize = thumbnail.Data.Length + file.Data.Length;
            if (totalSize > ServerGlobals.UploadSizeCap) // Limit upload size
                return -1;

            await using var db = new AppDbContext();

            int userUploadCount = db.Files.Where(x => x.UploaderId == user.UserIdentification.BlazeId && x.Type == (FileType)typeId).Count();

            if (userUploadCount >= ServerGlobals.ContentUploadCap)
                return -1;

            using var sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(file.Data);
            string hash = Convert.ToHexString(hashBytes).ToLower();
            file.Data.Position = 0; // Reset for later copy

            // Prevent user from uploading identical park files
            if (await db.Files.AnyAsync(f => f.FileHash == hash && f.UploaderId == user.UserIdentification.BlazeId))
                return -2;

            var newFileData = new FilesDbData
            {
                UploaderId = user.UserIdentification.BlazeId,
                UploaderName = user.UserIdentification.Name,
                Type = (FileType)typeId,
                Tags = tags,
                LocationId = locationId,
                Description = description,
                CreateDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                FileHash = hash
            };

            db.Files.Add(newFileData);
            await db.SaveChangesAsync();

            string dir = Path.Combine(ServerGlobals.BaseDirectory, $"wwwroot/skate3/content/PS3/{fileTypeInfo.Value.pathName}/{user.UserIdentification.BlazeId}/{newFileData.FileId}");
            Directory.CreateDirectory(dir);

            using (var fs = File.Create($"{dir}/{newFileData.FileId}_thumb.jpg"))
                await thumbnail.Data.CopyToAsync(fs);

            using (var fs = File.Create($"{dir}/{newFileData.FileId}{fileTypeInfo.Value.extension}"))
                await file.Data.CopyToAsync(fs);

            return newFileData.FileId;
        }

        private static float GetRating(FilesDbData file)
        {
            return file.Ratings.Count > 0
                ? file.Ratings.Average(r => r.Stars)
                : 0;
        }

        public static async Task<string> GetOneContent(HttpListenerContext ctx)
        {
            int.TryParse(ctx.Request.QueryString["fileId"], out int fileId);

            if (fileId <= 0)
                return "";

            await using var db = new AppDbContext();
            FilesDbData? file = db.Files.Include(f => f.Ratings).Where(x=>x.FileId == fileId).FirstOrDefault();

            if (file == null)
                return "";

            return FileDatasToXml(new List<FilesDbData> { file }).Replace("<ContentInfoContainer>", "<ContentInfo>").Replace("</ContentInfoContainer>", "</ContentInfo>");
        }

        public static string FileDatasToXml(List<FilesDbData> Files)
        {
            var container = new XElement("ContentInfoContainer",
                Files.Select(file =>
                    {
                        var extensionInfo = TypeIdToExtension(file.Type);
                        if (extensionInfo == null)
                            return null;

                        string basePath = $"/skate3/content/PS3/{extensionInfo.Value.pathName}/{file.UploaderId}/{file.FileId}/{file.FileId}";
                        string[] tags = file.Tags.Replace(" ", "").Split(',');

                        return new XElement("content",
                            new XElement("fileId", file.FileId),
                            new XElement("fileType", extensionInfo.Value.pathName),
                            new XElement("ownerId", file.UploaderId),
                            new XElement("ownerName", file.UploaderName),
                            new XElement("locationId", file.LocationId),
                            new XElement("downloadCount", file.DownloadCount),
                            new XElement("voteCount", file.Ratings.Count),
                            new XElement("rating", GetRating(file)),
                            new XElement("rank", 1),
                            new XElement("rankType", "RECENT"),
                            new XElement("fileSize", 123),
                            tags.Length == 5 ? Enumerable.Range(0, 5).Select(i => new XElement($"tag{i + 1}", tags[i])) : null,
                            new XElement("contentUri", basePath + extensionInfo.Value.extension),
                            new XElement("thumbnailUri", basePath + "_thumb.jpg"),
                            new XElement("highResThumbnailUri", basePath + "_thumb.jpg"),
                            new XElement("createDate", file.CreateDate),
                            new XElement("teamName"),
                            new XElement("description", file.Description),
                            new XElement("authorId", file.UploaderId),
                            new XElement("authorName", file.UploaderName),
                            new XElement("authorFileId", 0)
                        );
                    })
            );

            return container.ToString(SaveOptions.DisableFormatting);
        }

        public static async Task<string> GetTopN(HttpListenerContext ctx)
        {
            var pagination = HttpUtils.ParsePagination(ctx);

            if (pagination == null)
                return "";

            int.TryParse(ctx.Request.QueryString["rankingId"], out int rankingId);

            int.TryParse(ctx.Request.QueryString["typeId"], out int typeId);

            if (typeId <= 0)
                return "";

            await using var db = new AppDbContext();

            RankingType type = (RankingType)rankingId;

            List<FilesDbData> files = db.Files
                .Include(f => f.Ratings)
                .Where(f => f.Type == (FileType)typeId)
                .ToList();

            switch (type)
            {
                case RankingType.TOPRATED:
                    files = files.OrderByDescending(f => f.Ratings.Count).ToList();
                    break;
                case RankingType.MOSTVIEWED:
                    files = files.OrderByDescending(f => f.DownloadCount).ToList();
                    break;
                case RankingType.MOSTRECENT:
                    files = files.OrderByDescending(f => f.CreateDate).ToList();
                    break;
            }

            return FileDatasToXml(files.Skip(pagination.Value.start)
                .Take(pagination.Value.end - pagination.Value.start)
                .ToList());
        }

        public static async Task<string> GetFeaturedContent(HttpListenerContext ctx)
        {
            var pagination = HttpUtils.ParsePagination(ctx);
            int.TryParse(ctx.Request.QueryString["typeId"], out int typeId);
            
            if (pagination == null || typeId <= 0)
                return "";


            await using var db = new AppDbContext();

            return FileDatasToXml(db.Files
                .Include(f => f.Ratings)
                .Where(f => f.Type == (FileType)typeId)
                .Skip(pagination.Value.start)
                .Take(pagination.Value.end - pagination.Value.start)
                .ToList());
        }

        public static async Task<string> GetContent2(HttpListenerContext ctx)
        {
            int userId = -1;
            int.TryParse(ctx.Request.QueryString["userId"], out userId);

            if (userId == -1)
                return "";

            await using var db = new AppDbContext();

            return FileDatasToXml(db.Files
                .Include(f => f.Ratings)
                .Where(f => f.Type == FileType.SKATEPARK && f.UploaderId == userId)
                .ToList());
        }

        public static async Task GetFileContent(HttpListenerContext ctx)
        {
            int.TryParse(ctx.Request.QueryString["fileId"], out int fileId);
            if (fileId <= 0) return;

            await using var db = new AppDbContext();

            var file = await db.Files.FindAsync(fileId);
            if (file == null)
                return;

            var extensionInfo = TypeIdToExtension(file.Type);
            if (extensionInfo == null)
                return;

            string path = Path.Combine(
                ServerGlobals.BaseDirectory,
                $"wwwroot/skate3/content/PS3/{extensionInfo.Value.pathName}/{file.UploaderId}/{file.FileId}/{file.FileId}{extensionInfo.Value.extension}"
            );

            if (!await HttpUtils.ServeFile(ctx, path)) return;

            // Update download count in db
            file.DownloadCount++;
            await db.SaveChangesAsync();
        }

        public static async Task<string> Delete(HttpListenerContext ctx)
        {
            using var reader = new StreamReader(ctx.Request.InputStream);
            string body = await reader.ReadToEndAsync();
            var parsed = System.Web.HttpUtility.ParseQueryString(body);

            int fileId = -1;
            int.TryParse(parsed["fileId"], out fileId);
            if (fileId == -1)
                return "";

            User? user = HttpUtils.GetAuthenticatedUser(ctx);
            if (user == null)
                return "";

            await using var db = new AppDbContext();

            FilesDbData? file = await db.Files.FindAsync(fileId);

            if (file == null || file.UploaderId != user.UserIdentification.BlazeId)
                return "";

            // Remove existing bookmarks for this file
            var bookmarks = db.Bookmarks.Where(b => b.FileId == fileId).ToList();
            db.Bookmarks.RemoveRange(bookmarks);

            // Remove file itself from database
            db.Files.Remove(file);
            await db.SaveChangesAsync();

            string pathToDelete = Path.Combine(
                ServerGlobals.BaseDirectory,
                $"wwwroot/skate3/content/PS3/SKATEPARK/{file.UploaderId}/{file.FileId}"
            );

            if (Directory.Exists(pathToDelete))
                Directory.Delete(pathToDelete, true);

            return new XElement("IntegerContainer", new XElement("value", "0")).ToString();
        }

        public static async Task<string> Vote(HttpListenerContext ctx)
        {
            using var reader = new StreamReader(ctx.Request.InputStream);
            string body = await reader.ReadToEndAsync();
            var parsed = System.Web.HttpUtility.ParseQueryString(body);

            int fileId = -1;
            int.TryParse(parsed["fileId"], out fileId);

            float rating = -1;
            float.TryParse(parsed["rating"], out rating);

            User? user = HttpUtils.GetAuthenticatedUser(ctx);

            if (user == null || fileId == -1 || rating == -1 || rating > 5)
                return "0";

            uint reviewerId = user.UserIdentification.BlazeId;

            await using var db = new AppDbContext();

            FilesDbData? file = await db.Files.Include(f => f.Ratings).FirstOrDefaultAsync(f => f.FileId == fileId);
            if (file == null) return "0";

            if (!file.Ratings.Any(x=>x.UserId == reviewerId))
            {
                // Add Rating
                var newRating = new Rating
                {
                    FileId = fileId,
                    Stars = rating,
                    UserId = reviewerId
                };

                file.Ratings.Add(newRating);
            }
            else
            {
                // Update existing rating for user
                file.Ratings.Where(x => x.UserId == reviewerId).First().Stars = rating;
            }

            await db.SaveChangesAsync();
            return "1";
        }

        public static async Task<string> GetBookmarkedContent(HttpListenerContext ctx)
        {
            if (!uint.TryParse(ctx.Request.QueryString["userId"], out uint userId))
                return "";

            await using var db = new AppDbContext();

            var bookmarkedFileIds = db.Bookmarks
                .Where(b => b.UserId == userId)
                .Select(b => b.FileId)
                .ToList();

            return FileDatasToXml(db.Files
                .Include(f => f.Ratings)
                .Where(f => bookmarkedFileIds.Contains(f.FileId))
                .ToList());
        }

        public static async Task<string> AddBookmark(HttpListenerContext ctx)
        {
            string failedReponse = new XElement("IntegerContainer", new XElement("value", "0")).ToString();

            using var reader = new StreamReader(ctx.Request.InputStream);
            string body = await reader.ReadToEndAsync();
            var parsed = System.Web.HttpUtility.ParseQueryString(body);

            if (!int.TryParse(parsed["fileId"], out int fileId))
                return failedReponse;

            User? user = HttpUtils.GetAuthenticatedUser(ctx);
            if (user == null)
                return failedReponse;

            uint userId = user.UserIdentification.BlazeId;

            await using var db = new AppDbContext();

            if (await db.Bookmarks.AnyAsync(b => b.UserId == userId && b.FileId == fileId))
                return failedReponse;

            db.Bookmarks.Add(new Bookmark
            {
                UserId = userId,
                FileId = fileId
            });

            await db.SaveChangesAsync();
            return new XElement("IntegerContainer", new XElement("value", "1")).ToString();
        }

        public static async Task DeleteBookmark(HttpListenerContext ctx)
        {
            using var reader = new StreamReader(ctx.Request.InputStream);
            string body = await reader.ReadToEndAsync();
            var parsed = System.Web.HttpUtility.ParseQueryString(body);

            if (!int.TryParse(parsed["fileId"], out int fileId))
                return;

            User? user = HttpUtils.GetAuthenticatedUser(ctx);
            if (user == null)
                return;

            await using var db = new AppDbContext();

            var bookmark = await db.Bookmarks.FirstOrDefaultAsync(b => b.UserId == user.UserIdentification.BlazeId && b.FileId == fileId);
            if (bookmark == null)
                return;

            db.Bookmarks.Remove(bookmark);
            await db.SaveChangesAsync();
        }
    }
}