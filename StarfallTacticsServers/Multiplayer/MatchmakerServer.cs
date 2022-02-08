using StarfallTactics.StarfallTacticsServers.Instances;
using StarfallTactics.StarfallTacticsServers.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public class MatchmakerServer : MessagingServer
    {
        public InstanceManager InstanceManager { get; set; }

        public BattleMgrServer BattleMgrServer { get; set; }

        public BGMothershipAssaultManager BGMothershipAssaultManager { get; }

        public event EventHandler<PlayerEventArgs> PlayerLeaves;

        protected List<Player> Players { get; } = new List<Player>();

        protected Dictionary<string, GameModeManager> GameModeManagers { get; } = new Dictionary<string, GameModeManager>();

        protected object Locker { get; } = new object();

        public MatchmakerServer()
        {
            BGMothershipAssaultManager = new BGMothershipAssaultManager(this);
            GameModeManagers.Add(GameMode.BGMothershipAssaul, BGMothershipAssaultManager);
        }

        protected override void HandleClient(TcpClient client)
        {
            Player player;

            lock (Locker)
            {
                IPEndPoint endPoint = client?.Client.RemoteEndPoint as IPEndPoint;
                player = GetPlayer(endPoint?.Address);

                if (player is null)
                {
                    int id = CreateId();

                    player = new Player
                    {
                        Client = client,
                        Id = id,
                        Auth = Guid.NewGuid().ToString("N")
                    };

                    Players.Add(player);
                }
                else
                {
                    player.Client = client;
                    Log($"Reconnect: (Id = {player.Id}, Name = {player.Name})");
                }
            }

            player.Send(PacketType.AuthRequest, new JsonObject());

            base.HandleClient(client);

            lock (Locker)
            {
                OnPlayerLeaves(new PlayerEventArgs(player));
            }
        }
         
        protected override void HandleInputPacket(TcpClient client, string packet)
        {
            MatchmakerPacket doc = MatchmakerPacket.Parce(packet);

            if (doc?.Document is null)
                return;

            switch (doc.Type)
            {
                case PacketType.None:
                    Log($"Text Received: \"{packet}\"");
                    break;

                case PacketType.PlayerAuth:
                    HandlePlayerAuth(client, doc);
                    break;

                case PacketType.PlayerJoined:
                    HandlePlayerJoined(client, doc);
                    break;

                case PacketType.Battle:
                    HandleBattle(client, doc);
                    break;

                case PacketType.Chat:
                    HandleChat(client, doc);
                    break;

                default:
                    break;
            }
        }


        protected void HandleBattle(TcpClient client, MatchmakerPacket packet)
        {
            JsonNode doc = packet?.Document;
            Player player = GetPlayer(client);

            if (doc is null || player is null)
                return;

            string gameMode = (string)doc["game_mode"] ?? string.Empty;
            GameModeManager manager;

            if (GameModeManagers.TryGetValue(gameMode, out manager))
            {
                manager.Input(player, doc);
            }
        }

        protected void HandlePlayerAuth(TcpClient client, MatchmakerPacket packet)
        {
            Player player = GetPlayer(client);
            string playerName = (string)packet.Document["name"];

            if (player is null || player.Id < 0 || string.IsNullOrWhiteSpace(playerName))
                return;

            player.Name = playerName;

            player.Send(PacketType.PlayerAuthResponse, new JsonObject
            {
                ["id"] = player.Id,
                ["auth"] = player.Auth
            });

            Log($"Auth: (Name = {playerName}, Id = {player.Id})");
        }

        protected void HandlePlayerJoined(TcpClient client, MatchmakerPacket packet)
        {
            Player player = GetPlayer(client);
            string playerName = (string)packet.Document["name"];

            if (player is null || player.Id < 0 || string.IsNullOrWhiteSpace(playerName))
                return;

            Log($"Player Joined: (Name = {playerName}, Id = {player.Id})");
            SendPlayerConnectionMessage(player);
        }

        protected void HandleChat(TcpClient client, MatchmakerPacket packet)
        {
            JsonNode doc = packet?.Document;
            Player player = GetPlayer(client);

            if (doc is null || player is null)
                return;

            string name = player.Name;
            string msg = (string)doc["msg"];

            if (msg is null || name is null)
                return;

            Task.Factory.StartNew(() =>
            {
                foreach (var item in Players)
                {
                    item?.Send(PacketType.Chat, new JsonObject
                    {
                        ["id"] = item.Id,
                        ["auth"] = item.Auth,
                        ["name"] = name,
                        ["msg"] = msg
                    });
                }
            });
        }

        public void SendSystemMessage(string msg)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var item in Players)
                    SendSystemMessage(item, msg);
            });
        }

        public void SendSystemMessage(Player player, string msg)
        {
            player?.Send(PacketType.SystemMessage, new JsonObject
            {
                ["id"] = player.Id,
                ["auth"] = player.Auth,
                ["msg"] = msg
            });
        }

        public void SendPlayerConnectionMessage(Player player)
        {
            if (string.IsNullOrWhiteSpace(player?.Name))
                return;

            SendSystemMessage($"{player.Name} has joined!");
        }

        public static void Send(TcpClient client, PacketType packetType, JsonNode packet)
        {
            Send(client, MatchmakerPacket.Create(packetType, packet));
        }

        public static void Send(Player player, string packet)
        {
            Send(player?.Client, packet);
        }

        public static void Send(Player player, PacketType packetType, JsonNode packet)
        {
            Send(player?.Client, MatchmakerPacket.Create(packetType, packet));
        }

        public virtual Player GetPlayer(TcpClient client)
        {
            foreach (var item in Players)
                if (item.Client == client)
                    return item;

            return null;
        }

        public virtual Player GetPlayer(int id)
        {
            if (id < 0)
                return null;

            foreach (var item in Players)
                if (item.Id == id)
                    return item;

            return null;
        }

        public virtual Player GetPlayer(string auth)
        {
            if (string.IsNullOrWhiteSpace(auth))
                return null;

            foreach (var item in Players)
                if (item.Auth == auth)
                    return item;

            return null;
        }


        public virtual Player GetPlayer(IPAddress address)
        {
            if (address is null)
                return null;

            foreach (var item in Players)
            {
                IPEndPoint endPoint = item?.Client?.Client.RemoteEndPoint as IPEndPoint;

                if (endPoint?.Address.Equals(address) == true)
                    return item;
            }

            return null;
        }

        protected virtual int CreateId()
        {
            int id = 0;

            for (int i = 0; i < Players.Count; i++)
            {
                bool isEmpty = true;

                foreach (var item in Players)
                {
                    if (item.Id == id)
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty == true)
                    return id;

                id++;
            }

            return id;
        }

        protected virtual string CreateAuthFromId(int id)
        {
            return $"{id:D16}";
        }

        public override void Start()
        {
            base.Start();

            if (BattleMgrServer is null)
                BattleMgrServer = new BattleMgrServer();

            BattleMgrServer.Address = "http://127.0.0.1:1600/battlemgr/";
            BattleMgrServer.Matchmaker = this;
            BattleMgrServer.Start();
        }

        public override void Stop()
        {
            base.Stop();

            BattleMgrServer?.Stop();
        }

        protected virtual void OnPlayerLeaves(PlayerEventArgs args)
        {
            PlayerLeaves?.Invoke(this, args);
        }
    }
}
