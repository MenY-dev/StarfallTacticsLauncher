using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public class MatchmakerPacketEventArgs : EventArgs
    {
        public MatchmakerPacket Packet { get; }

        public MatchmakerPacketEventArgs(MatchmakerPacket packet)
        {
            Packet = packet;
        }
    }
}
