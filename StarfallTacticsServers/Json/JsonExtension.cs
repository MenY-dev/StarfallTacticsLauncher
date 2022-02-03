using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Json
{
    public static class JsonExtension
    {
        public static JsonNode Clone(this JsonNode node)
        {
            return JsonNode.Parse(node.ToJsonString());
        }
    }
}
