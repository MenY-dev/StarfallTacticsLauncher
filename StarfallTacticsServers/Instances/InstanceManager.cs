using StarfallTactics.StarfallTacticsServers.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Instances
{
    public class InstanceManager
    {
        public virtual string GameLocation
        {
            get => gameLocation;
            set
            {
                gameLocation = value;
                logLocation = Path.Combine(gameLocation, "Msk", "starfall_game", "Starfall", "Saved", "Logs");
                executablePath = Path.Combine(gameLocation, "Msk", "starfall_game", "Starfall", "Binaries", "Win64", "Starfall.exe");
                instanceConfigPath = Path.Combine(logLocation, "instance.json");
                loadingResultPath = Path.Combine(logLocation, "instance_loaded");
            }
        }

        public string LogLocation { get => logLocation; }
        public string ExecutablePath { get => executablePath; }
        public string InstanceConfigPath { get => instanceConfigPath; }
        public string LoadingResultPath { get => loadingResultPath; }
        public string GameUserSettingsPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InstanceSettings", "GameUserSettings.ini");

        public string SfMgr { get; set; }
        public string RealmMgr { get; set; }
        public string GalaxyMgrAddress { get; set; }
        public int GalaxyMgrPort { get; set; }

        protected List<IInstance> RunningInstances { get; set; } = new List<IInstance>();
        protected Stack<IInstance> InstancesQueue { get; set; } = new Stack<IInstance>();
        protected object instanceStartlocker { get; } = new object();
        protected object instanceCreationlocker { get; } = new object();

        private string gameLocation = string.Empty;
        private string logLocation = string.Empty;
        private string executablePath = string.Empty;
        private string instanceConfigPath = string.Empty;
        private string loadingResultPath = string.Empty;

        public void StartInstance(IInstance instance)
        {
            lock (instanceStartlocker)
            {
                if (InstancesQueue.Contains(instance) == false)
                {
                    InstancesQueue.Push(instance);

                    if (InstancesQueue.Count < 2)
                    {
                        HandleNextInstance();
                    }
                }
            }
        }

        public void StopInstance(string auth)
        {
            lock (instanceStartlocker)
            {
                foreach (var item in RunningInstances)
                    if (item.Auth == auth)
                        StopInstance(item);
            }
        }

        public void StopInstance(IInstance instance)
        {
            lock (instanceStartlocker)
            {
                if (instance?.Process is Process process)
                {
                    process.CloseMainWindow();
                    process.Close();
                }
            }
        }

        protected void HandleNextInstance()
        {
            if (InstancesQueue.Count < 1)
                return;

            Task.Factory.StartNew(() =>
            {
                while (RunningInstances.Count > 99)
                    Thread.Sleep(1000);

                HandleInstance(InstancesQueue.Pop());
            }, TaskCreationOptions.LongRunning);
        }

        protected void HandleInstance(IInstance instance)
        {
            if (instance is null)
                return;

            lock (instanceCreationlocker)
            {
                try
                {
                    if (Directory.Exists(GameLocation) == false || File.Exists(ExecutablePath) == false)
                    {
                        if ((instance is null) == false)
                            instance.State = InstanceState.Error;

                        return;
                    }

                    ClearInstanceLoadingResult();

                    Process process = new Process
                    {
                        EnableRaisingEvents = true
                    };

                    process.Exited += (o, e) =>
                    {
                        lock (instanceStartlocker)
                        {
                            instance.IsStarted = false;

                            if (RunningInstances.Contains(instance))
                                RunningInstances.Remove(instance);

                            if (instance?.IsCanceled == false)
                                instance.State = InstanceState.Exit;
                            else
                                instance.State = InstanceState.Cancel; ;
                        }
                    };

                    instance.Process = process;
                    instance.Manager = this;
                    instance.SfMgr = SfMgr;
                    instance.RealmMgr = RealmMgr;
                    instance.GalaxyMgrAddress = GalaxyMgrAddress;
                    instance.GalaxyMgrPort = GalaxyMgrPort;
                    instance.InstanceID = CreateId();
                    instance.InstancePort = 7777 + instance.InstanceID;
                    instance.Auth = CreateUniqueAuth();

                    WriteInstanceSettings();
                    WriteInstanceConfig(instance);

                    process.StartInfo = CreateInstanceProcesstInfo(instance);
                    process.Start();
                    RunningInstances.Add(instance);
                    instance.IsStarted = true;

                    DateTime startTime = DateTime.Now;

                    while (File.Exists(LoadingResultPath) == false || File.ReadAllText(InstanceConfigPath) == "0")
                    {
                        if (DateTime.Now.Subtract(startTime).Minutes > 1 || instance.IsStarted == false)
                        {
                            instance.IsCanceled = true;

                            try
                            {
                                process.CloseMainWindow();
                                process.Close();
                            }
                            catch { }

                            break;
                        }

                        Thread.Sleep(1000);
                    }
                }
                catch
                {
                    if ((instance is null) == false)
                        instance.State = InstanceState.Error;
                }
            }

            if (instance?.IsCanceled == false && instance.IsStarted == true)
                instance.State = InstanceState.ReadyToConnect;

            HandleNextInstance();
        }

        protected void ClearInstanceLoadingResult()
        {
            if (File.Exists(InstanceConfigPath))
                File.Delete(InstanceConfigPath);

            if (File.Exists(LoadingResultPath))
                File.Delete(LoadingResultPath);
        }

        protected void WriteInstanceConfig(IInstance instance)
        {
            FileInfo file = new FileInfo(instanceConfigPath);
            file.Directory.Create();
            File.WriteAllText(file.FullName, instance.ToInstanceDocument());
        }

        protected void WriteInstanceSettings()
        {
            FileInfo file = new FileInfo(GameUserSettingsPath);
            file.Directory.Create();
            File.WriteAllText(file.FullName, InstanceSettings.GameUserSettings);
        }

        protected virtual ProcessStartInfo CreateInstanceProcesstInfo(IInstance instance)
        {
            string arguments = ""

            + $"/Game/Maps/{instance.InstanceMap}?Listen?FSpawnTime=90?SNSpawnTime=90"
            + $" -port={instance.InstancePort}"
            + " -messaging"
            + " -windowed"
            + " -NOSOUND"
            + " -NOSPLASH"
            + " -SPECTATORONLY"
            + " -NOINI"
            + " -NoLoadingScreen"
            + $" -GAMEUSERSETTINGSINI=\"{GameUserSettingsPath}\"";
            //+ " -ExecCmds=\"ShowUI false\"";

            ProcessStartInfo processInfo = new ProcessStartInfo(ExecutablePath, arguments);

            //processInfo.UseShellExecute = false;
            //processInfo.CreateNoWindow = true;
            processInfo.WindowStyle = ProcessWindowStyle.Minimized;

            return processInfo;
        }

        protected virtual int CreateId()
        {
            int id = 0;

            for (int i = 0; i < RunningInstances.Count; i++)
            {
                bool isEmpty = true;

                foreach (var item in RunningInstances)
                {
                    if (item.InstanceID == id)
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty == true)
                    return id;

                id++;
            }

            return id;
        }

        protected virtual string CreateUniqueAuth()
        {
            string auth;
            bool isUnique;

            do
            {
                auth = Guid.NewGuid().ToString("N");
                isUnique = true;

                foreach (var instance in RunningInstances)
                {
                    if (auth == instance.Auth)
                    {
                        isUnique = false;
                        break;
                    }
                }
            }
            while (isUnique == false);

            return auth;
        }
    }
}
