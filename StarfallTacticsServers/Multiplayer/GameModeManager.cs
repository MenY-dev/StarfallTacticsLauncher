using StarfallTactics.StarfallTacticsServers.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public class GameModeManager
    {
        public virtual MatchmakerServer Matchmaker
        {
            get => matchmaker;
            protected set
            {
                if (matchmaker != value)
                {
                    if ((matchmaker is null) == false)
                        matchmaker.PlayerLeaves -= OnPlayerLeaves;

                    matchmaker = value;

                    if ((matchmaker is null) == false)
                        matchmaker.PlayerLeaves += OnPlayerLeaves;
                }
            }
        }

        public virtual InstanceManager InstanceManager => Matchmaker.InstanceManager;

        private MatchmakerServer matchmaker;

        public GameModeManager(MatchmakerServer matchmaker)
        {
            Matchmaker = matchmaker;
        }

        protected virtual void OnPlayerLeaves(object sender, PlayerEventArgs e)
        {

        }

        public virtual void Input(Player player, JsonNode packet)
        {

        }
    }
}

