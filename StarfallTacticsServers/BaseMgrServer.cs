using StarfallTactics.StarfallTacticsServers.Json;
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
using System.Web;

namespace StarfallTactics.StarfallTacticsServers
{
    public class BaseMgrServer : HttpServer
    {
        protected override void HandleRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = request.HttpMethod;
            ClientQuery query;

            switch (method)
            {
                case "GET":
                    query = ClientQuery.Parse(request.QueryString);
                    break;

                case "POST":
                    string postQueryText = null;

                    if (request.InputStream != null)
                        using (StreamReader reader = new StreamReader(request.InputStream))
                            postQueryText = reader.ReadToEnd();

                    query = ClientQuery.Parse(postQueryText);

                    Log($"POST Request: {postQueryText}");
                    break;

                default:
                    query = new ClientQuery();
                    break;
            }

            if ((query.Function is null) == false)
            {
                HandleQuery(context, query);
            }
        }

        public virtual void SendResponse(HttpListenerContext context, JsonNode doc)
        {
            Send(context, new JsonObject
            {
                ["doc"] = doc?.Clone() ?? new JsonObject()
            }.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }

        public virtual void SendResponse(HttpListenerContext context, object doc)
        {
            SendResponse(context, JsonSerializer.SerializeToNode(doc ?? new object()));
        }

        protected virtual void HandleQuery(HttpListenerContext context, ClientQuery query)
        {

        }
    }
}
