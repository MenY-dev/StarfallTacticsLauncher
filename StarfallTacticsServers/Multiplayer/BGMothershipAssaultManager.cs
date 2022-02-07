using StarfallTactics.StarfallTacticsServers.Debugging;
using StarfallTactics.StarfallTacticsServers.Instances;
using StarfallTactics.StarfallTacticsServers.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public class BGMothershipAssaultManager : GameModeManager
    {
        public int RoomSize { get; set; } = 2;

        public List<Room> Rooms { get; } = new List<Room>();

        protected object Locker { get; } = new object();


        public BGMothershipAssaultManager(MatchmakerServer matchmaker) : base(matchmaker)
        {

        }

        public override void Input(Player player, JsonNode packet)
        {
            lock (Locker)
            {
                switch ((string)packet["command"])
                {
                    case BattleAction.Search:
                        HandleSearchRequest(player, packet);
                        break;

                    case BattleAction.Confirm:
                        HandleBattleConfirm(player, packet);
                        break;

                    case BattleAction.Cancel:
                        HandleBattleCancell(player, packet);
                        break;

                    default:
                        break;
                }
            }
        }

        protected void HandleSearchRequest(Player player, JsonNode packet)
        {
            if (packet is null || player is null || player.Id < 0)
                return;

            this.Log($"Request Battle: (Player = {player.Name}, Id = {player.Id}, Auth = {player.Auth})");

            DisconnectPlayer(player);
            ConnectPlayer(player);
        }

        protected void HandleBattleConfirm(Player player, JsonNode packet)
        {
            if (packet is null || player is null || player.Id < 0)
                return;

            Room room = GetRoom(player);

            if (room is null)
                return;

            room[player].IsReady = true;
            player.DiscoveryCharacterData = packet["discovery_character_data"]?.Clone();

            if (CheckRoomReadyToStart(room) == false)
                return;

            room.State = RoomState.Starting;

            List<CharacterInfo> characters = new List<CharacterInfo>();
            int playerIndex = 0;

            foreach (var item in room.Queue.Keys)
            {
                characters.Add(new CharacterInfo
                {
                    Id = item.Id,
                    Auth = item.Auth,
                    Name = item.Name,
                    Faction = 0,
                    Team = playerIndex % 2
                });

                playerIndex++;
            }

            BattleInstance instance = new BattleInstance(
                new Random().Next() % 2 == 0 ? "bg_MothershipAssault" : "bg_MothershipAssault_2",
                characters);

            room.Instance = instance;
            instance.StateChanged += OnInstanceStateChanged;
            InstanceManager.StartInstance(instance);

            this.Log($"Start Battle: (Room = {room.Id}, Size = {room.Queue.Count})");
        }

        protected void HandleBattleCancell(Player player, JsonNode packet)
        {
            if (packet is null || player is null || player.Id < 0)
                return;

            DisconnectPlayer(player);
        }

        protected virtual void OnInstanceStateChanged(object sender, InstanceStateEventArgs e)
        {
            lock (Locker)
            {
                if (e.State != InstanceState.ReadyToConnect)
                    return;

                BattleInstance instance = e.Instance as BattleInstance;
                Room room = GetRoom(instance);

                if (instance is null || room?.State != RoomState.Starting)
                    return;

                if (Rooms.Contains(room))
                    Rooms.Remove(room);

                if ((instance.Characters?.Count ?? 0) > 0)
                {
                    foreach (var character in instance.Characters)
                    {
                        Player player = room.GetPlayer(character.Auth);

                        if (player is null)
                            continue;

                        player.Send(PacketType.Battle, new JsonObject
                        {
                            ["id"] = player.Id,
                            ["auth"] = player.Auth,
                            ["command"] = BattleAction.Start,
                            ["game_mode"] = GameMode.BGMothershipAssaul,
                            ["room"] = room.Id,
                            ["battle_port"] = instance.InstancePort,
                            ["battle_auth"] = character.Auth,
                        });
                    }

                    this.Log($"Instance Started: (Map = {e.Instance.InstanceMap}, Auth = {e.Instance.Auth}, Port = {e.Instance.InstancePort})");
                }
            }
        }

        public virtual void ConnectPlayer(Player player)
        {
            if (player is null)
                return;

            Room room = GetFreeRoom();

            if (room is null)
                room = CreateEmptyRoom(RoomSize);

            room.AddPlayer(player);

            if (room.Queue.Count >= room.Size)
            {
                room.State = RoomState.Awaiting;
                Task.Factory.StartNew(() => SendConfirmation(room));
            }
        }

        public virtual void ConnectPlayers(IEnumerable<Player> players)
        {
            if (players is null)
                return;

            foreach (var player in players)
                ConnectPlayer(player);
        }

        public virtual void DisconnectPlayer(Player player)
        {
            Room room = GetRoom(player);

            if (room is null)
                return;

            switch (room.State)
            {
                case RoomState.Open:
                    room.RemovePlayer(player);

                    if (room.Queue.Count < 1 && Rooms.Contains(room))
                        Rooms.Remove(room);

                    break;

                case RoomState.Awaiting:
                    room.RemovePlayer(player);
                    Rooms.Remove(room);
                    ConnectPlayers(room.Queue?.Keys);
                    break;

                default:
                    break;
            }
        }

        public Room CreateEmptyRoom(int size)
        {
            Room room = new Room
            {
                State = RoomState.Open,
                Size = size
            };

            Rooms.Add(room);
            return room;
        }

        public Room GetFreeRoom()
        {
            foreach (var item in Rooms)
                if (item.Queue.Count < item.Size)
                    return item;

            return null;
        }

        public Room GetRoom(Player player)
        {
            foreach (var room in Rooms)
            {
                if ((room[player] is null) == false)
                    return room;
            }

            return null;
        }

        public Room GetRoom(IInstance instance)
        {
            foreach (var room in Rooms)
            {
                if (room.Instance == instance)
                    return room;
            }

            return null;
        }

        public Room GetRoom(Guid id)
        {
            foreach (var room in Rooms)
            {
                if (room.Id == id)
                    return room;
            }

            return null;
        }

        public virtual void SendConfirmation(Room room)
        {
            SendConfirmation(room?.Queue.Keys);
        }

        public virtual void SendConfirmation(IEnumerable<Player> players)
        {
            if (players is null)
                return;

            foreach (var player in players)
                SendConfirmation(player);
        }

        public virtual void SendConfirmation(Player player)
        {
            player?.Send(PacketType.Battle, new JsonObject
            {
                ["id"] = player.Id,
                ["auth"] = player.Auth,
                ["command"] = BattleAction.Found,
                ["game_mode"] = GameMode.BGMothershipAssaul,
                ["room"] = GetRoom(player)?.Id
            });
        }

        protected virtual bool CheckRoomReadyToStart(Room room)
        {
            int readyPlayers = 0;

            foreach (var item in room.Queue)
                if (item.Value.IsReady == true)
                    readyPlayers++;

            return readyPlayers >= room.Size;
        }

        protected override void OnPlayerLeaves(object sender, PlayerEventArgs e)
        {
            base.OnPlayerLeaves(sender, e);
            DisconnectPlayer(e.Player);
        }
    }
}
