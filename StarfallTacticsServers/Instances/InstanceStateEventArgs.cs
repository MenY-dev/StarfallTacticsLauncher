using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Instances
{
    public class InstanceStateEventArgs : EventArgs
    {
        public IInstance Instance{ get; }
        public InstanceState State { get; }

        public InstanceStateEventArgs(IInstance instance, InstanceState state)
        {
            Instance = instance;
            State = state;
        }
    }
}
