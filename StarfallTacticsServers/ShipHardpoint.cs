using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class ShipHardpoint
    {
        [JsonPropertyName("hp")]
        public string Hardpoint { get; set; } = string.Empty;

        [JsonPropertyName("eqlist")]
        public List<HardpointEquipment> EquipmentList { get; set; } = new List<HardpointEquipment>();
    }
}
