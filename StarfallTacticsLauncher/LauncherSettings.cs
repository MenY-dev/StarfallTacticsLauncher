using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsLauncher
{
    public class LauncherSettings
    {
        [JsonPropertyName("game_location")]
        public string GameLocation { get; set; } = "";

        [JsonPropertyName("enable_multiplayer")]
        public bool EnableMultiplayer { get; set; } = false;

        [JsonPropertyName("multiplayer_address")]
        public string MultiplayerAddress { get; set; } = "127.0.0.1";

        [JsonPropertyName("multiplayer_port")]
        public int MultiplayerPort { get; set; } = 1300;

        [JsonPropertyName("show_game_log")]
        public bool ShowGameLog { get; set; } = false;

        [JsonPropertyName("server_settings")]
        public ServerSettings ServerSettings { get; set; } = new ServerSettings();
    }

    public class ServerSettings
    {
        [JsonPropertyName("address")]
        public string Address { get; set; } = "127.0.0.1";

        [JsonPropertyName("port")]
        public int Port { get; set; } = 1300;

        [JsonPropertyName("battlegrounds_settings")]
        public BattlegroundsSettings BattlegroundsMode { get; set; } = new BattlegroundsSettings();
    }

    public class BattlegroundsSettings
    {
        [JsonPropertyName("room_size")]
        public int RoomSize { get; set; } = 6;
    }
}
