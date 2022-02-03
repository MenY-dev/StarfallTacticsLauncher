using StarfallTactics.StarfallTacticsServers.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public abstract class BaseServer
    {

        public bool IsStarded
        {
            get => isStarded;
            protected set
            {
                if (value != isStarded)
                {
                    isStarded = value;
                    StateChanged?.Invoke(this, EventArgs.Empty);
                    Log(isStarded ? $"Start!" : $"Stop!");
                }
            }
        }

        public event EventHandler<EventArgs> StateChanged;

        private bool isStarded = false;

        public abstract void Start();

        public abstract void Stop();

        protected void Log(object obj) => (this as object).Log(obj);

        protected void Log(string text) => (this as object).Log(text);
    }
}
