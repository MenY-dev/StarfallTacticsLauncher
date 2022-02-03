using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public class MatchmakerPacket
    {
        public PacketType Type { get; set; } = PacketType.None;

        public JsonNode Document { get; set; }

        public static string Create<T>(PacketType packetType, T document)
        {
            JsonNode packet = new JsonObject
            {
                ["type"] = JsonSerializer.SerializeToNode(packetType.ToString()),
                ["doc"] = JsonSerializer.SerializeToNode(document)
            };

            return packet.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) ?? string.Empty;
        }

        public static string Create(PacketType packetType, JsonNode document)
        {
            JsonNode packet = new JsonObject
            {
                ["type"] = JsonSerializer.SerializeToNode(packetType.ToString()),
                ["doc"] = document
            };

            return packet.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) ?? string.Empty;
        }

        public static MatchmakerPacket Parce(string json)
        {
            MatchmakerPacket packet = new MatchmakerPacket();
            JsonNode packetNodes = JsonNode.Parse(json ?? string.Empty);
            PacketType type;

            if (Enum.TryParse(packetNodes["type"]?.GetValue<string>(), true, out type) == false)
                type = PacketType.None;

            packet.Type = type;
            packet.Document = packetNodes["doc"];

            return packet;
        }

        public T ParceDocument<T>()
        {
            return JsonSerializer.Deserialize<T>(Document ?? new JsonObject());
        }
    }
}
