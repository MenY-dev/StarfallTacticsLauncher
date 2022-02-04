using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public class MatchmakerClient : MessagingClient
    {
        public event EventHandler<MatchmakerPacketEventArgs> PacketReceived;

        protected override void HandleInputPacket(TcpClient client, string packet)
        {
            MatchmakerPacket doc = MatchmakerPacket.Parce(packet);

            if (doc is null)
                return;

            OnPacketReceived(new MatchmakerPacketEventArgs(doc));
        }

        public override void Send(string packet)
        {
            base.Send(packet);
        }

        public virtual void Send(PacketType packetType, JsonNode packet)
        {
            base.Send(MatchmakerPacket.Create(packetType, packet));
        }

        protected virtual void OnPacketReceived(MatchmakerPacketEventArgs args)
        {
            PacketReceived?.Invoke(this, args);
        }
    }
}
