using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class RealmMgrChannelManager : ChannelManager
    {
        public RealmMgrChannelManager() : base()
        {
            Channels.Add(new TextChatChannel(this, "Deprived Chat", 6));
            Channels.Add(new TextChatChannel(this, "Eclipse Chat", 7));
            Channels.Add(new TextChatChannel(this, "Vanguard Chat", 8));
            Channels.Add(new Channel(this, "CharacterFriends", 9));
            Channels.Add(new Channel(this, "QuickMatch", 10));
            Channels.Add(new Channel(this, "CharactParty", 11));
            Channels.Add(new BattleGroundChannel(this, "BattleGround", 12));
            Channels.Add(new Channel(this, "Discovery", 13));
            Channels.Add(new Channel(this, "Galactic", 14));
        }

        private protected void HandleQuickMatchChannel(TcpClient client, SFCP.BinaryPacket packet, byte[] data)
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
                        int modeNameLength = inputReader.ReadUInt16();
                        byte[] modeNameBytes = inputReader.ReadBytes(modeNameLength);
                        string modeName = Encoding.ASCII.GetString(modeNameBytes);
                        int difficulty = inputReader.ReadInt32();

                        if (modeName == "srv1")
                        {
                            Stream stream = client.GetStream();
                            SFCP.BinaryPacket outPacket;

                            outPacket = new SFCP.BinaryPacket(packet.Channel, 22);
                            PacketHandler.Write(stream, outPacket);

                            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, true))
                            {
                                writer.Write((byte)3);

                                // //////////////////////////////////////

                                writer.Write((ushort)1);

                                writer.Write((int)0);

                                writer.Write((ushort)4);
                                stream.Write(new byte[4] { 57, 56, 55, 54 }, 0, 4);

                                writer.Write((byte)1);
                                writer.Write((byte)1);

                                // /////////////////////////////////////

                                writer.Write((ushort)4);
                                stream.Write(new byte[4] { 115, 114, 118, 49 }, 0, 4);

                                // /////////////////////////////////////

                                writer.Write((byte)2);
                            }


                            outPacket = new SFCP.BinaryPacket(packet.Channel, 51);
                            PacketHandler.Write(stream, outPacket);

                            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, true))
                            {
                                writer.Write((byte)0);

                                // //////////////////////////////////////

                                writer.Write((ushort)20);
                                writer.Write(Encoding.ASCII.GetBytes("127.0.0.1\0          "));

                                // //////////////////////////////////////

                                writer.Write((ushort)7777);

                                // //////////////////////////////////////

                                writer.Write((ushort)24);
                                writer.Write(Encoding.ASCII.GetBytes("a0b1c2d3e4f5g6h7i8j9k10l"));
                            }

                            Log("Start match: (Mode = \"Survival\")");
                        }
                    }
                }
                catch (Exception e)
                {
                    Log(e);
                }
            }
        }
    }
}
