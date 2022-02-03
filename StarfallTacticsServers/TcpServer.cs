using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class TcpServer : BaseServer
    {
        public event EventHandler<TcpRequestEventArgs> ClientConnected;

        public TcpListener Listener { get; protected set; } = null;

        public string Address
        {
            get => address;
            set
            {
                if (value != address)
                {
                    if (IsStarded)
                        Stop();

                    address = value;
                }
            }
        }

        private string address = null;

        public override void Start()
        {
            if (IsStarded)
                Stop();

            string[] addressData = address.Split(':');

            Listener = new TcpListener(new IPEndPoint(
                IPAddress.Parse(addressData[0]),
                int.Parse(addressData[1])));

            IsStarded = true;
            Listener.Start();

            Task.Run(() => AcceptClientsAsync());
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

            Listener?.Stop();
            IsStarded = false;
        }

        protected virtual async Task AcceptClientsAsync()
        {
            while (IsStarded && Listener != null)
            {
                try
                {
                    Log("Listening...");
                    TcpClient client = await Listener.AcceptTcpClientAsync();

                    Task task = Task.Run(() =>
                    {
                        Log($"Connected! ({client.Client.RemoteEndPoint})");
                        OnClientConnected(new TcpRequestEventArgs(client));
                        HandleClient(client);
                    });
                }
                catch (Exception e)
                {
                    Log($"Error!\r\n{e}");
                }
            }
        }

        protected virtual void HandleClient(TcpClient client)
        {

        }

        protected virtual void OnClientConnected(TcpRequestEventArgs args)
        {
            ClientConnected?.Invoke(this, args);
        }
    }
}
