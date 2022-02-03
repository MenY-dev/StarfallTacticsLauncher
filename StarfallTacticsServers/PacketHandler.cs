using StarfallTactics.StarfallTacticsServers.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public static class PacketHandler
    {
        public static int ReadHeader(Stream stream, out SFCP.Header data) => Read(stream, out data);

        public static int ReadInstanceAuth(Stream stream, out SFCP.InstanceAuth data) => Read(stream, out data);

        public static int Read<T>(Stream stream, out T data)
        {
            if (stream is null)
                throw new ArgumentNullException("stream");

            int size = Marshal.SizeOf(default(T));
            byte[] bytes = new byte[size];
            int readResult = stream.Read(bytes, 0, size);
            IntPtr buffer = Marshal.AllocHGlobal(size);

            Marshal.Copy(bytes, 0, buffer, size);
            data = (T)Marshal.PtrToStructure(buffer, typeof(T));
            Marshal.FreeHGlobal(buffer);

            return readResult;
        }

        public static void Write(Stream stream, SFCP.Request request)
        {
            Write(stream, request.Header);
            Write(stream, request.Body);
        }

        public static void Write(Stream stream, SFCP.Response response)
        {
            Write(stream, response.Header);
            stream.WriteByte(response.ErrorCode);
            Write(stream, response.Body);
        }

        public static void Write(Stream stream, SFCP.Header header, byte[] data)
        {
            header.Size = (ushort)(Marshal.SizeOf(header) + data.Length);
            Write(stream, header);
            stream.Write(data, 0, data.Length);
        }

        public static void Write(Stream stream, SFCP.TextPacket packet, string text)
        {
            int maxLength = 16000;

            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);

            byte[] data = Encoding.Unicode.GetBytes(text);
            packet.Header.Size = (ushort)(Marshal.SizeOf(packet) + data.Length);
            Write(stream, packet);
            stream.Write(data, 0, data.Length);
        }

        public static void Write(Stream stream, SFCP.BinaryPacket packet, byte[] data)
        {
            packet.Header.Size = (ushort)(Marshal.SizeOf(packet) + data.Length);
            Write(stream, packet);
            stream.Write(data, 0, data.Length);
        }

        public static void Write<T>(Stream stream, T data)
        {
            if (stream is null)
                throw new ArgumentNullException("stream");

            int size = Marshal.SizeOf(data);
            byte[] bytes = new byte[size];
            IntPtr buffer = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(data, buffer, false);
            Marshal.Copy(buffer, bytes, 0, size);
            Marshal.FreeHGlobal(buffer);

            stream.Write(bytes, 0, size);
        }
    }
}
