using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class ChannelRegisterEventArgs : EventArgs
    {
        public string Channel { get; }
        public int Id { get; }

        public ChannelRegisterEventArgs(string channel, int id)
        {
            Channel = channel;
            Id = id;
        }
    }
}
