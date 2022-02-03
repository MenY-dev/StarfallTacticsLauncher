using StarfallTactics.StarfallTacticsServers.Multiplayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class ChannelManager : BaseChannelManager
    {
        public event EventHandler<ChannelRegisterEventArgs> ChannelRegister;

        public List<Channel> Channels { get; }  = new List<Channel>();

        public PlayerServer PlayerServer { get; set; } = null;

        public StarfallProfile Profile => PlayerServer?.Profile;

        public MatchmakerClient Matchmaker => PlayerServer?.Matchmaker;

        public TcpClient Client { get; set; } = null;

        protected object SendLocker { get; } = new object();

        protected override void HandleClient(TcpClient client)
        {
            Client = client;
            base.HandleClient(client);
        }

        protected override void OnDataReceived(TcpClient client, SFCP.Header header)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[header.Size];
            stream.Read(buffer, 0, header.Size);

            switch (header.Cmd)
            {
                case 1:
                    HandleUserAuth(client, header, buffer);
                    break;
                case 2:
                    string channel = Encoding.UTF8.GetString(buffer, 0, Math.Min(buffer.Length, 64)).Trim('\0');
                    HandleChannelRegister(client, header, channel);
                    break;
                case 4:
                    HandleInstanceAuth(client, header, buffer);
                    break;
                case 32:
                    SFCP.BinaryPacket binaryPacket = SFCP.BinaryPacket.Default;
                    binaryPacket.Header = header;
                    binaryPacket.Header.Size = (ushort)Math.Max(0, binaryPacket.Header.Size - 5);
                    binaryPacket.Channel = BitConverter.ToInt32(buffer, 0);
                    binaryPacket.Gap8 = buffer[4];
                    byte[] binaryPacketData = new byte[binaryPacket.Header.Size];
                    Array.Copy(buffer, 5, binaryPacketData, 0, binaryPacketData.Length);
                    Log($"Bytes({BitConverter.ToString(buffer).Replace("-", "")})");
                    GetChannelById(binaryPacket.Channel)?.Input(binaryPacketData);
                    HandleBinary(client, binaryPacket, binaryPacketData);
                    break;
                default:
                    string textRequest = Encoding.UTF8.GetString(buffer).Replace("\0", "");
                    string binaryRequest = BitConverter.ToString(buffer).Replace("-", "");
                    Log($"Unhandled Request: (Text = \"{textRequest}\", Binary = \"{binaryRequest}\")");
                    break;
            }
        }

        protected virtual void HandleUserAuth(TcpClient client, SFCP.Header header, byte[] data)
        {
            UseClientStream((stream) =>
            {
                string name = Encoding.UTF8.GetString(data, 0, 64).Trim('\0');
                string session = Encoding.UTF8.GetString(data, 63, 64).Trim('\0');
                SFCP.Header outHeader = SFCP.Header.Default;

                outHeader.Cmd = 1;
                outHeader.Size = 133;

                Log($"UserAuth: (Name = {name}, Session = {session})");

                PacketHandler.Write(stream, outHeader);
                stream.WriteByte(0);
                stream.Write(data, 0, 64);
                stream.Write(data, 63, 64);
                stream.Flush();

                Log($"Response: (Name = {name}, Session = {session})");
            });
        }

        protected virtual void HandleInstanceAuth(TcpClient client, SFCP.Header header, byte[] data)
        {
            UseClientStream((stream) =>
            {
                int instanceId = BitConverter.ToInt32(data, 0);
                byte passwordLength = data[4];
                string password = Encoding.ASCII.GetString(data, 5, passwordLength);
                SFCP.Header outHeader = SFCP.Header.Default;

                outHeader.Cmd = 4;
                outHeader.Size = 74;

                Log($"InstanceAuth: (InstanceId = {instanceId}, Auth = {password})");

                PacketHandler.Write(stream, outHeader);
                stream.WriteByte(0);
                stream.Write(data, 0, 69);
                stream.Flush();

                Log($"Response: (InstanceId = {instanceId}, Auth = {password})");
            });
        }

        public virtual void HandleChannelRegister(TcpClient client, SFCP.Header header, string channelName)
        {
            UseClientStream((stream) =>
            {
                Channel channel = GetChannelByName(channelName);
                int id = channel?.Id ?? -1;

                Log($"Register: (Channel = {channelName})");

                if (id < 0)
                {
                    Log($"Register Falled: (Channel = {channelName})");
                    return;
                }


                channel.Register();
                //SFCP.Header outHeader = SFCP.Header.Default;
                //byte[] outData = new byte[69];

                //outHeader.Cmd = 2;
                //outHeader.Size = 73;
                //outData[0] = 0;

                //BitConverter.GetBytes(id).CopyTo(outData, 1);
                //Encoding.UTF8.GetBytes(channelName).CopyTo(outData, 5);

                //PacketHandler.Write(stream, outHeader, outData);
                //OnChannelRegister(new ChannelRegisterEventArgs(channelName, id));
                //Log($"Response: (Channel = {channelName}, Id = {id})");
            });
        }

        public Channel GetChannelById(int id)
        {
            foreach (var item in Channels)
                if (item.Id == id)
                    return item;

            return null;
        }

        public Channel GetChannelByName(string name)
        {
            foreach (var item in Channels)
                if (item.Name == name)
                    return item;

            return null;
        }

        protected virtual string GetChannelName(int id) => GetChannelById(id)?.Name ?? null;

        protected virtual int GetChannelId(string name) => GetChannelByName(name)?.Id ?? -1;

        public virtual void HandleBinary(TcpClient client, SFCP.BinaryPacket packet, byte[] data)
        {

        }

        public virtual void Send(SFCP.Header header)
        {
            UseClientStream((stream) => PacketHandler.Write(stream, header));
        }

        public virtual void Send(SFCP.Header header, byte[] data)
        {
            UseClientStream((stream) => PacketHandler.Write(stream, header, data));
        }

        public virtual void Send(SFCP.BinaryPacket packet, byte[] data)
        {
            UseClientStream((stream) => PacketHandler.Write(stream, packet, data));
        }

        public virtual void Send(SFCP.Request request)
        {
            UseClientStream((stream) => PacketHandler.Write(stream, request));
        }

        public virtual void Send(SFCP.Response response)
        {
            UseClientStream((stream) => PacketHandler.Write(stream, response));
        }

        public void UseClientStream(Action<Stream> action)
        {
            lock (SendLocker)
            {
                try
                {
                    Stream stream = Client?.GetStream();

                    if (stream is null || stream.CanWrite == false)
                        return;

                    action?.Invoke(stream);
                }
                catch { }
            }
        }

        protected virtual void OnChannelRegister(ChannelRegisterEventArgs eventArgs)
        {
            ChannelRegister?.Invoke(this, eventArgs);
        }
    }
}
