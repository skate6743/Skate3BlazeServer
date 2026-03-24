using Servers.Blaze.Models;
using Servers.Database;
using System.Net;

namespace Servers.HTTP
{
    public class HttpUtils
    {
        public static User? GetAuthenticatedUser(HttpListenerContext ctx)
        {
            string? blazeSession = ctx.Request.Headers["BLAZE-SESSION"];
            if (string.IsNullOrEmpty(blazeSession)) return null;
            return ServerGlobals.Users.Values.FirstOrDefault(x => x.Session.BlazeToken == blazeSession);
        }

        public static (int start, int end)? ParsePagination(HttpListenerContext ctx, int maxRange = 7)
        {
            if (!int.TryParse(ctx.Request.QueryString["startIndex"], out int start) ||
                !int.TryParse(ctx.Request.QueryString["endIndex"], out int end) ||
                end - start > maxRange)
                return null;
            return (start - 1, end - 1);
        }

        public static async Task<bool> ServeFile(HttpListenerContext ctx, string path)
        {
            if (!File.Exists(path))
                return false;

            byte[] data = await File.ReadAllBytesAsync(path);
            var response = ctx.Response;
            response.ContentType = "application/octet-stream";
            response.ContentLength64 = data.Length;
            response.AddHeader("Content-Disposition", $"inline; filename={Path.GetFileName(path)}");
            await response.OutputStream.WriteAsync(data);
            response.Close();
            return true;
        }
    }
}
