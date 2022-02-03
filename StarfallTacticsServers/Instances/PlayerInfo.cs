using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Instances
{
    public class PlayerInfo
    {
        public int UserId { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public string Auth { get; set; } = string.Empty;
        public int FleetId { get; set; } = 0;
        public int Team { get; set; } = 0;
    }
}
