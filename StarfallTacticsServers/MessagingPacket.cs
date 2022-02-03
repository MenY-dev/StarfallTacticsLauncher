using StarfallTactics.StarfallTacticsServers.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public static class MessagingPacket
    {
        public static string Receive(TcpClient client) => Receive(client?.GetStream());

        public static string Receive(Stream stream)
        {
            if (stream is null || stream.CanRead == false)
                return null;

            byte id;
            uint size;
            byte cmd;
            byte[] buffer;

            using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                id = reader.ReadByte();
                size = reader.ReadUInt32();
                cmd = reader.ReadByte();
            }

            if (id != 90 || size < 6)
                return null;

            int dataSize = (int)(size - 6);
            buffer = new byte[size - 6];
            ReadPacket(stream, buffer, dataSize);

            if (cmd != 16)
                return null;

            string packet = Encoding.UTF8.GetString(buffer);

            if (string.IsNullOrWhiteSpace(packet) == false)
                return packet;
            
            return null;
        }

        private static void ReadPacket(Stream stream, byte[] buffer, int size)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                int position = 0;

                while (position < size)
                {
                    byte[] chunk = new byte[size];
                    int bytesCount = stream.Read(chunk, 0, size - position);

                    if (bytesCount > 0)
                    {
                        position += bytesCount;
                        ms.Write(chunk, 0, bytesCount);
                    }
                }
            }
        }

        public static void Send(TcpClient client, string message) => Send(client?.GetStream(), message);

        public static void Send(Stream stream, string message)
        {
            if (stream is null || stream.CanWrite == false || message is null)
                return;

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            uint packetSize = (uint)(buffer.Length + 6);

            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write((byte)90);
                writer.Write(packetSize);
                writer.Write((byte)16);
                writer.Write(buffer);
            }

            stream.Flush();
        }
    }
}
