
namespace StarfallTactics.StarfallTacticsLauncher
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.GamePage = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.DebugGroup = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ShowGameLogCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MultiplayerGroup = new System.Windows.Forms.GroupBox();
            this.ServerAddressProperty = new System.Windows.Forms.Panel();
            this.ServerAddressBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.EnableMultiplayerBox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.GameLocationGroup = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GameLocationBox = new System.Windows.Forms.TextBox();
            this.SelectGameLocationButton = new System.Windows.Forms.Button();
            this.ProfileGroup = new System.Windows.Forms.GroupBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.ResetProfileButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.CharacterFactionBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.CharacterNameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.NicknameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.PlayButton = new System.Windows.Forms.Button();
            this.DedicatedServerPage = new System.Windows.Forms.TabPage();
            this.BattlegroundsGameModeGroup = new System.Windows.Forms.GroupBox();
            this.panel12 = new System.Windows.Forms.Panel();
            this.BattlegroundsRoomSizeBox = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.StartDedicatedServerButton = new System.Windows.Forms.Button();
            this.NetworkGroup = new System.Windows.Forms.GroupBox();
            this.panel14 = new System.Windows.Forms.Panel();
            this.DedicatedServerPortBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.DedicatedServerAddressBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.LogPage = new System.Windows.Forms.TabPage();
            this.LogOutput = new System.Windows.Forms.RichTextBox();
            this.panel15 = new System.Windows.Forms.Panel();
            this.ServerPortBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.GamePage.SuspendLayout();
            this.panel7.SuspendLayout();
            this.DebugGroup.SuspendLayout();
            this.panel6.SuspendLayout();
            this.MultiplayerGroup.SuspendLayout();
            this.ServerAddressProperty.SuspendLayout();
            this.panel11.SuspendLayout();
            this.GameLocationGroup.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.ProfileGroup.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel8.SuspendLayout();
            this.DedicatedServerPage.SuspendLayout();
            this.BattlegroundsGameModeGroup.SuspendLayout();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BattlegroundsRoomSizeBox)).BeginInit();
            this.panel13.SuspendLayout();
            this.NetworkGroup.SuspendLayout();
            this.panel14.SuspendLayout();
            this.panel10.SuspendLayout();
            this.LogPage.SuspendLayout();
            this.panel15.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.GamePage);
            this.tabControl1.Controls.Add(this.DedicatedServerPage);
            this.tabControl1.Controls.Add(this.LogPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(384, 385);
            this.tabControl1.TabIndex = 0;
            // 
            // GamePage
            // 
            this.GamePage.Controls.Add(this.panel7);
            this.GamePage.Controls.Add(this.panel8);
            this.GamePage.Location = new System.Drawing.Point(4, 22);
            this.GamePage.Name = "GamePage";
            this.GamePage.Padding = new System.Windows.Forms.Padding(3);
            this.GamePage.Size = new System.Drawing.Size(376, 359);
            this.GamePage.TabIndex = 0;
            this.GamePage.Text = "Game";
            this.GamePage.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.AutoScroll = true;
            this.panel7.Controls.Add(this.DebugGroup);
            this.panel7.Controls.Add(this.MultiplayerGroup);
            this.panel7.Controls.Add(this.GameLocationGroup);
            this.panel7.Controls.Add(this.ProfileGroup);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(370, 321);
            this.panel7.TabIndex = 3;
            // 
            // DebugGroup
            // 
            this.DebugGroup.AutoSize = true;
            this.DebugGroup.Controls.Add(this.panel6);
            this.DebugGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.DebugGroup.Location = new System.Drawing.Point(0, 269);
            this.DebugGroup.Name = "DebugGroup";
            this.DebugGroup.Padding = new System.Windows.Forms.Padding(6);
            this.DebugGroup.Size = new System.Drawing.Size(370, 49);
            this.DebugGroup.TabIndex = 2;
            this.DebugGroup.TabStop = false;
            this.DebugGroup.Text = "Debug";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.ShowGameLogCheckBox);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(6, 19);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(358, 24);
            this.panel6.TabIndex = 3;
            // 
            // ShowGameLogCheckBox
            // 
            this.ShowGameLogCheckBox.AutoSize = true;
            this.ShowGameLogCheckBox.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ShowGameLogCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowGameLogCheckBox.Location = new System.Drawing.Point(160, 0);
            this.ShowGameLogCheckBox.Name = "ShowGameLogCheckBox";
            this.ShowGameLogCheckBox.Size = new System.Drawing.Size(198, 24);
            this.ShowGameLogCheckBox.TabIndex = 1;
            this.ShowGameLogCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "Show Game Log";
            // 
            // MultiplayerGroup
            // 
            this.MultiplayerGroup.AutoSize = true;
            this.MultiplayerGroup.Controls.Add(this.panel15);
            this.MultiplayerGroup.Controls.Add(this.ServerAddressProperty);
            this.MultiplayerGroup.Controls.Add(this.panel11);
            this.MultiplayerGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.MultiplayerGroup.Location = new System.Drawing.Point(0, 172);
            this.MultiplayerGroup.Name = "MultiplayerGroup";
            this.MultiplayerGroup.Padding = new System.Windows.Forms.Padding(6);
            this.MultiplayerGroup.Size = new System.Drawing.Size(370, 97);
            this.MultiplayerGroup.TabIndex = 3;
            this.MultiplayerGroup.TabStop = false;
            this.MultiplayerGroup.Text = "Multiplayer";
            // 
            // ServerAddressProperty
            // 
            this.ServerAddressProperty.Controls.Add(this.ServerAddressBox);
            this.ServerAddressProperty.Controls.Add(this.label6);
            this.ServerAddressProperty.Dock = System.Windows.Forms.DockStyle.Top;
            this.ServerAddressProperty.Location = new System.Drawing.Point(6, 43);
            this.ServerAddressProperty.Name = "ServerAddressProperty";
            this.ServerAddressProperty.Size = new System.Drawing.Size(358, 24);
            this.ServerAddressProperty.TabIndex = 1;
            // 
            // ServerAddressBox
            // 
            this.ServerAddressBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerAddressBox.Location = new System.Drawing.Point(160, 0);
            this.ServerAddressBox.Name = "ServerAddressBox";
            this.ServerAddressBox.Size = new System.Drawing.Size(198, 20);
            this.ServerAddressBox.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label6.Size = new System.Drawing.Size(160, 24);
            this.label6.TabIndex = 0;
            this.label6.Text = "Server Address";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.EnableMultiplayerBox);
            this.panel11.Controls.Add(this.label7);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(6, 19);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(358, 24);
            this.panel11.TabIndex = 4;
            // 
            // EnableMultiplayerBox
            // 
            this.EnableMultiplayerBox.AutoSize = true;
            this.EnableMultiplayerBox.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.EnableMultiplayerBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnableMultiplayerBox.Location = new System.Drawing.Point(160, 0);
            this.EnableMultiplayerBox.Name = "EnableMultiplayerBox";
            this.EnableMultiplayerBox.Size = new System.Drawing.Size(198, 24);
            this.EnableMultiplayerBox.TabIndex = 1;
            this.EnableMultiplayerBox.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 24);
            this.label7.TabIndex = 0;
            this.label7.Text = "Enable Multiplayer";
            // 
            // GameLocationGroup
            // 
            this.GameLocationGroup.AutoSize = true;
            this.GameLocationGroup.Controls.Add(this.panel1);
            this.GameLocationGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.GameLocationGroup.Location = new System.Drawing.Point(0, 121);
            this.GameLocationGroup.Name = "GameLocationGroup";
            this.GameLocationGroup.Padding = new System.Windows.Forms.Padding(6);
            this.GameLocationGroup.Size = new System.Drawing.Size(370, 51);
            this.GameLocationGroup.TabIndex = 0;
            this.GameLocationGroup.TabStop = false;
            this.GameLocationGroup.Text = "Game Location";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.SelectGameLocationButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 26);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GameLocationBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 3, 6, 0);
            this.panel2.Size = new System.Drawing.Size(308, 26);
            this.panel2.TabIndex = 1;
            // 
            // GameLocationBox
            // 
            this.GameLocationBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GameLocationBox.Location = new System.Drawing.Point(0, 3);
            this.GameLocationBox.Name = "GameLocationBox";
            this.GameLocationBox.Size = new System.Drawing.Size(302, 20);
            this.GameLocationBox.TabIndex = 0;
            // 
            // SelectGameLocationButton
            // 
            this.SelectGameLocationButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.SelectGameLocationButton.Location = new System.Drawing.Point(308, 0);
            this.SelectGameLocationButton.Name = "SelectGameLocationButton";
            this.SelectGameLocationButton.Size = new System.Drawing.Size(50, 26);
            this.SelectGameLocationButton.TabIndex = 1;
            this.SelectGameLocationButton.Text = "Select";
            this.SelectGameLocationButton.UseVisualStyleBackColor = true;
            // 
            // ProfileGroup
            // 
            this.ProfileGroup.AutoSize = true;
            this.ProfileGroup.Controls.Add(this.panel9);
            this.ProfileGroup.Controls.Add(this.panel5);
            this.ProfileGroup.Controls.Add(this.panel4);
            this.ProfileGroup.Controls.Add(this.panel3);
            this.ProfileGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.ProfileGroup.Location = new System.Drawing.Point(0, 0);
            this.ProfileGroup.Name = "ProfileGroup";
            this.ProfileGroup.Padding = new System.Windows.Forms.Padding(6);
            this.ProfileGroup.Size = new System.Drawing.Size(370, 121);
            this.ProfileGroup.TabIndex = 1;
            this.ProfileGroup.TabStop = false;
            this.ProfileGroup.Text = "Profile";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.ResetProfileButton);
            this.panel9.Controls.Add(this.label5);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(6, 91);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(358, 24);
            this.panel9.TabIndex = 3;
            // 
            // ResetProfileButton
            // 
            this.ResetProfileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResetProfileButton.Location = new System.Drawing.Point(160, 0);
            this.ResetProfileButton.Name = "ResetProfileButton";
            this.ResetProfileButton.Size = new System.Drawing.Size(198, 24);
            this.ResetProfileButton.TabIndex = 1;
            this.ResetProfileButton.Text = "Reset";
            this.ResetProfileButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = "Reset Profile";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.CharacterFactionBox);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(6, 67);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(358, 24);
            this.panel5.TabIndex = 2;
            // 
            // CharacterFactionBox
            // 
            this.CharacterFactionBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CharacterFactionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CharacterFactionBox.FormattingEnabled = true;
            this.CharacterFactionBox.Items.AddRange(new object[] {
            "Deprived",
            "Eclipse",
            "Vanguard"});
            this.CharacterFactionBox.Location = new System.Drawing.Point(160, 0);
            this.CharacterFactionBox.Name = "CharacterFactionBox";
            this.CharacterFactionBox.Size = new System.Drawing.Size(198, 21);
            this.CharacterFactionBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Character Faction";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.CharacterNameBox);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(6, 43);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(358, 24);
            this.panel4.TabIndex = 1;
            // 
            // CharacterNameBox
            // 
            this.CharacterNameBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CharacterNameBox.Location = new System.Drawing.Point(160, 0);
            this.CharacterNameBox.Name = "CharacterNameBox";
            this.CharacterNameBox.Size = new System.Drawing.Size(198, 20);
            this.CharacterNameBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "Character Name";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.NicknameBox);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(6, 19);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(358, 24);
            this.panel3.TabIndex = 0;
            // 
            // NicknameBox
            // 
            this.NicknameBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NicknameBox.Location = new System.Drawing.Point(160, 0);
            this.NicknameBox.Name = "NicknameBox";
            this.NicknameBox.Size = new System.Drawing.Size(198, 20);
            this.NicknameBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nickname";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.PlayButton);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panel8.Location = new System.Drawing.Point(3, 324);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(370, 32);
            this.panel8.TabIndex = 4;
            // 
            // PlayButton
            // 
            this.PlayButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.PlayButton.Location = new System.Drawing.Point(290, 0);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(80, 32);
            this.PlayButton.TabIndex = 0;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            // 
            // DedicatedServerPage
            // 
            this.DedicatedServerPage.Controls.Add(this.BattlegroundsGameModeGroup);
            this.DedicatedServerPage.Controls.Add(this.panel13);
            this.DedicatedServerPage.Controls.Add(this.NetworkGroup);
            this.DedicatedServerPage.Location = new System.Drawing.Point(4, 22);
            this.DedicatedServerPage.Name = "DedicatedServerPage";
            this.DedicatedServerPage.Padding = new System.Windows.Forms.Padding(3);
            this.DedicatedServerPage.Size = new System.Drawing.Size(376, 359);
            this.DedicatedServerPage.TabIndex = 3;
            this.DedicatedServerPage.Text = "Dedicated Server";
            this.DedicatedServerPage.UseVisualStyleBackColor = true;
            // 
            // BattlegroundsGameModeGroup
            // 
            this.BattlegroundsGameModeGroup.AutoSize = true;
            this.BattlegroundsGameModeGroup.Controls.Add(this.panel12);
            this.BattlegroundsGameModeGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.BattlegroundsGameModeGroup.Location = new System.Drawing.Point(3, 76);
            this.BattlegroundsGameModeGroup.Name = "BattlegroundsGameModeGroup";
            this.BattlegroundsGameModeGroup.Padding = new System.Windows.Forms.Padding(6);
            this.BattlegroundsGameModeGroup.Size = new System.Drawing.Size(370, 49);
            this.BattlegroundsGameModeGroup.TabIndex = 6;
            this.BattlegroundsGameModeGroup.TabStop = false;
            this.BattlegroundsGameModeGroup.Text = "Battlegrounds Game Mode";
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.BattlegroundsRoomSizeBox);
            this.panel12.Controls.Add(this.label9);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(6, 19);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(358, 24);
            this.panel12.TabIndex = 1;
            // 
            // BattlegroundsRoomSizeBox
            // 
            this.BattlegroundsRoomSizeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BattlegroundsRoomSizeBox.Location = new System.Drawing.Point(160, 0);
            this.BattlegroundsRoomSizeBox.Name = "BattlegroundsRoomSizeBox";
            this.BattlegroundsRoomSizeBox.Size = new System.Drawing.Size(198, 20);
            this.BattlegroundsRoomSizeBox.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Left;
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label9.Size = new System.Drawing.Size(160, 24);
            this.label9.TabIndex = 0;
            this.label9.Text = "Room Size";
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.StartDedicatedServerButton);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panel13.Location = new System.Drawing.Point(3, 324);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(370, 32);
            this.panel13.TabIndex = 5;
            // 
            // StartDedicatedServerButton
            // 
            this.StartDedicatedServerButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.StartDedicatedServerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.4F);
            this.StartDedicatedServerButton.Location = new System.Drawing.Point(250, 0);
            this.StartDedicatedServerButton.Name = "StartDedicatedServerButton";
            this.StartDedicatedServerButton.Size = new System.Drawing.Size(120, 32);
            this.StartDedicatedServerButton.TabIndex = 0;
            this.StartDedicatedServerButton.Text = "Start Server";
            this.StartDedicatedServerButton.UseVisualStyleBackColor = true;
            // 
            // NetworkGroup
            // 
            this.NetworkGroup.AutoSize = true;
            this.NetworkGroup.Controls.Add(this.panel14);
            this.NetworkGroup.Controls.Add(this.panel10);
            this.NetworkGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.NetworkGroup.Location = new System.Drawing.Point(3, 3);
            this.NetworkGroup.Name = "NetworkGroup";
            this.NetworkGroup.Padding = new System.Windows.Forms.Padding(6);
            this.NetworkGroup.Size = new System.Drawing.Size(370, 73);
            this.NetworkGroup.TabIndex = 4;
            this.NetworkGroup.TabStop = false;
            this.NetworkGroup.Text = "Network";
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.DedicatedServerPortBox);
            this.panel14.Controls.Add(this.label10);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel14.Location = new System.Drawing.Point(6, 43);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(358, 24);
            this.panel14.TabIndex = 2;
            // 
            // DedicatedServerPortBox
            // 
            this.DedicatedServerPortBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DedicatedServerPortBox.Location = new System.Drawing.Point(160, 0);
            this.DedicatedServerPortBox.Name = "DedicatedServerPortBox";
            this.DedicatedServerPortBox.Size = new System.Drawing.Size(198, 20);
            this.DedicatedServerPortBox.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Left;
            this.label10.Location = new System.Drawing.Point(0, 0);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label10.Size = new System.Drawing.Size(160, 24);
            this.label10.TabIndex = 0;
            this.label10.Text = "Port";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.DedicatedServerAddressBox);
            this.panel10.Controls.Add(this.label8);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(6, 19);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(358, 24);
            this.panel10.TabIndex = 1;
            // 
            // DedicatedServerAddressBox
            // 
            this.DedicatedServerAddressBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DedicatedServerAddressBox.FormattingEnabled = true;
            this.DedicatedServerAddressBox.Location = new System.Drawing.Point(160, 0);
            this.DedicatedServerAddressBox.Name = "DedicatedServerAddressBox";
            this.DedicatedServerAddressBox.Size = new System.Drawing.Size(198, 21);
            this.DedicatedServerAddressBox.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(160, 24);
            this.label8.TabIndex = 0;
            this.label8.Text = "Address";
            // 
            // LogPage
            // 
            this.LogPage.Controls.Add(this.LogOutput);
            this.LogPage.Location = new System.Drawing.Point(4, 22);
            this.LogPage.Margin = new System.Windows.Forms.Padding(0);
            this.LogPage.Name = "LogPage";
            this.LogPage.Size = new System.Drawing.Size(376, 359);
            this.LogPage.TabIndex = 2;
            this.LogPage.Text = "Log";
            this.LogPage.UseVisualStyleBackColor = true;
            // 
            // LogOutput
            // 
            this.LogOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.LogOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LogOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogOutput.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LogOutput.ForeColor = System.Drawing.Color.White;
            this.LogOutput.Location = new System.Drawing.Point(0, 0);
            this.LogOutput.Margin = new System.Windows.Forms.Padding(0);
            this.LogOutput.Name = "LogOutput";
            this.LogOutput.ReadOnly = true;
            this.LogOutput.Size = new System.Drawing.Size(376, 359);
            this.LogOutput.TabIndex = 0;
            this.LogOutput.Text = "Version 0.1.8";
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.ServerPortBox);
            this.panel15.Controls.Add(this.label11);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel15.Location = new System.Drawing.Point(6, 67);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(358, 24);
            this.panel15.TabIndex = 5;
            // 
            // ServerPortBox
            // 
            this.ServerPortBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerPortBox.Location = new System.Drawing.Point(160, 0);
            this.ServerPortBox.Name = "ServerPortBox";
            this.ServerPortBox.Size = new System.Drawing.Size(198, 20);
            this.ServerPortBox.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Left;
            this.label11.Location = new System.Drawing.Point(0, 0);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label11.Size = new System.Drawing.Size(160, 24);
            this.label11.TabIndex = 0;
            this.label11.Text = "Server Port";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 385);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Starfall Tactics Launcher";
            this.tabControl1.ResumeLayout(false);
            this.GamePage.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.DebugGroup.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.MultiplayerGroup.ResumeLayout(false);
            this.ServerAddressProperty.ResumeLayout(false);
            this.ServerAddressProperty.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.GameLocationGroup.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ProfileGroup.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.DedicatedServerPage.ResumeLayout(false);
            this.DedicatedServerPage.PerformLayout();
            this.BattlegroundsGameModeGroup.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BattlegroundsRoomSizeBox)).EndInit();
            this.panel13.ResumeLayout(false);
            this.NetworkGroup.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.LogPage.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage GamePage;
        private System.Windows.Forms.TabPage LogPage;
        private System.Windows.Forms.GroupBox GameLocationGroup;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox GameLocationBox;
        private System.Windows.Forms.Button SelectGameLocationButton;
        private System.Windows.Forms.GroupBox ProfileGroup;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox NicknameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox CharacterFactionBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox CharacterNameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox DebugGroup;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.CheckBox ShowGameLogCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button ResetProfileButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox LogOutput;
        private System.Windows.Forms.TabPage DedicatedServerPage;
        private System.Windows.Forms.Button StartDedicatedServerButton;
        private System.Windows.Forms.GroupBox MultiplayerGroup;
        private System.Windows.Forms.Panel ServerAddressProperty;
        private System.Windows.Forms.TextBox ServerAddressBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.CheckBox EnableMultiplayerBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox NetworkGroup;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.GroupBox BattlegroundsGameModeGroup;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown BattlegroundsRoomSizeBox;
        private System.Windows.Forms.ComboBox DedicatedServerAddressBox;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.TextBox DedicatedServerPortBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.TextBox ServerPortBox;
        private System.Windows.Forms.Label label11;
    }
}

