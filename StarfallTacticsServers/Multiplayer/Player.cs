using System.Net.Sockets;
using System.Text.Json.Nodes;

namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public class Player
    {
        public int Id { get; set; } = -1;
        public string Auth { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool InGame { get; set; } = false;
        public PlayerStatus Status { get; set; } = PlayerStatus.Offline;
        public TcpClient Client { get; set; }
        public JsonNode DiscoveryCharacterData { get; set; }
        protected object Locker { get; } = new object();

        public virtual void Send(string packet)
        {
            if (string.IsNullOrWhiteSpace(packet))
                return;

            lock (Locker)
                MatchmakerServer.Send(this, packet);
        }

        public virtual void Send(PacketType packetType, JsonNode packet)
        {
            if (packet is null)
                return;

            lock (Locker)
                MatchmakerServer.Send(this, packetType, packet);
        }
    }
}
