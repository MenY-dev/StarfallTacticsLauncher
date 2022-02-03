using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Debugging
{
    public static class ObjectLogExtension
    {
        public static void Log(this object obj, object msg) => Log(obj, msg?.ToString());

        public static void Log(this object obj, string msg) => Log(obj, msg, obj?.GetType().Name);

        public static void Log(this object obj, string msg, string tag)
        {
            string log = $"[{DateTime.Now:T}][{tag}] {msg ?? "NULL"}";
            Console.WriteLine(log);
            Debug.WriteLine(log);
        }
    }
}
