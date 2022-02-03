////////////////////////////////////////////////////////////////////////
//// This launcher was created for testing purposes. In the future, ////
//// it will be replaced by a launcher based on Avalonia UI!        ////
////////////////////////////////////////////////////////////////////////

using StarfallTactics.StarfallTacticsServers;
using StarfallTactics.StarfallTacticsServers.Database;
using StarfallTactics.StarfallTacticsServers.Debugging;
using StarfallTactics.StarfallTacticsServers.Instances;
using StarfallTactics.StarfallTacticsServers.Multiplayer;
using StarfallTactics.StarfallTacticsServers.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StarfallTactics.StarfallTacticsLauncher
{
    public partial class MainForm : Form
    {
        protected LauncherSettings Settings { get; set; }

        protected StarfallDatabase Database { get; set; }

        protected StarfallProfile Profile { get; set; }

        protected PlayerServer PlayerServer { get; set; }

        protected string AppPath { get; } = string.Empty;

        protected string DataPath { get; } = string.Empty;

        protected MatchmakerServer DedicatedServer { get; set; }

        protected MatchmakerServer LocalServer { get; set; }

        protected InstanceManager InstanceManager{ get; set; }

        public MainForm()
        {
            AppPath = AppDomain.CurrentDomain.BaseDirectory;
            DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Starfall Online");

            InitializeComponent();

            Console.SetOut(new ConsoleReader(LogOutput));
            Console.WriteLine();

            LoadDatabase();
            LoadSettings();
            LoadProfile();

            Profile.Edited += OnProfileEdited;

            PlayerServer = new PlayerServer();
            PlayerServer.Profile = Profile;

            NicknameBox.LostFocus += OnNicknameEdited;
            CharacterNameBox.LostFocus += OnCharacterNameEdited;
            CharacterFactionBox.TextChanged += OnCharacterFactionChanged;
            CharacterFactionBox.MouseWheel += LockMouseWheel;
            ResetProfileButton.Click += OnResetProfileButtonClick;
            PlayButton.Click += OnPlayButtonClick;
            SelectGameLocationButton.Click += OnSelectGameLocation;
            GameLocationBox.LostFocus += OnEditGameLocation;
            EnableMultiplayerBox.Click += OnEnableMultiplayerBoxClicked;
            ServerAddressBox.LostFocus += OnEditServerAddress;
            ServerPortBox.LostFocus += OnEditServerPort;
            ShowGameLogCheckBox.Click += OnGameLogCheckBoxClicked;

            DedicatedServerAddressBox.DropDown += OnDedicatedServerAddressBoxOpen;
            DedicatedServerAddressBox.LostFocus += OnEditDedicatedServerAddressBox;
            DedicatedServerAddressBox.MouseWheel += LockMouseWheel;
            DedicatedServerPortBox.LostFocus += OnEditDedicatedServerPortBox;
            BattlegroundsRoomSizeBox.ValueChanged += OnEditBattlegroundsRoomSize;
            StartDedicatedServerButton.Click += OnStartDedicatedServerButtonClicked;

            InstanceManager = new InstanceManager();
            InstanceManager.GameLocation = Settings.GameLocation;
            InstanceManager.SfMgr = "http://127.0.0.1:1600/battlemgr/";
            InstanceManager.RealmMgr = "http://127.0.0.1:1600/battlemgr/";
            InstanceManager.GalaxyMgrAddress = "";
            InstanceManager.GalaxyMgrPort = 0;

            DedicatedServer = new MatchmakerServer();
            DedicatedServer.InstanceManager = InstanceManager;
            DedicatedServer.BGMothershipAssaultManager.RoomSize = Settings.ServerSettings.BattlegroundsMode.RoomSize;

            LocalServer = new MatchmakerServer();
            LocalServer.InstanceManager = InstanceManager;
            LocalServer.Address = "127.0.0.1:1300";
            LocalServer.BGMothershipAssaultManager.RoomSize = 1;
        }

        private void OnProfileEdited(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                this.BeginInvoke(new Action(() => 
                {
                    Profile?.Use(args => SaveProfile());
                }));
            });
        }

        private void OnDedicatedServerAddressBoxOpen(object sender, EventArgs e)
        {
            DedicatedServerAddressBox.Items.Clear();

            foreach (var item in NetworkUtils.GetUnicastAddressesIterator())
            {
                IPAddress address = item.Information.Address;

                if (address.AddressFamily != AddressFamily.InterNetwork)
                    continue;

                DedicatedServerAddressBox.Items.Add(address.ToString());
            }
        }

        private void OnEditDedicatedServerAddressBox(object sender, EventArgs e)
        {
            if ((Settings?.ServerSettings is null) == false)
            {
                Settings.ServerSettings.Address = DedicatedServerAddressBox.Text;
                UpdateValues();
                SaveSettings();
            }
        }

        private void OnEditDedicatedServerPortBox(object sender, EventArgs e)
        {
            if ((Settings?.ServerSettings is null) == false)
            {
                if (int.TryParse(DedicatedServerPortBox.Text ?? string.Empty, out int port))
                    Settings.ServerSettings.Port = port;

                UpdateValues();
                SaveSettings();
            }
        }

        private void OnStartDedicatedServerButtonClicked(object sender, EventArgs e)
        {
            if (DedicatedServer.IsStarded == true)
            {
                DedicatedServer.Stop();
                StartDedicatedServerButton.Text = "Start Server";
                NetworkGroup.Enabled = true;
            }
            else
            {
                try
                {
                    DedicatedServer.Address = $"{Settings.ServerSettings.Address}:{Settings.ServerSettings.Port}";
                    DedicatedServer.Start();
                    StartDedicatedServerButton.Text = "Stop Server";
                    NetworkGroup.Enabled = false;
                }
                catch (Exception ex)
                {
                    this.Log(ex);
                }
            }
        }

        private void OnNicknameEdited(object sender, EventArgs e)
        {
            if ((Profile is null) == false)
            {
                Profile.Nickname = NicknameBox.Text;
                UpdateValues();
                SaveProfile();
            }
        }

        private void OnCharacterNameEdited(object sender, EventArgs e)
        {
            if ((Profile is null) == false)
            {
                var chars = Profile.CharacterModeProfile?.Chars;

                if ((chars?.Count ?? 0) > 0)
                {
                    var character = chars[0];
                    character.Name = CharacterNameBox.Text;
                    UpdateValues();
                    SaveProfile();
                }
            }
        }

        private void OnCharacterFactionChanged(object sender, EventArgs e)
        {
            if ((Profile is null) == false)
            {
                var chars = Profile.CharacterModeProfile?.Chars;

                if ((chars?.Count ?? 0) > 0)
                {
                    var character = chars[0];
                    character.Faction = Math.Max(0, Math.Min(2, CharacterFactionBox.SelectedIndex));
                    UpdateValues();
                    SaveProfile();
                }
            }
        }

        private void OnResetProfileButtonClick(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Reset Profile?",
                "Reset...",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Profile = CreateNewProfile();

                if (Profile?.CharacterModeProfile?.Chars?.Count > 0)
                    Profile.SelectCharacter(Profile.CharacterModeProfile.Chars[0]);

                SaveProfile();
                UpdateValues();
            }
        }

        private void OnEnableMultiplayerBoxClicked(object sender, EventArgs e)
        {
            if ((Settings is null) == false)
            {
                Settings.EnableMultiplayer = EnableMultiplayerBox.Checked;
                UpdateValues();
                SaveSettings();
            }
        }

        private void OnEditServerAddress(object sender, EventArgs e)
        {
            if ((Settings is null) == false)
            {
                Settings.MultiplayerAddress = ServerAddressBox.Text;
                UpdateValues();
                SaveSettings();
            }
        }

        private void OnEditServerPort(object sender, EventArgs e)
        {
            if ((Settings?.ServerSettings is null) == false)
            {
                if (int.TryParse(ServerPortBox.Text ?? string.Empty, out int port))
                    Settings.MultiplayerPort = port;

                UpdateValues();
                SaveSettings();
            }
        }

        private void OnEditBattlegroundsRoomSize(object sender, EventArgs e)
        {
            if ((Settings?.ServerSettings?.BattlegroundsMode is null) == false)
            {
                int size = Math.Max(0, (int)BattlegroundsRoomSizeBox.Value);
                Settings.ServerSettings.BattlegroundsMode.RoomSize = size;
                DedicatedServer.BGMothershipAssaultManager.RoomSize = size;
                UpdateValues();
                SaveSettings();
            }
        }

        private void OnGameLogCheckBoxClicked(object sender, EventArgs e)
        {
            if ((Settings is null) == false)
            {
                Settings.ShowGameLog = ShowGameLogCheckBox.Checked;
                UpdateValues();
                SaveSettings();
            }
        }

        private void OnEditGameLocation(object sender, EventArgs e)
        {
            if ((Settings is null) == false)
            {
                Settings.GameLocation = GameLocationBox.Text;
                UpdateValues();
                SaveSettings();
            }
        }

        private void OnSelectGameLocation(object sender, EventArgs e)
        {
            if ((Settings is null) == false)
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.SelectedPath = Settings.GameLocation;
                    DialogResult result = dialog.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                    {
                        Settings.GameLocation = dialog.SelectedPath;
                        UpdateValues();
                        SaveSettings();
                    }
                }
            }
        }

        void LockMouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void OnPlayButtonClick(object sender, EventArgs e)
        {
            if (PlayerServer.IsStarded == true)
            {
                StoptGame();
            }
            else
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            if (PlayerServer.IsStarded == true)
                StoptGame();

            if (IsReadyToLaunch() == true)
            {
                string matchmakerAddress;

                if (Settings.EnableMultiplayer)
                {
                    matchmakerAddress = $"{Settings.MultiplayerAddress}:{Settings.MultiplayerPort}";
                }
                else
                {
                    matchmakerAddress = "127.0.0.1:1300";

                    try
                    {
                        LocalServer.Start();
                    }
                    catch { }
                }

                PlayButton.Enabled = false;
                ProfileGroup.Enabled = false;
                GameLocationGroup.Enabled = false;
                MultiplayerGroup.Enabled = false;
                DebugGroup.Enabled = false;
                SaveProfile();
                PlayerServer.MatchmakerAddress = matchmakerAddress;
                PlayerServer.Profile = Profile;
                PlayerServer.Start();
                RunClient();
            }
        }

        private void StoptGame()
        {
            if (PlayerServer.IsStarded == false)
                return;

            PlayerServer.Stop();
            SaveProfile();
            PlayButton.Enabled = true;
            ProfileGroup.Enabled = true;
            GameLocationGroup.Enabled = true;
            MultiplayerGroup.Enabled = true;
            DebugGroup.Enabled = true;

            try
            {
                LocalServer.Stop();
            }
            catch { }
        }

        protected void LoadDatabase()
        {
            try
            {
                string path = Path.Combine(AppPath, "Database", "StarfallDatabase.json");

                if (File.Exists(path) == false)
                {
                    Database = new StarfallDatabase();
                }
                else
                {
                    string doc = File.ReadAllText(path);
                    Database = JsonSerializer.Deserialize<StarfallDatabase>(doc) ?? new StarfallDatabase();
                }
            }
            catch { }

            UpdateValues();
        }

        protected StarfallProfile CreateNewProfile()
        {
            StarfallProfile profile = new StarfallProfile();
            Character character = new Character();
            profile.CharacterModeProfile.Chars.Add(character);

            foreach (var item in Database.Items)
            {
                character.AddInventoryItem(item.Id, 9999, 2);
            }

            foreach (var item in Database.Ships)
            {
                character.AddInventoryItem(item.Hull, 9999999, 0);
            }

            foreach (var item in Database.DiscoveryItem)
            {
                character.AddInventoryItem(item.Id, 99999, 3);
            }

            return profile;
        }

        protected void LoadProfile()
        {
            try
            {
                string path = Path.Combine(DataPath, "Profiles", "StarfallProfile.json");

                if (File.Exists(path) == false)
                {
                    Profile = CreateNewProfile();
                    SaveProfile();
                }
                else
                {
                    string doc = File.ReadAllText(path);
                    Profile = JsonSerializer.Deserialize<StarfallProfile>(doc) ?? CreateNewProfile();
                }

                if (Profile?.CharacterModeProfile?.Chars?.Count > 0)
                    Profile.SelectCharacter(Profile.CharacterModeProfile.Chars[0]);
            }
            catch { }

            UpdateValues();
        }

        protected void SaveProfile()
        {
            if (Profile is null)
                return;

            try
            {
                string doc = JsonSerializer.Serialize(
                    Profile, new JsonSerializerOptions { WriteIndented = true });

                FileInfo file = new FileInfo(Path.Combine(DataPath, "Profiles", "StarfallProfile.json"));
                file.Directory.Create();
                File.WriteAllText(file.FullName, doc);
            }
            catch { }
        }

        protected void LoadSettings()
        {
            try
            {
                string path = Path.Combine(DataPath, "Launcher", "Settings.json");

                if (File.Exists(path) == false)
                {
                    Settings = new LauncherSettings();
                    SaveSettings();
                }
                else
                {
                    string doc = File.ReadAllText(path);
                    Settings = JsonSerializer.Deserialize<LauncherSettings>(doc) ?? new LauncherSettings();
                }
            }
            catch { }

            UpdateValues();
        }

        protected void SaveSettings()
        {
            if (Settings is null)
                return;

            try
            {
                string doc = JsonSerializer.Serialize(
                    Settings, new JsonSerializerOptions { WriteIndented = true });

                FileInfo file = new FileInfo(Path.Combine(DataPath, "Launcher", "Settings.json"));
                file.Directory.Create();
                File.WriteAllText(file.FullName, doc);
            }
            catch { }
        }

        protected void RunClient()
        {
            if (Profile is null || Settings is null)
                return;

            string path = GetPathToExecutable(Settings.GameLocation ?? string.Empty);

            if (File.Exists(path))
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(path);
                    Process process = new Process();

                    startInfo.Arguments = $"MgrUrl=http://127.0.0.1:1500/sfmgr/";
                    startInfo.Arguments += $" LauncherUsername={Profile.Nickname}";
                    startInfo.Arguments += $" LauncherAuth={Profile.TemporaryPass}";
                    startInfo.Arguments += $" StartTime={DateTime.Now.ToOADate()}";

                    if (Settings.ShowGameLog == true)
                        startInfo.Arguments += $" -log";

                    process.StartInfo = startInfo;
                    process.EnableRaisingEvents = true;
                    process.Exited += (o, e) => BeginInvoke((Action)(() => StoptGame()));
                    process.Start();
                }
                catch { }
            }
        }

        protected bool IsReadyToLaunch()
        {
            string game = Settings?.GameLocation ?? string.Empty;
            string nickname = Profile?.Nickname;
            string temporaryPass = Profile?.TemporaryPass;

            if (File.Exists(GetPathToExecutable(game)) == false)
            {
                MessageBox.Show("Game not found!", "Error!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(nickname))
            {
                MessageBox.Show("Empty nickname!", "Error!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(temporaryPass))
            {
                MessageBox.Show("Empty temporary pass!", "Error!");
                return false;
            }

            return true;
        }

        protected string GetPathToExecutable(string baseDirectory)
        {
            return Path.Combine(baseDirectory, "Msk", "starfall_game", "Starfall.exe");
        }

        protected void UpdateValues()
        {
            if ((Profile is null) == false)
            {
                NicknameBox.Text = Profile.Nickname;

                var chars = Profile.CharacterModeProfile?.Chars;

                if ((chars?.Count ?? 0) > 0)
                {
                    var character = chars[0];
                    CharacterNameBox.Text = character.Name;
                    CharacterFactionBox.SelectedIndex = Math.Max(0, Math.Min(2, character.Faction));
                }
            }

            if ((Settings is null) == false)
            {
                GameLocationBox.Text = Settings.GameLocation;
                EnableMultiplayerBox.Checked = Settings.EnableMultiplayer;
                ServerAddressBox.Enabled = Settings.EnableMultiplayer;
                ServerAddressBox.Text = Settings.MultiplayerAddress;
                ServerPortBox.Text = Settings.MultiplayerPort.ToString();
                ServerPortBox.Enabled = Settings.EnableMultiplayer;
                ShowGameLogCheckBox.Checked = Settings.ShowGameLog;

                if ((Settings.ServerSettings is null) == false)
                {
                    DedicatedServerAddressBox.Text = Settings.ServerSettings.Address;
                    DedicatedServerPortBox.Text = Settings.ServerSettings.Port.ToString();

                    if ((Settings.ServerSettings.BattlegroundsMode is null) == false)
                    {
                        BattlegroundsRoomSizeBox.ValueChanged -= OnEditBattlegroundsRoomSize;
                        BattlegroundsRoomSizeBox.Value = Settings.ServerSettings.BattlegroundsMode.RoomSize;
                        BattlegroundsRoomSizeBox.ValueChanged += OnEditBattlegroundsRoomSize;
                    }
                }
            }
        }
    }
}
