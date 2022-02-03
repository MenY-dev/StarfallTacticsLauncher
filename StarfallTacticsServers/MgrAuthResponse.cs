using StarfallTactics.StarfallTacticsServers.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class MgrAuthResponse
    {
        public MgrAuthResponse(string address, string port)
        {
            Address = address;
            Port = port;
        }

        [JsonPropertyName("address"), SValue]
        public string Address { get; set; } = "127.0.0.1";

        [JsonPropertyName("port"), SValue]
        public string Port { get; set; } = "1000";

        [JsonPropertyName("temporarypass"), SValue]
        public string TemporaryPass { get; set; } = "a0b1c2d3e4f5g6h7i8j9k10l";

        [JsonPropertyName("auth"), SValue]
        public string Auth { get; set; } = "a0b1c2d3e4f5g6h7i8j9k10l";

        [JsonPropertyName("tutorial_complete"), SValue]
        public bool TutorialComplete { get; set; } = true;

        [JsonPropertyName("realmname"), SValue]
        public string RealmName { get; set; } = "NewRealm";

        [JsonPropertyName("userbm"), SValue]
        public int UserBm { get; set; } = 1;
    }
}
