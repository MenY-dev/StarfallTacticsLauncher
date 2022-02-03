using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class BaseChannelManager : TcpServer
    {
        protected override void HandleClient(TcpClient client)
        {
            SFCP.Header header;
            NetworkStream stream = client.GetStream();
            int bytesRead;

            while ((bytesRead = PacketHandler.ReadHeader(stream, out header)) > 0)
            {
                if (bytesRead > 3)
                {
                    Log($"Request: (Id = {header.Id}, Size = {header.Size}, Cmd = {header.Cmd})");

                    if (header.Id == 85 && header.Size > 3)
                    {
                        header.Size -= 4;
                        OnDataReceived(client, header);
                    }
                }
            }

            Log($"Disconnected: {(client?.Client.RemoteEndPoint as IPEndPoint)?.Address}");
            stream.Close();
        }

        protected virtual void OnDataReceived(TcpClient client, SFCP.Header header) { }
    }
}
