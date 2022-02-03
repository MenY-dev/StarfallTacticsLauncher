using StarfallTactics.StarfallTacticsServers.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Instances
{
    public class Instance : IInstance
    {
        public InstanceState State
        {
            get => state;
            set
            {
                state = value;

                (this as IInstance).OnInstanceStateChanged(
                    new InstanceStateEventArgs(this, state));
            }
        }

        public string InstanceMap => (this as IInstance).InstanceMap ?? string.Empty;
        string IInstance.InstanceMap { get; set; }
        public int InstancePort => (this as IInstance).InstancePort;
        int IInstance.InstancePort { get; set; }
        public int InstanceID => (this as IInstance).InstanceID;
        int IInstance.InstanceID { get; set; }
        public string Auth => (this as IInstance).Auth ?? string.Empty;
        string IInstance.Auth { get; set; }

        public string GameMode => (this as IInstance).GameMode ?? string.Empty;
        string IInstance.GameMode { get; set; }
        public bool IsCustomGame => (this as IInstance).IsCustomGame;
        bool IInstance.IsCustomGame { get; set; }

        public string SfMgr => (this as IInstance).SfMgr ?? string.Empty;
        string IInstance.SfMgr { get; set; }
        public string RealmMgr => (this as IInstance).RealmMgr ?? string.Empty;
        string IInstance.RealmMgr { get; set; }
        public string GalaxyMgrAddress => (this as IInstance).GalaxyMgrAddress ?? string.Empty;
        string IInstance.GalaxyMgrAddress { get; set; }
        public int GalaxyMgrPort => (this as IInstance).GalaxyMgrPort;
        int IInstance.GalaxyMgrPort { get; set; }

        public bool IsStarted => (this as IInstance)?.IsStarted ?? false;
        bool IInstance.IsStarted { get; set; } = false;
        public bool IsCanceled => (this as IInstance)?.IsCanceled ?? false;
        bool IInstance.IsCanceled { get; set; } = false;

        public InstanceManager Manager => (this as IInstance)?.Manager;
        InstanceManager IInstance.Manager { get; set; }
        public Process Process => (this as IInstance)?.Process;
        Process IInstance.Process { get; set; }

        public event EventHandler<InstanceStateEventArgs> StateChanged;

        private InstanceState state;

        public virtual void Start(InstanceManager manager) => manager?.StartInstance(this);

        public virtual void Stop() => Manager?.StartInstance(this);

        public virtual JsonObject ToJsonObject()
        {
            string auth = Auth ?? string.Empty;

            JsonObject doc = new JsonObject
            {
                ["sfmgr"] = new JsonObject
                {
                    ["url"] = SfMgr,
                    ["auth"] = auth
                },
                ["realmmgr"] = new JsonObject
                {
                    ["url"] = RealmMgr,
                    ["auth"] = auth
                },
                ["galaxymgr"] = new JsonObject
                {
                    ["address"] = GalaxyMgrAddress,
                    ["port"] = GalaxyMgrPort,
                    ["instanceid"] = InstanceID,
                    ["systemid"] = 0,
                    ["auth"] = auth
                },
                ["game_mode"] = GameMode,
                ["is_custom_game"] = IsCustomGame ? 1 : 0
            };

            return doc;
        }

        public virtual string ToInstanceDocument()
        {
            return ToJsonObject()?.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) ?? string.Empty;
        }

        void IInstance.OnInstanceStateChanged(InstanceStateEventArgs args)
        {
            this.Log($"Instance State Changed: (ID={InstanceID}, Map={InstanceMap}, Port={InstancePort}, State={args.State}, Auth={Auth})");
            StateChanged?.Invoke(this, args);
        }
    }
}