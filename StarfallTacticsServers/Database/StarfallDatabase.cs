using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Database
{
    public class StarfallDatabase
    {
        [JsonPropertyName("ships")]
        public List<ShipEntry> Ships { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("items")]
        public List<ItemEntry> Items { get; set; } = new List<ItemEntry>();

        [JsonPropertyName("discovery_item")]
        public List<ItemEntry> DiscoveryItem { get; set; } = new List<ItemEntry>();
    }
}
