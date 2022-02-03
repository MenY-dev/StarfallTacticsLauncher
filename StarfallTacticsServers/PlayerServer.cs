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
        public SfMgrChannelManager SfMgrChannel { get; protected set; }
        public RealmMgrServer RealmMgrServer { get; protected set; }
        public RealmMgrChannelManager RealmMgrChannel { get; protected set; }
        public GalaxyMgrChannel GalaxyMgrChannel { get; protected set; }
        public MatchmakerClient Matchmaker{ get; protected set; }

        public string MatchmakerAddress { get; set; } = "127.0.0.1:1300";
        public int MatchmakerId { get; protected set; } = -1;
        public string MatchmakerAuth { get; protected set; } = string.Empty;

        public override void Start()
        {
            if (IsStarded)
                Stop();

            if (SfMgrServer is null)
                SfMgrServer = new SfMgrServer();

            if (SfMgrChannel is null)
                SfMgrChannel = new SfMgrChannelManager();

            if (RealmMgrServer is null)
                RealmMgrServer = new RealmMgrServer();

            if (RealmMgrChannel is null)
                RealmMgrChannel = new RealmMgrChannelManager();

            if (GalaxyMgrChannel is null)
                GalaxyMgrChannel = new GalaxyMgrChannel();

            if (Matchmaker is null)
            {
                Matchmaker = new MatchmakerClient();
                Matchmaker.PacketReceived += HandleMatchmakerResponse;
            }

            SfMgrServer.PlayerServer = this;
            RealmMgrServer.PlayerServer = this;
            RealmMgrChannel.PlayerServer = this;

            SfMgrServer.Address = "http://127.0.0.1:1500/sfmgr/";
            SfMgrChannel.Address = "127.0.0.1:1000";
            RealmMgrServer.Address = "http://127.0.0.1:1500/realmmgr/";
            RealmMgrChannel.Address = "127.0.0.1:1200";
            GalaxyMgrChannel.Address = "127.0.0.1:1100";
            Matchmaker.Address = MatchmakerAddress;

            IsStarded = true;

            SfMgrServer.Start();
            SfMgrChannel.Start();
            RealmMgrServer.Start();
            RealmMgrChannel.Start();
            GalaxyMgrChannel.Start();
            Matchmaker.Start();
        }

        public override void Stop()
        {
            if (IsStarded == false)
                return;

            SfMgrServer?.Stop();
            SfMgrChannel?.Stop();
            RealmMgrServer?.Stop();
            RealmMgrChannel?.Stop();
            GalaxyMgrChannel?.Stop();
            Matchmaker.Stop();

            IsStarded = false;
        }

        private void HandleMatchmakerResponse(object sender, MatchmakerPacketEventArgs e)
        {
            MatchmakerPacket doc = e.Packet;

            if (doc?.Document is null)
                return;

            switch (doc.Type)
            {
                case PacketType.None:
                    Log($"Text Received: \"{doc.ParceDocument<string>()}\"");
                    break;

                case PacketType.AuthResponse:
                    HandlePlayerAuthResponse(doc);
                    break;

                case PacketType.Battle:
                    HandleBattle(doc.Document);
                    break;

                case PacketType.Chat:
                    HandleStartChat(doc.Document);
                    break;

                default:
                    break;
            }
        }

        protected virtual void HandlePlayerAuthResponse(MatchmakerPacket doc)
        {
            JsonNode response = doc.Document;

            MatchmakerId = (int)response["id"];
            MatchmakerAuth = (string)response["auth"];
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
            BattleGroundChannel channel = RealmMgrChannel.GetChannelByName("BattleGround") as BattleGroundChannel;

            if (channel is null)
                return;

            channel.SendBattleFound();
        }

        protected virtual void HandleStartBattle(JsonNode doc)
        {
            BattleGroundChannel channel = RealmMgrChannel.GetChannelByName("BattleGround") as BattleGroundChannel;

            if (doc is null || channel is null)
                return;

            channel.SendStartBattle(
                MatchmakerAddress.Split(':')[0],
                (ushort)doc["battle_port"],
                (string)doc["battle_auth"]);
        }

        protected virtual void HandleStartChat(JsonNode doc)
        {
            TextChatChannel channel = SfMgrChannel.GetChannelByName("GeneralTextChat") as TextChatChannel;

            if (doc is null || channel is null)
                return;

            channel.SendMessage(
                (string)doc["name"],
                (string)doc["msg"],
                false);
        }
    }
}
