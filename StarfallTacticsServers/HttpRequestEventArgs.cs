using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class HttpRequestEventArgs : EventArgs
    {
        public HttpListenerContext Context { get; }

        public HttpRequestEventArgs(HttpListenerContext context)
        {
            Context = context;
        }
    }
}
