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
    public partial class RealmMgrServer : BaseMgrServer
    {
        public PlayerServer PlayerServer { get; set; } = null;
        protected StarfallProfile Profile => PlayerServer?.Profile;
        protected MatchmakerClient Matchmaker => PlayerServer?.Matchmaker;

        protected static readonly string EmptyResponse = "{\"doc\": {\"ok\": 1}}";

        public RealmMgrServer()
        {

        }

        protected override void HandleQuery(HttpListenerContext context, ClientQuery query)
        {
            JsonNode response = null;

            Profile?.Use(e =>
            {
                switch (query.Function)
                {
                    case "auth":
                    case "authcompletion":
                        SendMatchmakerAuth();
                        response = JsonSerializer.SerializeToNode(new MgrAuthResponse("127.0.0.1", "1200"));
                        break;

                    case "getcharacterdata":
                        response = Profile.CreateCharacterDataResponse();
                        break;

                    case "discovery_charactgetdata":
                        response = new JsonObject
                        {
                            ["result_data"] = new JsonObject
                            {
                                ["$"] = Profile.CreateDiscoveryCharacterResponse()
                            }
                        };
                        break;

                    case "menucurrentdetachment":
                        response = EmptyResponse;
                        break;

                    case "detachmentabilitysave":
                        response = EmptyResponse;
                        break;

                    case "detachmentsave":
                        response = EmptyResponse;
                        break;

                    case "galaxymapload":
                        response = EmptyResponse;
                        break;

                    case "get_charact_stats":
                        response = EmptyResponse;
                        break;

                    case "set_charact_event_checked":
                        response = EmptyResponse;
                        break;

                    case "save_charact_progress_stats":
                        response = EmptyResponse;
                        break;

                    case "ship.save":
                        response = HandleSaveShipRequest(query);
                        e.Edited = true;
                        break;

                    case "disassemble_items":
                        response = EmptyResponse;
                        break;

                    case "sellinventory":
                        response = EmptyResponse;
                        break;

                    case "startcrafting":
                        response = HandleStartCraftingRequest(query);
                        e.Edited = true;
                        break;

                    case "acquireallcrafteditems":
                        response = HandleAcquireAllCraftedItemsRequest(query);
                        e.Edited = true;
                        break;

                    case "acquirecrafteditem":
                        response = HandleAcquireCraftedItemRequest(query);
                        e.Edited = true;
                        break;

                    case "ship.delete":
                        response = HandleShipDeleteRequest(query);
                        e.Edited = true;
                        break;

                    case "buy_battle_ground_shop_item":
                        response = EmptyResponse;
                        break;

                    default:
                        break;
                }
            });

            if ((response is null) == false)
                SendResponse(context, response);
        }

        protected void SendMatchmakerAuth()
        {
            Character character = Profile.CurrentCharacter;

            if (character is null)
                return;

            Matchmaker?.Send(PacketType.Auth, new JsonObject
            {
                ["name"] = character.Name,
            });
        }
    }
}
