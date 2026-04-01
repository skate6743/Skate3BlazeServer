using HttpMultipartParser;
using Servers.Blaze.Models;
using System.Net;

namespace Servers.HTTP.ASMXEndpoints
{
    public class SkateProfileEndpoint
    {
        public static async Task<string> UploadSchema(HttpListenerContext ctx)
        {
            User? user = HttpUtils.GetAuthenticatedUser(ctx);
            if (user == null)
                return "0";

            var parser = await MultipartFormDataParser.ParseAsync(ctx.Request.InputStream);

            var schemaFile = parser.Files.FirstOrDefault(f => f.Name == "schema");

            if (schemaFile == null)
                return "0";

            if (schemaFile.Data.Length > 50000) // Limit upload size to 50kb
                return "0";

            uint userId = user.UserIdentification.BlazeId;

            string dir = Path.Combine(ServerGlobals.BaseDirectory, "wwwroot/skate3/content/PS3/SCHEMA");
            Directory.CreateDirectory(dir);

            using (var fs = File.Create(Path.Combine(dir, $"{userId}.bin")))
                await schemaFile.Data.CopyToAsync(fs);

            return "1";
        }

        public static async Task GetSchema(HttpListenerContext ctx)
        {
            int userId = -1;
            int.TryParse(ctx.Request.QueryString["userId"], out userId);
            if (userId == -1) return;

            string path = Path.Combine(
                ServerGlobals.BaseDirectory,
                $"wwwroot/skate3/content/PS3/SCHEMA/{userId}.bin"
            );

            await HttpUtils.ServeFile(ctx, path);
        }
    }
}