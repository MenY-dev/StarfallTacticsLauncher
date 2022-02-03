using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public class QueueEntry
    {
        public Player Player { get; }
        public int Position { get; set; } = 0;
        public int Team { get; set; } = 0;
        public bool IsReady { get; set; } = false;
        public bool IsSpectator { get; set; } = false;

        public QueueEntry(Player player)
        {
            Player = player;
        }
    }
}
