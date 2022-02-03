using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Instances
{
    public class BattleInstance : Instance, IInstance
    {
        public List<PlayerInfo> Players { get; } = new List<PlayerInfo>();
        public List<CharacterInfo> Characters { get; } = new List<CharacterInfo>();

        public BattleInstance(string map, IEnumerable<PlayerInfo> players, string gameMode = null, bool isCustomGame = false)
        {
            IInstance instance = this;

            instance.InstanceMap = map ?? string.Empty;
            instance.GameMode = gameMode ?? string.Empty;
            instance.IsCustomGame = isCustomGame;

            if ((players is null) == false)
                Players.AddRange(players);
        }

        public BattleInstance(string map, IEnumerable<CharacterInfo> characters, string gameMode = null, bool isCustomGame = false)
        {
            IInstance instance = this;

            instance.InstanceMap = map ?? string.Empty;
            instance.GameMode = gameMode ?? string.Empty;
            instance.IsCustomGame = isCustomGame;

            if ((characters is null) == false)
                Characters.AddRange(characters);
        }

        public override JsonObject ToJsonObject()
        {
            JsonObject doc = base.ToJsonObject();
            JsonArray players = new JsonArray();
            JsonArray characters = new JsonArray();

            foreach (var item in Players)
            {
                players.Add(new JsonObject
                {
                    ["user_id"] = item.UserId,
                    ["name"] = item.Name ?? string.Empty,
                    ["auth"] = item.Auth ?? string.Empty,
                    ["fleet_id"] = item.FleetId,
                    ["team"] = item.Team
                });
            }

            foreach (var item in Characters)
            {
                characters.Add(new JsonObject
                {
                    ["id"] = item.Id,
                    ["name"] = item.Name ?? string.Empty,
                    ["auth"] = item.Auth ?? string.Empty,
                    ["team"] = item.Team,
                    ["role"] = item.Role,
                    ["faction"] = item.Faction,
                    ["party_id"] = item.PartyId,
                    ["in_galaxy"] = item.InGalaxy ? 1 : 0,
                    ["hex_offset_x"] = item.HexOffsetX,
                    ["hex_offset_y"] = item.HexOffsetY,
                    ["features"] = new JsonObject()
                });
            }

            doc["players_list"] = players;
            doc["characters_list"] = characters;

            return doc;
        }
    }
}
