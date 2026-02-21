using Blaze.Components.Redirector.Commands;
using Servers.Blaze.Models;
using Blaze.Components.Redirector.Models;
using Blaze.Components.UserSessions.Models;
using System.Net;
using Servers;

namespace Blaze.Components.Redirector.Handlers
{
    public class GetServerInstanceHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            byte[] serverIPBytes = IPAddress.Parse(ServerGlobals.ServerIP).GetAddressBytes();
            serverIPBytes = serverIPBytes.Reverse().ToArray();

            BlazeMessage responseMsg = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                new ServerInstanceInfo
                {
                    Address = new ServerAddress
                    {
                        IpAddress = new IpAddress
                        {
                            IP = BitConverter.ToUInt32(serverIPBytes, 0),
                            Port = ServerGlobals.ServerPort
                        }
                    },
                    Secure = false, // No SSLV3 support implemented yet
                    DefaultDnsAddress = 0
                });

            await user.Stream.WriteAsync(responseMsg.Serialize());
            
        }
    }
}
