using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class GalaxyMgrChannel : ChannelManager
    {
        public GalaxyMgrChannel() : base()
        {
            Channels.Add(new Channel(this, "CharactParty", 51));
            Channels.Add(new Channel(this, "Discovery", 52));
        }
    }
}
