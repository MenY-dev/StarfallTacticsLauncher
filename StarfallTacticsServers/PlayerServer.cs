using StarfallTactics.StarfallTacticsServers.Multiplayer;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class PlayerServer : BaseServer
    {
        public StarfallProfile Profile { get; set; }
        public SfMgrServer SfMgrServer { get; protected set; }
        public SfMgrChannelManager SfMgrChannels { get; protected set; }
        public RealmMgrServer RealmMgrServer { get; protected set; }
        public RealmMgrChannelManager RealmMgrChannels { get; protected set; }
        public GalaxyMgrChannel GalaxyMgrChannels { get; protected set; }
        public MatchmakerClient Matchmaker{ get; protected set; }

        public string MatchmakerAddress { get; set; } = "127.0.0.1:1300";

        public override void Start()
        {
            if (IsStarded)
                Stop();

            if (SfMgrServer is null)
                SfMgrServer = new SfMgrServer();

            if (SfMgrChannels is null)
                SfMgrChannels = new SfMgrChannelManager();

            if (RealmMgrServer is null)
                RealmMgrServer = new RealmMgrServer();

            if (RealmMgrChannels is null)
                RealmMgrChannels = new RealmMgrChannelManager();

            if (GalaxyMgrChannels is null)
                GalaxyMgrChannels = new GalaxyMgrChannel();

            if (Matchmaker is null)
            {
                Matchmaker = new MatchmakerClient();
                Matchmaker.PacketReceived += HandleMatchmakerResponse;
            }

            SfMgrServer.PlayerServer = this;
            SfMgrChannels.PlayerServer = this;
            RealmMgrServer.PlayerServer = this;
            RealmMgrChannels.PlayerServer = this;

            SfMgrServer.Address = "http://127.0.0.1:1500/sfmgr/";
            SfMgrChannels.Address = "127.0.0.1:1000";
            RealmMgrServer.Address = "http://127.0.0.1:1500/realmmgr/";
            RealmMgrChannels.Address = "127.0.0.1:1200";
            GalaxyMgrChannels.Address = "127.0.0.1:1100";
            Matchmaker.Address = MatchmakerAddress;

            IsStarded = true;

            SfMgrServer.Start();
            SfMgrChannels.Start();
            RealmMgrServer.Start();
            RealmMgrChannels.Start();
            GalaxyMgrChannels.Start();
            Matchmaker.Start();
        }

        public override void Stop()
        {
            if (IsStarded == false)
                return;

            SfMgrServer?.Stop();
            SfMgrChannels?.Stop();
            RealmMgrServer?.Stop();
            RealmMgrChannels?.Stop();
            GalaxyMgrChannels?.Stop();
            Matchmaker.Stop();

            IsStarded = false;
        }

        private void HandleMatchmakerResponse(object sender, MatchmakerPacketEventArgs e)
        {
            MatchmakerPacket doc = e.Packet;

            if (doc?.Document is null)
                return;

            Log($"Matchmaker Response: (Type = {doc.Type})");

            switch (doc.Type)
            {
                case PacketType.None:
                    Log($"Text Received: \"{doc.ParceDocument<string>()}\"");
                    break;

                case PacketType.PlayerAuthResponse:
                    HandlePlayerAuthResponse(doc);
                    break;

                case PacketType.AuthRequest:
                    HandleAuthRequest(doc);
                    break;

                case PacketType.PlayersInfo:
                    HandlePlayersInfoResponse(doc);
                    break;

                case PacketType.PlayerJoined:
                    HandlePlayerJoinedResponse(doc);
                    break;

                case PacketType.PlayerDisconnected:
                    HandlePlayerDisconnectedResponse(doc);
                    break;

                case PacketType.PlayerStatus:
                    HandlePlayerStatusResponse(doc);
                    break;

                case PacketType.Battle:
                    HandleBattle(doc.Document);
                    break;

                case PacketType.Chat:
                    HandleChat(doc.Document);
                    break;

                case PacketType.SystemMessage:
                    HandleSystemMessage(doc.Document);
                    break;

                default:
                    break;
            }
        }

        protected virtual void HandlePlayerStatusResponse(MatchmakerPacket doc)
        {
            JsonNode response = doc.Document;
            FriendChannel channel = SfMgrChannels.GetChannelByName("UserFriends") as FriendChannel;

            if (response is null || channel is null)
                return;

            string name = (string)response["full_name"];
            PlayerStatus status = (PlayerStatus?)(int?)response["status"] ?? PlayerStatus.Offline;

            if (string.IsNullOrWhiteSpace(name))
                return;

            channel.SendFriendStatus(name, true, status);
        }

        protected virtual void HandlePlayerDisconnectedResponse(MatchmakerPacket doc)
        {
            JsonNode response = doc.Document;
            FriendChannel channel = SfMgrChannels.GetChannelByName("UserFriends") as FriendChannel;

            if (response is null || channel is null)
                return;

            string name = (string)response["full_name"];

            if (string.IsNullOrWhiteSpace(name))
                return;

            channel.SendRemoveFromFriends(name);
        }

        protected virtual void HandlePlayerJoinedResponse(MatchmakerPacket doc)
        {
            JsonNode response = doc.Document;
            FriendChannel channel = SfMgrChannels.GetChannelByName("UserFriends") as FriendChannel;

            if (response is null || channel is null)
                return;

            string name = (string)response["full_name"];

            if (string.IsNullOrWhiteSpace(name))
                return;

            channel.SendAcceptNewFriend(name);
            channel.SendFriendStatus(name, true, PlayerStatus.Menu);
        }

        protected virtual void HandlePlayersInfoResponse(MatchmakerPacket doc)
        {
            JsonNode response = doc.Document;
            FriendChannel channel = SfMgrChannels.GetChannelByName("UserFriends") as FriendChannel;

            if (response is null || channel is null)
                return;

            JsonArray players = response["players"]?.AsArray();

            if (players is null)
                return;

            foreach (var player in players)
            {
                string name = (string)player?["full_name"];

                if (string.IsNullOrWhiteSpace(name))
                    continue;

                channel.SendAcceptNewFriend(
                    name,
                    (bool?)player["in_game"] ?? false,
                    (PlayerStatus?)(int?)player["status"] ?? PlayerStatus.Offline);
            }
        }

        protected virtual void HandlePlayerAuthResponse(MatchmakerPacket doc)
        {
            JsonNode response = doc.Document;
            StarfallProfile profile = Profile;

            if (response is null || profile is null)
                return;

            profile.MatchmakerId = (int?)response["id"] ?? -1;
            profile.MatchmakerAuth = (string)response["auth"];
        }

        protected virtual void HandleAuthRequest(MatchmakerPacket doc)
        {
            SendMatchmakerAuth();
            Matchmaker?.Send(PacketType.PlayersInfoRequest, new JsonObject());
        }

        protected virtual void HandleBattle(JsonNode doc)
        {
            switch ((string)doc["command"])
            {
                case BattleAction.Found:
                    HandleBattleFound(doc);
                    break;

                case BattleAction.Start:
                    HandleStartBattle(doc);
                    break;

                default:
                    break;
            }
        }

        protected virtual void HandleBattleFound(JsonNode doc)
        {
            BattleGroundChannel channel = RealmMgrChannels.GetChannelByName("BattleGround") as BattleGroundChannel;

            if (channel is null)
                return;

            channel.SendBattleFound();
        }

        protected virtual void HandleStartBattle(JsonNode doc)
        {
            BattleGroundChannel channel = RealmMgrChannels.GetChannelByName("BattleGround") as BattleGroundChannel;

            if (doc is null || channel is null)
                return;

            channel.SendStartBattle(
                MatchmakerAddress.Split(':')[0],
                (ushort)doc["battle_port"],
                (string)doc["battle_auth"]);
        }

        protected virtual void HandleChat(JsonNode doc)
        {
            TextChatChannel channel = SfMgrChannels.GetChannelByName("GeneralTextChat") as TextChatChannel;

            if (doc is null || channel is null)
                return;

            channel.SendMessage(
                (string)doc["name"],
                (string)doc["msg"],
                false);
        }

        protected virtual void HandleSystemMessage(JsonNode doc)
        {
            TextChatChannel channel = SfMgrChannels.GetChannelByName("GeneralTextChat") as TextChatChannel;

            if (doc is null || channel is null)
                return;

            channel.SendMessage(
                "SERVER",
                (string)doc["msg"],
                true);
        }

        public virtual void SendMatchmakerAuth()
        {
            Matchmaker?.Send(PacketType.PlayerAuth, new JsonObject
            {
                ["name"] = Profile.Nickname,
            });
        }
    }
}
