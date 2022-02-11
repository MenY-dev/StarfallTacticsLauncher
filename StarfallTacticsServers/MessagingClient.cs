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

        public event EventHandler<EventArgs> Connected;

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
                Disconnect();
            }
            catch { }
        }

        protected virtual async Task ConnectToServerAsync(string address, int port, CancellationToken cancellationToken)
        {
            while (IsStarded && cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    Log("Listening...");

                    Client = new TcpClient();
                    await Client.ConnectAsync(address, port);

                    cancellationToken.ThrowIfCancellationRequested();

                    Log($"Connected! ({Client.Client.RemoteEndPoint})");
                    HandleConnection(Client, cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch (Exception e)
                {
                    if (cancellationToken.IsCancellationRequested == false)
                        Log($"Error!\r\n{e}");
                }

                if (cancellationToken.IsCancellationRequested)
                    break;

                Disconnect();
                Thread.Sleep(1000);
                Log($"Reconnect...");
            }
        }

        protected virtual void HandleConnection(TcpClient client, CancellationToken cancellationToken)
        {
            OnConnected(EventArgs.Empty);

            while (client?.Connected == true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string packet = MessagingPacket.Receive(client);

                if (string.IsNullOrWhiteSpace(packet) == false)
                    HandleInputPacket(client, packet);
            }
        }

        protected virtual void HandleInputPacket(TcpClient client, string packet)
        {

        }

        protected void Disconnect()
        {
            if (Client?.Client != null && Client.Connected == true)
            {
                Client.Close();
            }
        }

        public virtual void Send(string packet)
        {
            if (Client?.Client != null && Client?.Connected == true)
            {
                try
                {
                    lock (Locker)
                        MessagingPacket.Send(Client, packet);
                }
                catch { }
            }
        }

        protected virtual void OnConnected(EventArgs args)
        {
            Connected?.Invoke(this, args);
        }
    }
}
