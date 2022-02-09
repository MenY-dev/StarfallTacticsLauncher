using StarfallTactics.StarfallTacticsServers.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class BattleMgrServer : BaseMgrServer
    {
        public MatchmakerServer Matchmaker { get; set; }

        protected static readonly JsonNode EmptyResponse = new JsonObject { ["ok"] = 1 };

        protected override void HandleQuery(HttpListenerContext context, ClientQuery query)
        {
            JsonNode response = null;

            switch (query.Function)
            {
                case "discovery_charactgetdata":
                    var player = Matchmaker?.GetPlayer((int?)query["charactid"] ?? -1);

                    if (player is null)
                        break;

                    response = new JsonObject
                    {
                        ["result_data"] = new JsonObject
                        {
                            ["$"] = player.DiscoveryCharacterData?.ToJsonString()
                        }
                    };
                    break;
                case "battle_results":
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(60000);
                        Matchmaker?.InstanceManager?.StopInstance((string)query["auth"]);
                    });

                    response = EmptyResponse;
                    break;
                default:
                    break;
            }

            if ((response is null) == false)
                SendResponse(context, response);
        }
    }
}
