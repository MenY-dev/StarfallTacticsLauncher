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
    public class FriendChannel : Channel
    {
        public FriendChannel(ChannelManager channelManager, string name, int id) : base(channelManager, name, id)
        {
        }

        public override void Register()
        {
            base.Register();

            //SendAcceptNewFriend("0_MenY 0", false, 4);
            //SendAcceptNewFriend("147_MenY 1", true, 0);
            //SendAcceptNewFriend("2_MenY 2", true, 1);
            //SendAcceptNewFriend("3_MenY 3", true, 2);
            //SendAcceptNewFriend("15_MenY 4", true, 3);

            //SendAcceptNewFriend("MenY", false, 4);
            //SendAcceptNewFriend("MenY2", true, 0);
            //SendFriendStatus("MenY", true, 0);
            //SendFriendStatus("MenY", true, 0);
            //SendFriendStatus("MenY", true, 0);
            //SendFriendStatus("MenY2", false, 4);
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

                    if (command == 0)
                    {
                        int nameSize = inputReader.ReadUInt16();
                        string name = Encoding.UTF8.GetString(inputReader.ReadBytes(nameSize));
                        SendFriendRequestResult(name, 1);
                    }
                    else if (command == 3)
                    {
                        int nameSize = inputReader.ReadUInt16();
                        string name = Encoding.UTF8.GetString(inputReader.ReadBytes(nameSize));
                        SendAcceptNewFriend(name);
                    }
                    else if (command == 4)
                    {
                        PlayerStatus status = (PlayerStatus)inputReader.ReadByte();

                        PlayerServer?.Matchmaker?.Send(PacketType.PlayerStatus, new JsonObject
                        {
                            ["status"] = (int)status
                        });
                    }
                }
                catch (Exception e)
                {
                    this.Log(e);
                }
            }
        }

        public void SendAcceptNewFriend(string name, bool inGame = false, PlayerStatus status = PlayerStatus.Offline)
        {
            using (MemoryStream stream = new MemoryStream())
            using (SfBinaryWriter writer = new SfBinaryWriter(stream))
            {
                writer.WriteByte(0);
                writer.WriteText(name, true, Encoding.UTF8);
                writer.WriteByte(inGame ? (byte)1 : (byte)0);
                writer.WriteUInt16((ushort)(int)status);

                byte[] data = stream.ToArray();
                SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, (ushort)data.Length);
                Send(packet, data);
            }
        }

        public void SendAddToFriends(string name)
        {
            using (MemoryStream stream = new MemoryStream())
            using (SfBinaryWriter writer = new SfBinaryWriter(stream))
            {
                writer.WriteByte(1);
                writer.WriteText(name, true, Encoding.UTF8);

                byte[] data = stream.ToArray();
                SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, (ushort)data.Length);
                Send(packet, data);
            }
        }

        public void SendRemoveFromFriends(string name)
        {
            using (MemoryStream stream = new MemoryStream())
            using (SfBinaryWriter writer = new SfBinaryWriter(stream))
            {
                writer.WriteByte(2);
                writer.WriteText(name, true, Encoding.UTF8);

                byte[] data = stream.ToArray();
                SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, (ushort)data.Length);
                Send(packet, data);
            }
        }

        public void SendFriendStatus(string name, bool inGame = false, PlayerStatus status = PlayerStatus.Offline)
        {
            using (MemoryStream stream = new MemoryStream())
            using (SfBinaryWriter writer = new SfBinaryWriter(stream))
            {
                writer.WriteByte(3);
                writer.WriteText(name, true, Encoding.UTF8);
                writer.WriteByte(inGame ? (byte)1 : (byte)0);
                writer.WriteUInt16((ushort)(int)status);

                byte[] data = stream.ToArray();
                SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, (ushort)data.Length);
                Send(packet, data);
            }
        }

        public void SendFriendRequestResult(string name, byte result)
        {
            using (MemoryStream stream = new MemoryStream())
            using (SfBinaryWriter writer = new SfBinaryWriter(stream))
            {
                writer.WriteByte(4);
                writer.WriteByte(result);
                writer.WriteText(name, true, Encoding.UTF8);

                byte[] data = stream.ToArray();
                SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, (ushort)data.Length);
                Send(packet, data);
            }
        }
    }
}
