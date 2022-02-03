using StarfallTactics.StarfallTacticsServers.Debugging;
using StarfallTactics.StarfallTacticsServers.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class Channel
    {
        public ChannelManager ChannelManager { get; }

        public PlayerServer PlayerServer => ChannelManager.PlayerServer;

        public StarfallProfile Profile => PlayerServer?.Profile;

        public MatchmakerClient Matchmaker => PlayerServer?.Matchmaker;

        public string Name { get; } = string.Empty;

        public int Id { get; } = -1;

        public Channel(ChannelManager channelManager, string name, int id)
        {
            ChannelManager = channelManager;
            Name = name;
            Id = id;
        }

        public virtual void Input(byte[] data)
        {

        }

        public virtual void Input(string text)
        {

        }

        public virtual void Register()
        {
            SFCP.Response response = new SFCP.Response();

            response.Header = SFCP.Header.Default;
            response.Header.Cmd = 2;
            response.Header.Size = 73;
            response.ErrorCode = 0;
            response.Body = new SFCP.Register
            {
                ChannelId = Id,
                ChannelName = Name
            };

            Send(response);
            this.Log($"Register: (Name = {Name}, Id = {Id})");
        }

        public virtual void Send(SFCP.Header header)
        {
            ChannelManager?.Send(header);
        }

        public virtual void Send(SFCP.Header header, byte[] data)
        {
            ChannelManager?.Send(header);
        }

        public virtual void Send(SFCP.BinaryPacket packet, byte[] data)
        {
            ChannelManager?.Send(packet, data);
        }

        public virtual void Send(SFCP.TextPacket packet, string text)
        {
            ChannelManager?.Send(packet, text);
        }

        public virtual void Send(SFCP.Request request)
        {
            ChannelManager?.Send(request);
        }

        public virtual void Send(SFCP.Response response)
        {
            ChannelManager?.Send(response);
        }
    }
}
