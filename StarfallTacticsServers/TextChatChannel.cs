using StarfallTactics.StarfallTacticsServers.Debugging;
using StarfallTactics.StarfallTacticsServers.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class TextChatChannel : Channel
    {
        public TextChatChannel(ChannelManager channelManager, string name, int id) : base(channelManager, name, id)
        {

        }

        public override void Input(string text)
        {
            base.Input(text);

            if (text is null || text.Length < 1)
                return;

            string msg = text.Substring(1, text.Length - 1);

            switch (text[0])
            {
                case 'A':
                    Matchmaker?.Send(PacketType.Chat, new JsonObject
                    {
                        ["id"] = Profile?.MatchmakerId ?? -1,
                        ["auth"] = Profile?.MatchmakerAuth,
                        ["msg"] = msg
                    });
                    this.Log($"Public Message: (Msg = {msg})");
                    break;

                case 'U':
                    this.Log($"Private Message: (Msg = {msg})");
                    break;

                default:
                    this.Log($"Unknown Message: (Msg = {text})");
                    break;
            }
        }

        public void SendMessage(string name, string text, bool privateMsg = false)
        {
            string tag = privateMsg ? "U" : "A";
            Send(new SFCP.TextPacket(Id), $"{tag}{name}:{text}");
        }
    }
}
