using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Instances
{
    public class CharacterInfo
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public string Auth { get; set; } = string.Empty;
        public int Team { get; set; } = 0;
        public int Role { get; set; } = 0;
        public int Faction { get; set; } = 0;
        public int PartyId { get; set; } = -1;
        public bool InGalaxy { get; set; } = false;
        public int HexOffsetX { get; set; } = -1;
        public int HexOffsetY { get; set; } = -1;
    }
}
