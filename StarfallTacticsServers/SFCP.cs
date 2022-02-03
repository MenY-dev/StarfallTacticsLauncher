using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public struct SFCP
    {
        [StructLayout(LayoutKind.Explicit, Size = 4, Pack = 1)]
        public struct Header
        {
            [FieldOffset(0)] public byte Id;
            [FieldOffset(1)] public ushort Size;
            [FieldOffset(3)] public byte Cmd;

            public static Header Default => new Header() { Id = 85, Size = 4, Cmd = 0 };
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Request
        {
            public Header Header;
            public object Body;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Response
        {
            public Header Header;
            public byte ErrorCode;
            public object Body;
        }

        [StructLayout(LayoutKind.Explicit, Size = 68, Pack = 1)]
        public struct Register
        {
            [FieldOffset(0)]
            public int ChannelId;

            [FieldOffset(4), MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string ChannelName;
        }

        [StructLayout(LayoutKind.Explicit, Size = 4, Pack = 1)]
        public struct Unregister
        {
            [FieldOffset(0)]
            public int ChannelId;
        }

        [StructLayout(LayoutKind.Explicit, Size = 128, Pack = 1)]
        public struct UserAuth
        {
            [FieldOffset(0), MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string UserName;

            [FieldOffset(64), MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string TemporaryPass;
        }

        [StructLayout(LayoutKind.Explicit, Size = 69, Pack = 1)]
        public struct InstanceAuth
        {
            [FieldOffset(0)]
            public int InstanceId;

            [FieldOffset(4)]
            public byte Len;

            [FieldOffset(5), MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte Password;
        }

        [StructLayout(LayoutKind.Explicit, Size = 9, Pack = 1)]
        public struct TextPacket
        {
            [FieldOffset(0)]
            public Header Header;

            [FieldOffset(4)]
            public int Channel;

            [FieldOffset(8)]
            public byte Charset;

            public static TextPacket Default => new TextPacket()
            {
                Header = new Header() { Id = 85, Size = 9, Cmd = 16 },
                Channel = 0,
                Charset = 2
            };

            public TextPacket(int channel) : this(channel, 0) { }

            public TextPacket(int channel, ushort dataSize)
            {
                Header = new Header() { Id = 85, Size = (ushort)(dataSize + 9), Cmd = 16 };
                Channel = channel;
                Charset = 2;
            }
        }

        [StructLayout(LayoutKind.Explicit, Size = 9, Pack = 1)]
        public struct BinaryPacket
        {
            [FieldOffset(0)]
            public Header Header;

            [FieldOffset(4)]
            public int Channel;

            [FieldOffset(8)]
            public byte Gap8;

            public static BinaryPacket Default => new BinaryPacket()
            {
                Header = new Header() { Id = 85, Size = 9, Cmd = 32 },
                Channel = 0,
                Gap8 = 0
            };

            public BinaryPacket(int channel) : this(channel, 0) { }

            public BinaryPacket(int channel, ushort dataSize)
            {
                Header = new Header() { Id = 85, Size = (ushort)(dataSize + 9), Cmd = 32 };
                Channel = channel;
                Gap8 = 0;
            }
        }
    }
}
