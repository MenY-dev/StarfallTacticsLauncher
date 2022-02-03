using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class TcpRequestEventArgs : EventArgs
    {
        public TcpClient Client { get; }

        public TcpRequestEventArgs(TcpClient client)
        {
            Client = client;
        }
    }
}
