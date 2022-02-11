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
    public class CharactPartyChannel : Channel
    {
        public CharactPartyChannel(ChannelManager channelManager, string name, int id) : base(channelManager, name, id)
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

                    switch (command)
                    {
                        case 0:
                            int nameSize = inputReader.ReadUInt16();
                            string name = Encoding.UTF8.GetString(inputReader.ReadBytes(nameSize));
                            SendInviteResponse(name, false);
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

        public void SendInviteResponse(string name, bool accepted = false)
        {
            using (MemoryStream stream = new MemoryStream())
            using (SfBinaryWriter writer = new SfBinaryWriter(stream))
            {
                writer.WriteByte(1);
                writer.WriteText(name, true, Encoding.UTF8);
                writer.WriteByte(accepted ? (byte)1 : (byte)0);

                byte[] data = stream.ToArray();
                SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, (ushort)data.Length);
                Send(packet, data);
            }
        }
    }
}
