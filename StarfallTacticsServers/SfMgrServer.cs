using StarfallTactics.StarfallTacticsServers.Json;
using StarfallTactics.StarfallTacticsServers.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class SfMgrServer : BaseMgrServer
    {
        public PlayerServer PlayerServer { get; set; } = null;
        protected StarfallProfile Profile => PlayerServer?.Profile;
        protected MatchmakerClient Matchmaker => PlayerServer?.Matchmaker;

        protected static readonly string EmptyResponse = "{\"doc\": {}}";

        protected override void HandleQuery(HttpListenerContext context, ClientQuery query)
        {
            JsonNode response = null;

            Profile?.Use(e =>
            {
                switch (query.Function)
                {
                    case "auth":
                    case "authcompletion":
                        Matchmaker?.Send(PacketType.PlayerJoined, new JsonObject());
                        response = JsonSerializer.SerializeToNode(new MgrAuthResponse("127.0.0.1", "1000"));
                        break;

                    case "getrealms":
                        response = Profile.CreateRealmsResponse();
                        break;

                    case "allmyproperty":
                        response = new JsonObject
                        {
                            ["data_result"] = new JsonObject
                            {
                                ["$"] = Profile.CreateAllMyPropertyResponse().ToJsonString()
                            }
                        };

                        break;

                    case "charact.edit":
                        response = Profile.CreateCharactEditResponse();
                        break;

                    case "charact.select":
                        response = Profile.CreateCharactSelectResponse();
                        break;

                    case "analyticsregister":
                        response = EmptyResponse;
                        break;

                    case "getspecialfleet":
                        response = EmptyResponse;
                        break;

                    default:
                        break;
                }
            });

            if ((response is null) == false)
                SendResponse(context, response);
        }
    }
}
