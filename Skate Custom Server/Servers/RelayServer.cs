using Org.BouncyCastle.Asn1.Cms;
using Servers.Blaze.Models;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Servers
{
    public class RelayServer
    {
        public readonly short Port;
        private readonly Game _game;
        private readonly ConcurrentDictionary<uint, IPEndPoint> _whitelistedUsers = new();
        private readonly ConcurrentDictionary<IPEndPoint, uint> _endpointToUser = new();

        private UdpClient? _udpClient;
        private Task? _serverTask;
        private CancellationTokenSource? _cts;

        public RelayServer(Game game, short port)
        {
            Port = port;
            _game = game;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _udpClient = new UdpClient(Port);
            _serverTask = RunAsync(_cts.Token);
        }

        public async Task StopAsync()
        {
            if (_cts == null || _udpClient == null || _serverTask == null)
                return;
            try
            {
                await _cts.CancelAsync();
                await _serverTask;
            }
            catch { }
            finally
            {
                _udpClient.Close();
                _cts.Dispose();
            }
        }

        private async Task RunAsync(CancellationToken ct)
        {
            while (true)
            {
                UdpReceiveResult result;
                try { result = await _udpClient!.ReceiveAsync(ct); }
                catch (OperationCanceledException) { break; }
                catch (ObjectDisposedException) { break; }
                catch (SocketException) { continue; }

                _ = ProcessPacketAsync(_udpClient, result);
            }
        }

        private bool WhitelistFromRegistrationPacket(IPEndPoint ep, byte[] buf)
        {
            uint blazeId = BinaryPrimitives.ReadUInt32BigEndian(buf.AsSpan(8));

            if (_whitelistedUsers.TryAdd(blazeId, ep))
            {
                _endpointToUser.TryAdd(ep, blazeId);
                return true;
            }

            return false;
        }

        public bool RemoveFromWhitelistedUser(uint blazeId)
        {
            if (_whitelistedUsers.TryRemove(blazeId, out var ep))
            {
                _endpointToUser.TryRemove(ep, out _);
                return true;
            }
            return false;
        }

        private async Task ProcessPacketAsync(UdpClient udp, UdpReceiveResult result)
        {
            try
            {
                var buf = result.Buffer;
                if (result.RemoteEndPoint is not IPEndPoint ep)
                    return;

                if (buf.Length > 7000 || buf.Length < 16)
                    return;

                ushort receiverIdOffset = 0x0C;

                if (_game.PlayersInQueue > 0)
                {
                    // Validate incoming packet is a registration packet (first packet game ever sends)
                    if (buf[0] == 1 && buf[1] == 0 && buf[2] == 0 && buf.Length == 20)
                    {
                        if (!_endpointToUser.ContainsKey(ep))
                            WhitelistFromRegistrationPacket(ep, buf);

                        receiverIdOffset = 0x10;
                    }
                }

                uint receiverId = BinaryPrimitives.ReadUInt32BigEndian(buf.AsSpan(receiverIdOffset));

                if (_whitelistedUsers.TryGetValue(receiverId, out var targetEp))
                    await udp.SendAsync(buf, buf.Length, targetEp);
            }
            catch (ObjectDisposedException) { }
            catch (SocketException) { }
        }
    }
}