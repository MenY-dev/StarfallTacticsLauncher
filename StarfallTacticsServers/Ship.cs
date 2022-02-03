using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class Ship
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("hull")]
        public int Hull { get; set; } = 0;

        [JsonPropertyName("position")]
        public int Position { get; set; } = 0;

        [JsonPropertyName("kills")]
        public int Kills { get; set; } = 99;

        [JsonPropertyName("death")]
        public int Death { get; set; } = 88;

        [JsonPropertyName("played")]
        public int Played { get; set; } = 77;

        [JsonPropertyName("woncount")]
        public int WonCount { get; set; } = 66;

        [JsonPropertyName("lostcount")]
        public int LostCount { get; set; } = 55;

        [JsonPropertyName("xp")]
        public int Xp { get; set; } = 999999999;

        [JsonPropertyName("level")]
        public int Level { get; set; } = 20;

        [JsonPropertyName("damagedone")]
        public int DamageDone { get; set; } = 999;

        [JsonPropertyName("damagetaken")]
        public int DamageTaken { get; set; } = 999;

        [JsonPropertyName("timetoconstruct")]
        public int TimeToConstruct { get; set; } = 0;

        [JsonPropertyName("timetorepair")]
        public int TimeToRepair { get; set; } = 0;

        [JsonPropertyName("is_favorite")]
        public int IsFavorite { get; set; } = 0;

        [JsonPropertyName("hplist")]
        public List<ShipHardpoint> HardpointList { get; set; } = new List<ShipHardpoint>();

        [JsonPropertyName("progression")]
        public List<ShipProgression> Progression { get; set; } = new List<ShipProgression>();
    }
}
