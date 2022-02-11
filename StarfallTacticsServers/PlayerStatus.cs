using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public enum PlayerStatus : int
    {
        Menu = 0,
        Searching = 1,
        ActiveBattle = 2,
        GalaxyExploration = 3,
        Offline = 4
    }
}
