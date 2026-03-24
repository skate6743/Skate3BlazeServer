using Blaze.Components.Authentication.Models;
using Blaze.Components.UserSessions.Models;
using Servers.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Servers.Blaze.Models
{
    public class User()
    {
        public bool HasSentPreAuth = false;
        public bool IsAuthenticated = false;
        public UserPlatform Platform;
        public NetworkStream Stream;
        public SessionDetails Session = new SessionDetails();
        public UserSessionExtendedData ExtendedData = new UserSessionExtendedData();
        public UserIdentification UserIdentification;
        public bool IsMatchmaking = false;
        public bool Disconnected = false;
        public Game? CurrentGame;
        public Player? GamePlayer;
    }
}
