using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class InventoryItem
    {
        [JsonPropertyName("itemtype")]
        public int ItemType { get; set; } = 2;

        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("count")]
        public int Count { get; set; } = 9999;
    }
}
