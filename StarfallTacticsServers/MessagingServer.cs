using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class MessagingServer : TcpServer
    {
        protected override void HandleClient(TcpClient client)
        {
            while ((client is null) == false && client.Connected == true)
            {
                string packet = MessagingPacket.Receive(client);

                try
                {
                    if (string.IsNullOrWhiteSpace(packet) == false)
                        HandleInputPacket(client, packet);
                }
                catch { }
            }
        }

        protected virtual void HandleInputPacket(TcpClient client, string packet)
        {

        }

        public static void Send(TcpClient client, string packet)
        {
            try
            {
                lock (client)
                    MessagingPacket.Send(client, packet);
            }
            catch { }
        }
    }
}
