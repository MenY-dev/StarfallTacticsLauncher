using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Instances
{
    public interface IInstance
    {
        InstanceState State { get; set; }

        bool IsStarted { get; set; }
        bool IsCanceled { get; set; }
        InstanceManager Manager { get; set; }
        Process Process { get; set; }

        string InstanceMap { get; set; }
        int InstancePort { get; set; }
        int InstanceID { get; set; }
        string Auth { get; set; }

        string GameMode { get; set; }
        bool IsCustomGame { get; set; }

        string SfMgr { get; set; }
        string RealmMgr { get; set; }
        string GalaxyMgrAddress { get; set; }
        int GalaxyMgrPort { get; set; }

        event EventHandler<InstanceStateEventArgs> StateChanged;

        void OnInstanceStateChanged(InstanceStateEventArgs args);

        string ToInstanceDocument();
    }
}
