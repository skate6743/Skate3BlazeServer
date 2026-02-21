using System.Net;
using System.Text;
using System.Xml.Linq;

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

                if (path.ToLower().Contains("skate3/webkit"))
                    path = "/skate3/webkit/temp.html";

                // Craft a qos xml response based on qtyp
                if (path == "/qos/qos")
                {
                    string qtyp = req.QueryString["qtyp"];

                    bool isQos2 = qtyp == "2";

                    IPAddress ip = IPAddress.Parse(ServerGlobals.ServerIP);

                    byte[] bytes = ip.GetAddressBytes();
                    bytes = bytes.Reverse().ToArray();

                    uint ipInt = BitConverter.ToUInt32(bytes, 0);

                    var xml = new XDocument(
                        new XDeclaration("1.0", "UTF-8", null),
                        new XElement("qos",
                            new XElement("numprobes", isQos2 ? ServerGlobals.QoSProbes : 0),
                            new XElement("qosport", ServerGlobals.ServerPort),
                            new XElement("probesize", isQos2 ? ServerGlobals.QoSProbeSize : 0),
                            new XElement("qosip", ipInt),
                            new XElement("requestid", isQos2 ? 2 : 1),
                            new XElement("reqsecret", isQos2 ? 1 : 0)
                        )
                    );

                    byte[] data = Encoding.UTF8.GetBytes(xml.ToString());

                    res.ContentType = "text/xml";
                    res.ContentLength64 = data.Length;
                    await res.OutputStream.WriteAsync(data, 0, data.Length, ct);
                    res.Close();
                    continue;
                }

                string filePath = Path.Combine("wwwroot", path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(filePath))
                {
                    byte[] data = await System.IO.File.ReadAllBytesAsync(filePath, ct);
                    res.ContentType = "text/xml";
                    res.ContentLength64 = data.Length;
                    await res.OutputStream.WriteAsync(data, 0, data.Length, ct);
                }
                else
                {
                    res.StatusCode = 404;
                }

                res.Close();
            }
        }
    }
}
