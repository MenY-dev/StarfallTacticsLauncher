using StarfallTactics.StarfallTacticsServers.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class MatchmakerChannel : Channel
    {
        public MatchmakerChannel(ChannelManager channelManager, string name, int id) : base(channelManager, name, id)
        {

        }

        public override void Register()
        {
            base.Register();

            SetClientState(1);
            RequestDataUpdate();
        }

        protected virtual void SetClientState(byte state)
        {
            SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, 6);
            byte[] data = new byte[6];

            using (SfBinaryWriter writer = new SfBinaryWriter(data))
            {
                writer.WriteByte(11);
                writer.WriteByte(state);
                writer.WriteInt32(0);
            }

            Send(packet, data);
            this.Log($"Set ClientState: (State = {state})");
        }

        public virtual void RequestDataUpdate()
        {
            SFCP.BinaryPacket packet = new SFCP.BinaryPacket(Id, 1);
            byte[] data = new byte[1] { 12 };
            Send(packet, data);
            this.Log($"Request data update!");
        }
    }
}
