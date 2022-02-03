using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Instances
{
    public class InstanceEventArgs : EventArgs
    {
        public IInstance Instance { get; }

        public InstanceEventArgs(IInstance instance)
        {
            Instance = instance;
        }
    }
}
