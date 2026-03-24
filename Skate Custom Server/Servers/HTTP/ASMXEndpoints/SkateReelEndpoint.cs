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
        public static async Task<int> HandleUpload(HttpListenerContext ctx)
        {
            User? user = HttpUtils.GetAuthenticatedUser(ctx);
            if (user == null)
                return -1;

            var parser = await MultipartFormDataParser.ParseAsync(ctx.Request.InputStream);

            if (!int.TryParse(parser.GetParameterValue("typeId"), out int typeId))
                return -1;

            if (typeId != (int)FileType.SKATEPARK)
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
            var parkFile = parser.Files.FirstOrDefault(f => f.Name == "file");

            if (thumbnail == null || parkFile == null)
                return -1;

            long totalSize = thumbnail.Data.Length + parkFile.Data.Length;
            if (totalSize > 512000) // Limit upload size to 512kb
                return -1;

            await using var db = new AppDbContext();
            var file = new FilesDbData
            {
                UploaderId = user.UserIdentification.BlazeId,
                UploaderName = user.UserIdentification.Name,
                Type = (FileType)typeId,
                Tags = tags,
                LocationId = locationId,
                Description = description,
                CreateDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            db.Files.Add(file);
            await db.SaveChangesAsync();

            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"wwwroot/skate3/content/PS3/SKATEPARK/{user.UserIdentification.BlazeId}/{file.FileId}");
            Directory.CreateDirectory(dir);

            using (var fs = File.Create($"{dir}/{file.FileId}_thumb.jpg"))
                await thumbnail.Data.CopyToAsync(fs);

            using (var fs = File.Create($"{dir}/{file.FileId}.bin"))
                await parkFile.Data.CopyToAsync(fs);

            return file.FileId;
        }

        private static float GetRating(FilesDbData file)
        {
            return file.Ratings.Count > 0
                ? file.Ratings.Average(r => r.Stars)
                : 0;
        }

        public static string FileDatasToXml(List<FilesDbData> Files)
        {
            var container = new XElement("ContentInfoContainer",
                Files.Select(file =>
                    {
                        string basePath = $"/skate3/content/PS3/SKATEPARK/{file.UploaderId}/{file.FileId}/{file.FileId}";
                        string[] tags = file.Tags.Replace(" ", "").Split(',');

                        return new XElement("content",
                            new XElement("fileId", file.FileId),
                            new XElement("fileType", "SKATEPARK"),
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
                            new XElement("contentUri", basePath + ".bin"),
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

            int rankingId = -1;
            int.TryParse(ctx.Request.QueryString["rankingId"], out rankingId);

            if (rankingId == -1)
                return "";

            await using var db = new AppDbContext();

            RankingType type = (RankingType)rankingId;

            List<FilesDbData> files = db.Files
                .Include(f => f.Ratings)
                .Where(f => f.Type == FileType.SKATEPARK)
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

            if (pagination == null)
                return "";

            await using var db = new AppDbContext();

            return FileDatasToXml(db.Files
                .Include(f => f.Ratings)
                .Where(f => f.Type == FileType.SKATEPARK)
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
            int fileId = -1;
            int.TryParse(ctx.Request.QueryString["fileId"], out fileId);
            if (fileId == -1) return;

            await using var db = new AppDbContext();

            var file = await db.Files.FindAsync(fileId);
            if (file == null) return;

            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                $"wwwroot/skate3/content/PS3/SKATEPARK/{file.UploaderId}/{file.FileId}/{file.FileId}.bin"
            );

            if (!await HttpUtils.ServeFile(ctx, path)) return;

            // Update download count in db
            file.DownloadCount++;
            await db.SaveChangesAsync();
        }

        public static async Task Delete(HttpListenerContext ctx)
        {
            using var reader = new StreamReader(ctx.Request.InputStream);
            string body = await reader.ReadToEndAsync();
            var parsed = System.Web.HttpUtility.ParseQueryString(body);

            int fileId = -1;
            int.TryParse(parsed["fileId"], out fileId);
            if (fileId == -1) return;

            User? user = HttpUtils.GetAuthenticatedUser(ctx);
            if (user == null)
                return;

            await using var db = new AppDbContext();

            FilesDbData? file = await db.Files.FindAsync(fileId);

            if (file == null || file.UploaderId != user.UserIdentification.BlazeId)
                return;

            db.Files.Remove(file);
            await db.SaveChangesAsync();

            string pathToDelete = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                $"wwwroot/skate3/content/PS3/SKATEPARK/{file.UploaderId}/{file.FileId}"
            );

            if (Directory.Exists(pathToDelete))
                Directory.Delete(pathToDelete, true);
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

            FilesDbData file = await db.Files.Include(f => f.Ratings).FirstOrDefaultAsync(f => f.FileId == fileId);
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
                // Update current rating for user
                file.Ratings.Where(x => x.UserId == reviewerId).First().Stars = rating;
            }

            await db.SaveChangesAsync();
            return "1";
        }
    }
}