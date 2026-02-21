using System.Net;
using System.Net.Sockets;

namespace Servers
{
    public class QoSServer
    {
        private ushort _port;
        private CancellationTokenSource? _cts;
        private UdpClient? _udpClient;
        private Task _serverTask;

        public QoSServer(ushort port)
        {
            _port = port;
        }

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

        private async Task RunAsync(CancellationToken ct)
        {
            Console.WriteLine($"QoS UDP server started on port {_port}");

            while (!ct.IsCancellationRequested)
            {
                UdpReceiveResult result;
                try { result = await _udpClient.ReceiveAsync(ct); }
                catch (OperationCanceledException) { break; }
                catch (ObjectDisposedException) { break; }

                _ = UdpSendBack(_udpClient, result);
            }
        }

        private async Task UdpSendBack(UdpClient udp, UdpReceiveResult result)
        {
            try
            {
                var buf = result.Buffer;
                if (buf.Length < 0x20 && result.RemoteEndPoint is IPEndPoint { Address.AddressFamily: AddressFamily.InterNetwork } ep)
                {
                    var ip = ep.Address.GetAddressBytes();
                    var response = new byte[buf.Length + 10];
                    buf.CopyTo(response, 0);
                    int o = buf.Length;
                    response[o] = ip[0];
                    response[o + 1] = ip[1];
                    response[o + 2] = ip[2];
                    response[o + 3] = ip[3];
                    response[o + 4] = (byte)(ep.Port >> 8);
                    response[o + 5] = (byte)(ep.Port & 0xFF);
                    await udp.SendAsync(response, response.Length, result.RemoteEndPoint);
                }
                else if (buf.Length >= 0x20 && buf.Length <= ServerGlobals.QoSProbeSize)
                {
                    await udp.SendAsync(buf, buf.Length, result.RemoteEndPoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}