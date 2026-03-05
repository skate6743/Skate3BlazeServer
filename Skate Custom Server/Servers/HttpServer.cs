using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace Servers
{
    public class HttpServer
    {
        private ushort _port;
        private CancellationTokenSource? _cts;
        private HttpListener? _listener;
        private Task _serverTask;

        public HttpServer(ushort port)
        {
            _port = port;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{ServerGlobals.HttpServerPort}/");
            _listener.Start();
            _serverTask = RunAsync(_cts.Token);
        }

        public async Task StopAsync()
        {
            if (_cts == null || _listener == null)
                return;

            await _cts.CancelAsync();
            await _serverTask;
            _cts.Dispose();
            _listener.Close();
        }

        private async Task RunAsync(CancellationToken ct)
        {
            Console.WriteLine($"Downloads HTTP server started on port {_port}");

            while (!ct.IsCancellationRequested)
            {
                HttpListenerContext ctx;
                try { ctx = await _listener.GetContextAsync().WaitAsync(ct); }
                catch (TaskCanceledException) { break; }

                var req = ctx.Request;
                var res = ctx.Response;

                // Headers game expects
                res.Headers["Server"] = "Microsoft-IIS/6.0";
                res.Headers["Accept-Ranges"] = "bytes";
                res.Headers["X-Powered-By"] = "ASP.NET";
                res.Headers["Cache-Control"] = "no-cache";

                string path = req.Url.AbsolutePath.TrimEnd('/');
                if (path == "/serverstats")
                {
                    string json = new JObject(
                        new JProperty("signed-in", ServerGlobals.Users.Count),
                        new JProperty("dirtycast-instances", ServerGlobals.LobbyRelayServers.Count)
                    ).ToString();

                    byte[] data = Encoding.Latin1.GetBytes(json);
                    await res.OutputStream.WriteAsync(data, 0, data.Length, ct);
                }
                else
                {
                    if (path.ToLower().Contains("skate3/webkit"))
                        path = "/skate3/webkit/temp.html";

                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    string filePath = Path.Combine(basePath, "wwwroot", path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                    if (File.Exists(filePath))
                    {
                        byte[] data = await File.ReadAllBytesAsync(filePath, ct);

                        // Set content type based on file extension
                        string ext = Path.GetExtension(filePath).ToLower();
                        res.ContentType = ext switch
                        {
                            ".xml" => "text/xml",
                            ".html" or ".htm" => "text/html",
                            ".jpg" or ".jpeg" => "image/jpeg",
                            ".png" => "image/png",
                            ".psg" => "application/octet-stream",
                            _ => "application/octet-stream"
                        };

                        res.ContentLength64 = data.Length;
                        await res.OutputStream.WriteAsync(data, 0, data.Length, ct);
                    }
                    else
                    {
                        res.StatusCode = 404;
                    }
                }

                res.Close();
            }
        }
    }
}
