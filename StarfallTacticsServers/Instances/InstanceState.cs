using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Instances
{
    public enum InstanceState : int
    {
        None = 0,
        ReadyToConnect = 1,
        Exit = 2,
        Cancel = 3,
        Error = 4
    }
}
