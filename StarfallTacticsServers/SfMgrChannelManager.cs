using StarfallTactics.StarfallTacticsServers.Multiplayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class SfMgrChannelManager : ChannelManager
    {
        public SfMgrChannelManager() : base()
        {
            Channels.Add(new MatchmakerChannel(this, "Matchmaker", 1));
            Channels.Add(new TextChatChannel(this, "GeneralTextChat", 2));
            Channels.Add(new TextChatChannel(this, "SystemMessages", 3));
            Channels.Add(new Channel(this, "UserAnalytics", 4));
            Channels.Add(new FriendChannel(this, "UserFriends", 5));
        }

        public override void HandleChannelRegister(TcpClient client, SFCP.Header header, string channelName)
        {
            base.HandleChannelRegister(client, header, channelName);

            if (channelName == "UserFriends")
            {
                PlayerServer?.Matchmaker?.Send(PacketType.PlayersInfoRequest, new JsonObject());
            }
        }
    }
}
