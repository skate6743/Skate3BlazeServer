using Newtonsoft.Json.Linq;
using Servers.Blaze.Models;
using Servers.HTTP.ASMXEndpoints;
using Servers.HTTP.ChatEndpoints;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Servers.HTTP
{
    public class HttpServer
    {
        private ushort _port;
        private CancellationTokenSource? _cts;
        private HttpListener? _listener;
        private Task _serverTask;
        private static readonly Dictionary<string, (int count, DateTime windowStart)> _requestCounts = new();
        private static readonly HashSet<string> _blacklist = new();
        private static readonly object _rateLock = new();

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

        private static bool IsBlacklisted(string ip)
        {
            lock (_rateLock)
            {
                if (_blacklist.Contains(ip))
                    return true;

                var now = DateTime.UtcNow;

                if (_requestCounts.TryGetValue(ip, out var entry))
                {
                    if (now - entry.windowStart > TimeSpan.FromSeconds(60))
                    {
                        _requestCounts[ip] = (1, now);
                    }
                    else
                    {
                        int newCount = entry.count + 1;
                        if (newCount > 100)
                        {
                            _blacklist.Add(ip);
                            _requestCounts.Remove(ip);
                            _ = Task.Delay(TimeSpan.FromHours(1)).ContinueWith(_ =>
                            {
                                lock (_rateLock) { _blacklist.Remove(ip); }
                            });
                            return true;
                        }
                        _requestCounts[ip] = (newCount, entry.windowStart);
                    }
                }
                else
                {
                    _requestCounts[ip] = (1, now);
                }

                return false;
            }
        }

        private async Task RunAsync(CancellationToken ct)
        {
            Console.WriteLine($"Downloads HTTP server started on port {_port}");

            while (!ct.IsCancellationRequested)
            {
                HttpListenerContext ctx;
                try { ctx = await _listener.GetContextAsync().WaitAsync(ct); }
                catch (TaskCanceledException) { break; }

                string remoteIP = ctx.Request.RemoteEndPoint.Address.ToString();
                if (IsBlacklisted(remoteIP))
                {
                    ctx.Response.Close();
                    continue;
                }

                if (ctx.Request.ContentLength64 > 1_048_576)
                {
                    ctx.Response.Close();
                    continue;
                }

                _ = Task.Run(async() => {
                    try
                    {
                        var req = ctx.Request;
                        var res = ctx.Response;

                        // Game expects certain IIS server related headers
                        res.Headers["Server"] = "Microsoft-IIS/6.0";
                        res.Headers["Node"] = "customsk8webdl";
                        res.Headers["Accept-Ranges"] = "bytes";
                        res.Headers["X-Powered-By"] = "ASP.NET";
                        res.Headers["X-Frame-Options"] = "SAMEORIGIN";
                        res.Headers["X-AspNet-Version"] = "2.0.50727";
                        res.Headers["Cache-Control"] = "max-age=0, no-cache, no-store";
                        res.Headers["Connection"] = "keepalive";
                        res.Headers["Date"] = DateTime.UtcNow.ToString("R");

                        string path = req.Url.AbsolutePath.TrimEnd('/');
                        
                        // Custom server endpoints (not officially in Skate 3)
                        switch (path)
                        {
                            case "/sendchat":
                                await SendChatEndpoint.SendChat(ctx);
                                res.Close();
                                break;
                            case "/serverstats":
                                uint inLobbies = 0;
                                foreach (User user in ServerGlobals.Users.Values)
                                {
                                    if (user.CurrentGame != null)
                                        inLobbies++;
                                }

                                string json = new JObject(
                                    new JProperty("signed-in", ServerGlobals.Users.Count),
                                    new JProperty("in-lobbies", inLobbies),
                                    new JProperty("games-count", ServerGlobals.Games.Count),
                                    new JProperty("dirtycast-instances", ServerGlobals.LobbyRelayServers.Count)
                                ).ToString();

                                byte[] data = Encoding.Latin1.GetBytes(json);
                                await res.OutputStream.WriteAsync(data, 0, data.Length, ct);
                                break;
                            case "/spoofusername":
                                await SpoofUsernameEndpoint.SpoofUsername(ctx);
                                break;
                            case "/removespoof":
                                await SpoofUsernameEndpoint.RemoveSpoof(ctx);
                                break;
                            case "/spoofingpanel":
                                string panelContent = await SpoofUsernameEndpoint.GetPanel(ctx);
                                byte[] contentBytes = Encoding.Latin1.GetBytes(panelContent.ToString());
                                res.ContentLength64 = contentBytes.Length;
                                await res.OutputStream.WriteAsync(contentBytes, 0, contentBytes.Length, ct);
                                break;
                        }

                        // SkateReel.asmx endpoint
                        if (path.StartsWith("/skate3/ws/SkateReel.asmx"))
                        {
                            res.ContentType = "text/xml";
                            string responseContent = "";

                            string action = path.Split('/').LastOrDefault();
                            switch (action)
                            {
                                case "Upload":
                                    int newFileId = await SkateReelEndpoint.HandleUpload(ctx);
                                    responseContent = newFileId.ToString();
                                    break;
                                case "GetNumberOfFiles2":
                                    responseContent = new XElement("IntegerContainer", new XElement("value", "0")).ToString();
                                    break;
                                case "GetCategoriesAndTags":
                                    responseContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot\\skate3\\config\\CategoriesAndTags.xml"));
                                    break;
                                case "GetNumFilesForBrowser2":
                                    responseContent = new XElement("IntegerContainer", new XElement("value", "69")).ToString();
                                    break;
                                case "GetFeaturedContent":
                                    responseContent = await SkateReelEndpoint.GetFeaturedContent(ctx);
                                    break;
                                case "GetTopN":
                                    responseContent = await SkateReelEndpoint.GetTopN(ctx);
                                    break;
                                case "GetContent2":
                                    responseContent = await SkateReelEndpoint.GetContent2(ctx);
                                    break;
                                case "Delete":
                                    await SkateReelEndpoint.Delete(ctx);
                                    break;
                                case "GetFileContent":
                                    await SkateReelEndpoint.GetFileContent(ctx);
                                    break;
                                case "Vote":
                                    responseContent = await SkateReelEndpoint.Vote(ctx);
                                    break;
                                default:
                                    res.StatusCode = 404;
                                    break;
                            }

                            if (!string.IsNullOrEmpty(responseContent))
                            {
                                byte[] contentBytes = Encoding.Latin1.GetBytes(responseContent.ToString());
                                res.ContentLength64 = contentBytes.Length;
                                await res.OutputStream.WriteAsync(contentBytes, 0, contentBytes.Length, ct);
                            }

                            res.Close();
                            return;
                        }
                        else
                        {
                            if (path.ToLower().Contains("skate3/webkit"))
                                path = "/skate3/webkit/temp.html";

                            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

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
                    catch (Exception ex)
                    {
                        ServerLogger.Log($"HTTP request error: {ex}");
                        try { ctx.Response.StatusCode = 500; ctx.Response.Close(); } catch { }
                    }
                });
            }
        }
    }
}
