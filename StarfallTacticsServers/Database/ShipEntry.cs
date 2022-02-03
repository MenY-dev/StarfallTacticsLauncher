using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Database
{
    public class ShipEntry
    {
        [JsonPropertyName("hull")]
        public int Hull { get; set; } = 0;

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("faction")]
        public string Faction { get; set; } = "";

        [JsonPropertyName("class")]
        public string Class { get; set; } = "";
    }
}
