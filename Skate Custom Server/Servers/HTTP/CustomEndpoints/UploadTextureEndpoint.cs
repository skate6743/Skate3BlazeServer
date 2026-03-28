using HttpMultipartParser;
using Servers.Blaze.Models;
using Servers.Database;
using Servers.HTTP.Models;
using System.Net;

namespace Servers.HTTP.CustomEndpoints
{
    public class UploadTextureEndpoint
    {
        public static async Task<string> Upload(HttpListenerContext ctx)
        {
            User? user = HttpUtils.GetAuthenticatedUser(ctx);
            if (user == null)
                return "Unauthorized";

            var parser = await MultipartFormDataParser.ParseAsync(ctx.Request.InputStream);

            var file = parser.Files.FirstOrDefault(f => f.Name == "texture");

            if (file == null)
                return "Texture not found in request data";

            long totalSize = file.Data.Length;
            if (totalSize > 512000) // Limit upload size to 512kb
                return "File too big";

            await using var db = new AppDbContext();

            using var sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(file.Data);
            string hash = Convert.ToHexString(hashBytes).ToLower();
            file.Data.Position = 0; // Reset for later copy


            // If logo already exists give a pre existing url
            FilesDbData? existingTex = db.Files.Where(f => f.FileHash == hash && f.Type == FileType.RTEX_PS3).FirstOrDefault();
            if (existingTex != null)
                return $"/skate3/content/PS3/RTEX_PS3/{existingTex.UploaderId}/{existingTex.FileId}.psg";

            var newFileData = new FilesDbData
            {
                UploaderId = user.UserIdentification.BlazeId,
                UploaderName = user.UserIdentification.Name,
                Type = FileType.RTEX_PS3,
                Tags = "",
                LocationId = -1,
                Description = "",
                CreateDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                FileHash = hash
            };
            db.Files.Add(newFileData);
            await db.SaveChangesAsync();

            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"wwwroot/skate3/content/PS3/RTEX_PS3/{user.UserIdentification.BlazeId}/{newFileData.FileId}");
            Directory.CreateDirectory(dir);

            using (var fs = File.Create($"{dir}/{newFileData.FileId}.psg"))
                await file.Data.CopyToAsync(fs);

            return $"/skate3/content/PS3/RTEX_PS3/{newFileData.UploaderId}/{newFileData.FileId}.psg";
        }
    }
}