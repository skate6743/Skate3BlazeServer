using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaze.Components.Gamemanager.Models
{
    public enum PlayerRemovedReason : int
    {
        PLAYER_JOIN_TIMEOUT = 0,
        PLAYER_CONN_LOST = 1,
        BLAZESERVER_CONN_LOST = 2,
        MIGRATION_FAILED = 3,
        GAME_DESTROYED = 4,
        GAME_ENDED = 5,
        PLAYER_LEFT = 6,
        GROUP_LEFT = 7,
        PLAYER_KICKED = 8,
        PLAYER_KICKED_WITH_BAN = 9,
        PLAYER_JOIN_FROM_QUEUE_FAILED = 10,
        PLAYER_RESERVATION_TIMEOUT = 11,
        HOST_EJECTED = 12,
    }
}
