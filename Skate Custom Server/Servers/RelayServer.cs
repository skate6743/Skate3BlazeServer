using Servers.Blaze.Models;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Servers
{
    public class RelayServer
    {
        private short _port = 17000;
        private UdpClient? _udpClient;
        private Task _serverTask;
        private readonly ConcurrentDictionary<IPEndPoint, DateTime> _players = new();
        private CancellationTokenSource? _cts;
        private Game _game;
        private ConcurrentBag<IPEndPoint> _whitelistedIps = new ConcurrentBag<IPEndPoint>();

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _udpClient = new UdpClient(_port);
            _serverTask = RunAsync(_cts.Token);
        }

        public async Task StopAsync()
        {
            if (_cts == null || _udpClient == null)
                return;
            await _cts.CancelAsync();
            await _serverTask;
            _udpClient.Close();
            _cts.Dispose();
        }

        public RelayServer(Game game, short port)
        {
            _port = port;
            _game = game;
        }

        private async Task RunAsync(CancellationToken ct)
        {
            while (true)
            {
                UdpReceiveResult result;
                try { result = await _udpClient.ReceiveAsync(ct); }
                catch (OperationCanceledException) { break; }
                catch (ObjectDisposedException) { break; }
                catch (SocketException) { continue; }
                _= UdpSendBack(_udpClient, result);
            }
        }

        private async Task UdpSendBack(UdpClient udp, UdpReceiveResult result)
        {
            try
            {
                var buf = result.Buffer;
                if (result.RemoteEndPoint is IPEndPoint ep)
                {
                    if (buf.Length > 5000)
                        return;

                    if (_game.PlayersInQueue > 0 && _players.Count < 11)
                    {
                        if (!_whitelistedIps.Contains(ep))
                        {
                            _whitelistedIps.Add(ep);
                        }
                    }

                    if (!_whitelistedIps.Contains(ep))
                        return;

                    _players[ep] = DateTime.UtcNow;

                    var stale = _players.Where(kv => (DateTime.UtcNow - kv.Value).TotalSeconds >= 5).ToList();
                    foreach (var kv in stale)
                    {
                        _players.TryRemove(kv.Key, out _);
                    }


                    var tasks = _players.Keys
                        .Where(receiver => !receiver.Equals(ep))
                        .Select(receiver => udp.SendAsync(buf, buf.Length, receiver));

                    await Task.WhenAll(tasks);
                }
            }
            catch { }
        }
    }
}