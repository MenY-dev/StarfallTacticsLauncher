using StarfallTactics.StarfallTacticsServers.Debugging;
using StarfallTactics.StarfallTacticsServers.Multiplayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class BattleGroundChannel : Channel
    {
        public BattleGroundChannel(ChannelManager channelManager, string name, int id) : base(channelManager, name, id)
        {

        }

        public override void Input(byte[] data)
        {
            if (data.Length < 1)
                return;

            using (MemoryStream inputStream = new MemoryStream(data))
            using (BinaryReader inputReader = new BinaryReader(inputStream))
            {
                try
                {
                    int command = inputReader.ReadByte();
                    var profile = Profile;

                    switch (command)
                    {
                        case 0:
                            profile?.Use(e =>
                            {
                                Matchmaker.Send(PacketType.Battle, new JsonObject
                                {
                                    ["id"] = profile.MatchmakerId,
                                    ["auth"] = profile.MatchmakerAuth,
                                    ["game_mode"] = GameMode.BGMothershipAssaul,
                                    ["command"] = BattleAction.Confirm,
                                    ["discovery_character_data"] = Profile.CreateDiscoveryCharacterResponse()
                                });
                            });
                            break;

                        case 1:
                            Matchmaker.Send(PacketType.Battle, new JsonObject
                            {
                                ["id"] = profile?.MatchmakerId ?? -1,
                                ["auth"] = profile?.MatchmakerAuth,
                                ["game_mode"] = GameMode.BGMothershipAssaul,
                                ["command"] = BattleAction.Cancel
                            });
                            break;

                        case 2:
                            Matchmaker.Send(PacketType.Battle, new JsonObject
                            {
                                ["id"] = profile?.MatchmakerId ?? -1,
                                ["auth"] = profile?.MatchmakerAuth,
                                ["game_mode"] = GameMode.BGMothershipAssaul,
                                ["command"] = BattleAction.Search,
                            });
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    this.Log(e);
                }
            }
        }

        public virtual void SendBattleFound()
        {
            SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, 1);
            byte[] data = new byte[1] { 3 };
            Send(packet, data);
            this.Log("Battle Found!");
        }

        public void SendStartBattle(string addres, ushort port, string auth)
        {
            SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, 12);
            byte[] buffer = new byte[1024];
            byte[] data;

            using (SfBinaryWriter writer = new SfBinaryWriter(buffer))
            {
                writer.WriteByte(0);
                writer.WriteText(addres, true, Encoding.ASCII);
                writer.WriteUInt16(port);
                writer.WriteText(auth, true, Encoding.ASCII);

                data = new byte[writer.Stream.Position];
                Array.Copy(buffer, data, data.Length);
            }

            Send(packet, data);
            this.Log("Start Battle!");
        }
    }
}
