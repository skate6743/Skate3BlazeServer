using Blaze;
using Blaze.Components.Gamemanager;
using Blaze.Components.Gamemanager.Models;
using Blaze.Components.Redirector.Handlers;
using Blaze.MessageLists;
using Servers.Blaze.Models;
using System.Net;
using System.Net.Sockets;

namespace Servers.Blaze
{
    public class BlazeServer
    {
        private ushort _port;
        private CancellationTokenSource? _cts;
        private TcpListener? _listener;
        private Task _serverTask;

        public BlazeServer(ushort port)
        {
            _port = port;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            _serverTask = RunAsync(_cts.Token);
        }

        public async Task StopAsync()
        {
            if (_cts == null || _listener == null)
                return;

            await _cts.CancelAsync();
            await _serverTask;
            _listener.Stop();
            _listener.Dispose();
            _cts.Dispose();
        }

        private async Task RunAsync(CancellationToken ct)
        {
            Console.WriteLine($"Blaze server started on IP {ServerGlobals.ServerIP} and port {_port}");

            while (!ct.IsCancellationRequested)
            {
                // Wait for a client to connect
                try
                {
                    var client = await _listener.AcceptTcpClientAsync(ct);

                    // Handle the new client in a separate task
                    _ = HandleClient(client);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            using (client)
            {
                const int headerLength = 0x0C;
                NetworkStream clientStream = client.GetStream();
                byte[] buffer = new byte[4096];
                int bytesRead;
                var accumulated = new List<byte>();
                var user = new User
                {
                    Stream = clientStream,
                };

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ServerGlobals.PingPeriodSecs + 3));
                try
                {
                    while ((bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length, cts.Token)) > 0)
                    {
                        cts.Dispose();
                        cts = new CancellationTokenSource(TimeSpan.FromSeconds(ServerGlobals.PingPeriodSecs + 3));
                        accumulated.AddRange(buffer.AsSpan(0, bytesRead).ToArray());

                        // Limit first packet to only be allowed to be Util component
                        if (!user.HasSentPreAuth && (accumulated.Count < 8 || (accumulated[3] != 5 && accumulated[3] != 9)))
                            cts.Cancel();

                        user.HasSentPreAuth = true;

                        while (true)
                        {
                            if (accumulated.Count < headerLength)
                                break;

                            ushort bodyLength = (ushort)((accumulated[0] << 8) | accumulated[1]);

                            int fullPacketSize = headerLength + bodyLength;

                            if (accumulated.Count < fullPacketSize)
                                break;

                            byte[] packetBytes = accumulated
                                .GetRange(0, fullPacketSize)
                                .ToArray();

                            accumulated.RemoveRange(0, fullPacketSize);
                            await DispatchPacket(user, packetBytes);
                        }
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception) { }
                finally
                {
                    cts.Dispose();

                    try
                    {
                        await HandleClientDisconnected(user);
                    }
                    catch (Exception ex)
                    {
                        ServerLogger.Log(ex.Message);
                        
                        // Ensure user is always removed from the global list
                        if (user.IsAuthenticated)
                            ServerGlobals.Users.TryRemove(user.Session.BlazeId, out _);
                    }
                }

                
            }
        }

        private async Task DispatchPacket(User user, byte[] packetBytes)
        {
            BlazeComponent component =
                (BlazeComponent)TdfUtils.GetComponentFromPacket(packetBytes);

            // Restrict access to some components until authenticated with proper PSN ticket
            if (!user.IsAuthenticated &&
                component != BlazeComponent.Util &&
                component != BlazeComponent.Authentication &&
                component != BlazeComponent.Redirector)
            {
                return;
            }

            switch (component)
            {
                case BlazeComponent.Redirector:
                    await GetServerInstanceHandler.HandleRequest(user, packetBytes);
                    break;
                case BlazeComponent.Authentication:
                    await AuthenticationComponentRouter.Handle(user, packetBytes);
                    break;
                case BlazeComponent.Util:
                    await UtilComponentRouter.Handle(user, packetBytes);
                    break;
                case BlazeComponent.UserSessions:
                    await UserSessionsComponentRouter.Handle(user, packetBytes);
                    break;
                case BlazeComponent.AssociationLists:
                    await AssociationListsComponentRouter.Handle(user, packetBytes);
                    break;
                case BlazeComponent.GameReporting:
                    await GameReportingComponentRouter.Handle(user, packetBytes);
                    break;
                case BlazeComponent.Gamemanager:
                    await GamemanagerComponentRouter.Handle(user, packetBytes);
                    break;
                default:
                    await ServerUtils.SendError(
                        user,
                        packetBytes,
                        ServerUtils.ErrorCode.ERR_COMPONENT_NOT_FOUND);
                    break;
            }
        }

        private async Task HandleClientDisconnected(User user)
        {
            user.Disconnected = true;
            if (user.IsAuthenticated)
            {
                if (user.CurrentGame != null)
                {
                    Player player = user.GamePlayer;
                    await GameManagerUtils.RemoveUserFromGame(player, user.CurrentGame, (int)PlayerRemovedReason.PLAYER_LEFT);
                }

                ServerGlobals.Users.TryRemove(user.Session.BlazeId, out _);
            }
        }
    }
}