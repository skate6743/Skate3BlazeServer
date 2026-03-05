using Blaze.Components.Authentication.Models;
using Blaze.Components.Gamemanager.Models;
using Blaze.Components.UserSessions.Models;
using System.Net;
using System.Net.Sockets;

namespace Servers.Blaze.Models
{
    public class User()
    {
        public bool HasSentPreAuth = false;
        public bool IsAuthenticated = false;
        public NetworkStream Stream;
        public SessionDetails Session = new SessionDetails();
        public UserSessionExtendedData ExtendedData = new UserSessionExtendedData();
        public UserIdentification UserIdentification;
        public bool isMatchmaking;
        public Game? CurrentGame;
        public Player? gamePlayer;
    }
}
