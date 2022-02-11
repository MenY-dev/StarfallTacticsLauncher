using StarfallTactics.StarfallTacticsServers.Multiplayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarfallTactics.StarfallTacticsLauncher
{
    public partial class ConnectionCheckForm : Form
    {
        private MatchmakerClient client;
        bool isConnected = false;

        public ConnectionCheckForm(string address)
        {
            InitializeComponent();

            client = new MatchmakerClient();
            client.Address = address;
            client.PacketReceived += MatchmakerPacketReceived;
            client.Connected += ClientConnected;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (client is null)
                return;

            try
            {
                client.Start();

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(10000);

                    if (IsDisposed == false && isConnected == false)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            Status.Text = $"Timeout exceeded. The server is not responding.";
                            CloseButton.Enabled = true;

                            if ((client is null) == false)
                            {
                                client.PacketReceived -= MatchmakerPacketReceived;
                                client.Connected -= ClientConnected;
                            }
                        }));
                    }
                });
            }
            catch
            {
                DialogResult = DialogResult.Abort;
            }
        }

        private void MatchmakerPacketReceived(object sender, MatchmakerPacketEventArgs e)
        {
            try
            {
                if (e?.Packet?.Document is null)
                    return;

                if (e.Packet.Type == PacketType.Info)
                {
                    JsonNode doc = e.Packet.Document;

                    if (Version.TryParse((string)doc["version"], out Version version))
                    {
                        if (version.Equals(MatchmakerClient.Version))
                        {
                            isConnected = true;
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            Status.Text = $"The server version ({MatchmakerClient.Version})" +
                                $"is different from the client version ({version}). Connection is not possible.";

                            CloseButton.Enabled = true;
                        }
                    }
                }
            }
            catch
            {
                DialogResult = DialogResult.Abort;
            }
        }

        private void ClientConnected(object sender, EventArgs e)
        {
            client?.Send(PacketType.Info, new JsonObject());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            try
            {
                client.PacketReceived -= MatchmakerPacketReceived;
                client.Connected -= ClientConnected;
                client?.Stop();
                client = null;
            }
            catch { }
        }
    }
}
