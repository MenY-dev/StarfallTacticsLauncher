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
            base.HandleClient(client);

            lock (Locker)
            {
                Player player = GetPlayer(client);

                Log($"Disconnect: (Id = {player?.Id}, Name = {player?.Name})");

                if ((player is null) == false)
                {
                    player.Client = null;
                    player.InGame = false;

                    OnPlayerLeaves(new PlayerEventArgs(player));

                    SendToAll(PacketType.PlayerDisconnected, new JsonObject
                    {
                        ["id"] = player.Id,
                        ["name"] = player.Name,
                        ["full_name"] = player.FullName
                    });
                }
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

                case PacketType.PlayerStatus:
                    HandlePlayerStatus(client, doc);
                    break;

                case PacketType.PlayerStatusRequest:
                    HandlePlayerStatusRequest(client, doc);
                    break;

                case PacketType.PlayersInfoRequest:
                    HandlePlayersInfoRequest(client, doc);
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
            string playerName = (string)packet.Document["name"];

            if (string.IsNullOrWhiteSpace(playerName))
                playerName = "Player";

            Player player;

            lock (Locker)
            {
                int id = CreateId();

                player = new Player
                {
                    Client = client,
                    Id = id,
                    Auth = Guid.NewGuid().ToString("N"),
                    Name = playerName,
                    FullName = $"{id} {playerName}",
                    InGame = true,
                    Status = PlayerStatus.Menu
                };

                Players.Add(player);
            }

            player.Send(PacketType.PlayerAuthResponse, new JsonObject
            {
                ["id"] = player.Id,
                ["auth"] = player.Auth,
                ["name"] = player.Name,
                ["full_name"] = player.FullName,
            });

            Log($"Auth: (Name = {playerName}, Id = {player.Id})");
        }

        protected void HandlePlayerJoined(TcpClient client, MatchmakerPacket packet)
        {
            Player player = GetPlayer(client);

            if (player is null || player.Id < 0)
                return;

            player.InGame = true;

            SendToAll(PacketType.PlayerJoined, new JsonObject
            {
                ["id"] = player.Id,
                ["name"] = player.Name,
                ["full_name"] = player.FullName
            });

            Log($"Player Joined: (Name = {player.FullName}, Id = {player.Id})");
        }

        protected void HandlePlayerStatus(TcpClient client, MatchmakerPacket packet)
        {
            JsonNode doc = packet?.Document;
            Player player = GetPlayer(client);

            if (doc is null || player is null)
                return;

            player.Status = (PlayerStatus?)(int?)doc["status"] ?? PlayerStatus.Offline;

            SendToAll(PacketType.PlayerStatus, new JsonObject
            {
                ["name"] = player.Name,
                ["full_name"] = player.FullName,
                ["in_game"] = player.InGame,
                ["status"] = (int)player.Status
            });
        }

        protected void HandlePlayerStatusRequest(TcpClient client, MatchmakerPacket packet)
        {
            JsonNode doc = packet?.Document;
            Player player = GetPlayer(client);

            if (doc is null || player is null)
                return;

            string name = (string)doc["full_name"] ?? string.Empty;
            Player requiredPlayer = GetPlayerWithFullName(name);

            if (requiredPlayer is null)
                return;

            player.Send(PacketType.PlayerStatus, new JsonObject
            {
                ["name"] = requiredPlayer.Name,
                ["full_name"] = requiredPlayer.FullName,
                ["in_game"] = requiredPlayer.InGame,
                ["status"] = (int)requiredPlayer.Status
            });
        }

        protected void HandlePlayersInfoRequest(TcpClient client, MatchmakerPacket packet)
        {
            JsonNode doc = packet?.Document;
            Player player = GetPlayer(client);

            if (doc is null || player is null)
                return;

            JsonArray players = new JsonArray();

            foreach (var item in Players)
            {
                players.Add(new JsonObject
                {
                    ["name"] = item.Name,
                    ["full_name"] = item.FullName,
                    ["in_game"] = item.InGame,
                    ["status"] = item.InGame ? (int)item.Status : (int)PlayerStatus.Offline
                });
            }

            player.Send(PacketType.PlayersInfo, new JsonObject
            {
                ["players"] = players
            });
        }

        protected void HandleChat(TcpClient client, MatchmakerPacket packet)
        {
            JsonNode doc = packet?.Document;
            Player player = GetPlayer(client);

            if (doc is null || player is null)
                return;

            string name = player.FullName;
            string msg = (string)doc["msg"];

            if (msg is null || name is null)
                return;

            SendToAll(PacketType.Chat, new JsonObject
            {
                ["name"] = name,
                ["msg"] = msg
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

        public void SendToAll(PacketType packetType, JsonNode packet, bool waitForCompleteon = false)
        {
            Task task = SendToAllAsync(packetType, packet);

            if (waitForCompleteon == true)
                task.Wait();
        }

        public async Task SendToAllAsync(PacketType packetType, JsonNode packet)
        {
            if (packet is null)
                return;

            JsonNode response = packet.Clone();

            await Task.Factory.StartNew(() =>
            {
                foreach (var item in Players)
                {
                    if (item?.InGame == true)
                        item.Send(packetType, response.Clone());
                }
            });
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


        public virtual Player GetPlayer(IPEndPoint address)
        {
            if (address is null)
                return null;

            foreach (var item in Players)
            {
                IPEndPoint endPoint = item?.Client?.Client.RemoteEndPoint as IPEndPoint;

                if (endPoint?.Equals(address) == true)
                    return item;
            }

            return null;
        }

        public virtual Player GetPlayerWithFullName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            foreach (var item in Players)
                if (item.FullName == name)
                    return item;

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
