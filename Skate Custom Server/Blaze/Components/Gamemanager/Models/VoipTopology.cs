using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaze.Components.Gamemanager.Models
{
    public enum VoipTopology : int
    {
        VOIP_DISABLED = 0,
        VOIP_DEDICATED_SERVER = 1,
        VOIP_PEER_TO_PEER = 2,
    }
}
