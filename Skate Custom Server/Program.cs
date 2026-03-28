using Newtonsoft.Json;
using Servers.Blaze;
using Servers.HTTP;
using Servers.Models;
using Servers;

if (File.Exists("settings.json"))
{
    string serverSettingsJson = File.ReadAllText("settings.json");
    ServerSettings? serverSettings = JsonConvert.DeserializeObject<ServerSettings>(serverSettingsJson);

    if (serverSettings != null && serverSettings.LocalHost)
        ServerGlobals.ServerIP = serverSettings.LocalIPAddress;
    else
        ServerGlobals.ServerIP = await new HttpClient().GetStringAsync("https://api.ipify.org");
}
else
{
    var settings = new ServerSettings();
    File.WriteAllText("settings.json", JsonConvert.SerializeObject(settings));
    ServerGlobals.ServerIP = await new HttpClient().GetStringAsync("https://api.ipify.org");
}

ServerGlobals.ServerPort = 42100;
ServerGlobals.HttpServerPort = 80;

// Game must send a heartbeat ping request every X seconds or disconnect client
ServerGlobals.PingPeriodSecs = 15;

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
    Console.WriteLine("All servers stopped.");
}