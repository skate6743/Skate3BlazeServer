using Blaze.Components.Gamemanager.Models;
using System.Net.Sockets;

namespace Servers.Blaze.Models
{
    public class Player()
    {
        public User UserData; // Reference to user session details
        public ReplicatedGamePlayer PlayerData;
    }
}
