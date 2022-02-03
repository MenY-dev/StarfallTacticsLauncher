using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class MessagingClient : BaseServer
    {

        public string Address
        {
            get => address;
            set
            {
                if (IsStarded)
                    Stop();

                if (value != address)
                {
                    address = value;
                }
            }
        }

        protected TcpClient Client { get; set; }
        protected object Locker { get; } = new object();
        protected CancellationTokenSource CancellationToken { get; set; }

        private string address = null;

        public override void Start()
        {
            if (IsStarded)
                Stop();

            string[] addressData = address.Split(':');

            CancellationToken = new CancellationTokenSource();
            Client = new TcpClient();
            IsStarded = true;

            Task.Run(() => ConnectToServerAsync(
                addressData[0],
                int.Parse(addressData[1]),
                CancellationToken.Token));
        }

        public virtual void Start(string address)
        {
            if (IsStarded)
                Stop();

            Address = address;
            Start();
        }

        public override void Stop()
        {
            if (IsStarded == false)
                return;

            IsStarded = false;

            try
            {
                CancellationToken.Cancel();

                if (Client != null && Client.Client != null)
                {
                    Client.Close();
                }
            }
            catch { }
        }

        protected virtual async Task ConnectToServerAsync(string address, int port, CancellationToken cancellationToken)
        {
            while (IsStarded && (Client is null) == false && cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    Log("Listening...");
                    await Client.ConnectAsync(address, port);

                    if (cancellationToken.IsCancellationRequested)
                        return;

                    Log($"Connected! ({Client.Client.RemoteEndPoint})");
                    HandleConnection(Client);

                    if (cancellationToken.IsCancellationRequested)
                        return;

                    Client.Close();
                }
                catch (Exception e)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    Log($"Error!\r\n{e}");
                }

                Thread.Sleep(1000);
                Log($"Reconnect...");
            }
        }

        protected virtual void HandleConnection(TcpClient client)
        {
            while (client?.Connected == true)
            {
                string packet;

                packet = MessagingPacket.Receive(Client);

                if (string.IsNullOrWhiteSpace(packet) == false)
                    HandleInputPacket(Client, packet);
            }
        }

        protected virtual void HandleInputPacket(TcpClient client, string packet)
        {

        }

        public virtual void Send(string packet)
        {
            if (Client.Connected == true)
            {
                try
                {
                    lock (Locker)
                        MessagingPacket.Send(Client, packet);
                }
                catch { }
            }
        }
    }
}
