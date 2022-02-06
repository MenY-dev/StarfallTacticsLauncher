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
        [JsonPropertyName("items")]
        public List<ItemEntry> Items { get; set; } = new List<ItemEntry>();

        [JsonPropertyName("discovery_items")]
        public List<ItemEntry> DiscoveryItems { get; set; } = new List<ItemEntry>();

        [JsonPropertyName("ships")]
        public List<ShipEntry> Ships { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ship_decals")]
        public List<ShipDecalEntry> ShipDecals { get; set; } = new List<ShipDecalEntry>();

        [JsonPropertyName("ship_skins")]
        public List<ShipSkinEntry> ShipSkins { get; set; } = new List<ShipSkinEntry>();

        [JsonPropertyName("skin_colors")]
        public List<SkinColorEntry> SkinColors { get; set; } = new List<SkinColorEntry>();
    }
}
