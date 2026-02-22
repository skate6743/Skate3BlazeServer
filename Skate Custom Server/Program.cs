using Servers;

bool localHost = true;
string ip = "127.0.0.1";
if (!localHost)
    ip = await new HttpClient().GetStringAsync("https://api.ipify.org");

ServerGlobals.ServerIP = ip;
ServerGlobals.ServerPort = 42100;
ServerGlobals.HttpServerPort = 80;

// Game must send a heartbeat ping request every X seconds or disconnect client
ServerGlobals.PingPeriodSecs = 20;

// Start QoSServer, a UDP server used during authentication flow for testing network connection
var qosServer = new QoSServer(ServerGlobals.ServerPort);
qosServer.Start();

// Start HTTP server used for mandatory stuff like config .xml during login flow (found in wwwroot folder)
var httpServer = new HttpServer(ServerGlobals.HttpServerPort);
httpServer.Start();

// Main Blaze server
var blazeServer = new BlazeServer(ServerGlobals.ServerPort);
blazeServer.Start();

// Wait for Ctrl+C
var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

Console.WriteLine("All servers runnning, press CTRL+C to stop...");

try
{
    await Task.Delay(Timeout.Infinite, cts.Token);
}
catch (OperationCanceledException)
{
    await blazeServer.StopAsync();
    await httpServer.StopAsync();
    await qosServer.StopAsync();
    Console.WriteLine("All servers stopped.");
}