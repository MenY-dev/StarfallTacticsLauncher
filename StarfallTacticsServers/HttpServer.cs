using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class HttpServer : BaseServer
    {
        public HttpListener Listener { get; protected set; } = new HttpListener();

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

            IsStarded = true;
            Listener.Prefixes.Clear();
            Listener.Prefixes.Add(address);
            Listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            Listener.Start();

            Task.Factory.StartNew(() => ListeningLoop());
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

        protected virtual void ListeningLoop()
        {
            while (IsStarded && Listener != null)
            {
                try
                {
                    Log("Listening...");
                    var context = Listener.GetContext();

                    Task.Factory.StartNew(() =>
                    {
                        Log($"Connected: {context.Request?.RemoteEndPoint}");

                        var request = context.Request;
                        var response = context.Response;
                        List<string> query = new List<string>();

                        if ((request is null) == false && (response is null) == false)
                        {
                            string method = request.HttpMethod;

                            Log($"Request: (Method = {method}, Url = {request.Url}");
                            HandleRequest(context);
                        }
                        else
                        {
                            Log("Emty request!");
                        }

                    });
                }
                catch (Exception e)
                {
                    Log($"Error!\r\n{e}");
                }
            }
        }

        public virtual void Send(HttpListenerContext context, JsonNode packet)
        {
            if ((packet is null) == false)
            {
                Send(context, packet.ToJsonString(
                    new JsonSerializerOptions { WriteIndented = true }));
            }

        }

        public virtual void Send(HttpListenerContext context, string packet)
        {
            if (string.IsNullOrWhiteSpace(packet) == false)
            {
                Log($"Response: \r\n{packet}");
                Send(context, Encoding.UTF8.GetBytes(packet));
            }

        }

        public virtual void Send(HttpListenerContext context, byte[] packet)
        {
            if ((packet is null) == false &&
                context.Response.OutputStream is Stream stream)
            {
                stream.Write(packet, 0, packet.Length);
                stream.Close();
            }
        }

        protected virtual void HandleRequest(HttpListenerContext context)
        {

        }
    }
}
