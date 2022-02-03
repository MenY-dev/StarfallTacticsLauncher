using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class SfMgrChannelManager : ChannelManager
    {
        public SfMgrChannelManager() : base()
        {
            Channels.Add(new MatchmakerChannel(this, "Matchmaker", 1));
            Channels.Add(new Channel(this, "GeneralTextChat", 2));
            Channels.Add(new Channel(this, "SystemMessages", 3));
            Channels.Add(new Channel(this, "UserAnalytics", 4));
            Channels.Add(new Channel(this, "UserFriends", 5));
        }
    }
}
