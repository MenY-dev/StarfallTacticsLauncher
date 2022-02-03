using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class CharacterModeProfile
    {
        [JsonPropertyName("chars")]
        public List<Character> Chars { get; set; } = new List<Character>();
    }
}
