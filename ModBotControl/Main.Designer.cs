namespace ModBotControl
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_twtichConnect = new Button();
            tb_streamerName = new TextBox();
            lb_streamerName = new Label();
            lb_obsUrl = new Label();
            tb_obsUrl = new TextBox();
            lb_obsPassword = new Label();
            tb_obsPassword = new TextBox();
            lstbx_Scences = new ListBox();
            lstbx_Sources = new ListBox();
            lb_scences = new Label();
            lb_sources = new Label();
            btn_startReplay = new Button();
            lb_createdClips = new Label();
            lb_replayLength = new Label();
            tb_replayLength = new TextBox();
            cb_replayKey = new ComboBox();
            lb_replayKey = new Label();
            rtb_clipsCreated = new RichTextBox();
            lb_redemptions = new Label();
            cb_redemptions = new ComboBox();
            chkbx_twitchAPI = new CheckBox();
            chkbx_twitchWSS = new CheckBox();
            SuspendLayout();
            // 
            // btn_twtichConnect
            // 
            btn_twtichConnect.Location = new Point(357, 28);
            btn_twtichConnect.Name = "btn_twtichConnect";
            btn_twtichConnect.Size = new Size(140, 23);
            btn_twtichConnect.TabIndex = 0;
            btn_twtichConnect.Text = "Connect To Twitch";
            btn_twtichConnect.UseVisualStyleBackColor = true;
            btn_twtichConnect.Click += ConnectToTwitch_Click;
            // 
            // tb_streamerName
            // 
            tb_streamerName.Location = new Point(133, 29);
            tb_streamerName.Name = "tb_streamerName";
            tb_streamerName.Size = new Size(194, 23);
            tb_streamerName.TabIndex = 1;
            tb_streamerName.Leave += tb_streamerName_Leave;
            // 
            // lb_streamerName
            // 
            lb_streamerName.AutoSize = true;
            lb_streamerName.Location = new Point(38, 32);
            lb_streamerName.Name = "lb_streamerName";
            lb_streamerName.Size = new Size(89, 15);
            lb_streamerName.TabIndex = 2;
            lb_streamerName.Text = "Streamer Name";
            // 
            // lb_obsUrl
            // 
            lb_obsUrl.AutoSize = true;
            lb_obsUrl.Location = new Point(38, 66);
            lb_obsUrl.Name = "lb_obsUrl";
            lb_obsUrl.Size = new Size(53, 15);
            lb_obsUrl.TabIndex = 4;
            lb_obsUrl.Text = "OBS URL";
            // 
            // tb_obsUrl
            // 
            tb_obsUrl.Location = new Point(133, 63);
            tb_obsUrl.Name = "tb_obsUrl";
            tb_obsUrl.Size = new Size(194, 23);
            tb_obsUrl.TabIndex = 3;
            tb_obsUrl.Leave += tb_obsUrl_Leave;
            // 
            // lb_obsPassword
            // 
            lb_obsPassword.AutoSize = true;
            lb_obsPassword.Location = new Point(38, 106);
            lb_obsPassword.Name = "lb_obsPassword";
            lb_obsPassword.Size = new Size(82, 15);
            lb_obsPassword.TabIndex = 6;
            lb_obsPassword.Text = "OBS Password";
            // 
            // tb_obsPassword
            // 
            tb_obsPassword.Location = new Point(133, 103);
            tb_obsPassword.Name = "tb_obsPassword";
            tb_obsPassword.Size = new Size(194, 23);
            tb_obsPassword.TabIndex = 5;
            tb_obsPassword.Leave += tb_obsPassword_Leave;
            // 
            // lstbx_Scences
            // 
            lstbx_Scences.FormattingEnabled = true;
            lstbx_Scences.ItemHeight = 15;
            lstbx_Scences.Location = new Point(557, 32);
            lstbx_Scences.Name = "lstbx_Scences";
            lstbx_Scences.Size = new Size(120, 259);
            lstbx_Scences.TabIndex = 8;
            lstbx_Scences.SelectedIndexChanged += lstbx_Scences_IndexChanged;
            // 
            // lstbx_Sources
            // 
            lstbx_Sources.FormattingEnabled = true;
            lstbx_Sources.ItemHeight = 15;
            lstbx_Sources.Location = new Point(753, 32);
            lstbx_Sources.Name = "lstbx_Sources";
            lstbx_Sources.Size = new Size(120, 334);
            lstbx_Sources.TabIndex = 9;
            // 
            // lb_scences
            // 
            lb_scences.AutoSize = true;
            lb_scences.Location = new Point(503, 32);
            lb_scences.Name = "lb_scences";
            lb_scences.Size = new Size(49, 15);
            lb_scences.TabIndex = 10;
            lb_scences.Text = "Scences";
            // 
            // lb_sources
            // 
            lb_sources.AutoSize = true;
            lb_sources.Location = new Point(699, 32);
            lb_sources.Name = "lb_sources";
            lb_sources.Size = new Size(48, 15);
            lb_sources.TabIndex = 11;
            lb_sources.Text = "Sources";
            // 
            // btn_startReplay
            // 
            btn_startReplay.Location = new Point(357, 63);
            btn_startReplay.Name = "btn_startReplay";
            btn_startReplay.Size = new Size(140, 23);
            btn_startReplay.TabIndex = 12;
            btn_startReplay.Text = "Enable Replays";
            btn_startReplay.UseVisualStyleBackColor = true;
            btn_startReplay.Click += btn_startReplay_Click;
            // 
            // lb_createdClips
            // 
            lb_createdClips.AutoSize = true;
            lb_createdClips.Location = new Point(38, 295);
            lb_createdClips.Name = "lb_createdClips";
            lb_createdClips.Size = new Size(77, 15);
            lb_createdClips.TabIndex = 14;
            lb_createdClips.Text = "Clips Created";
            // 
            // lb_replayLength
            // 
            lb_replayLength.AutoSize = true;
            lb_replayLength.Location = new Point(38, 143);
            lb_replayLength.Name = "lb_replayLength";
            lb_replayLength.Size = new Size(82, 15);
            lb_replayLength.TabIndex = 16;
            lb_replayLength.Text = "Replay Length";
            // 
            // tb_replayLength
            // 
            tb_replayLength.Location = new Point(133, 140);
            tb_replayLength.Name = "tb_replayLength";
            tb_replayLength.Size = new Size(50, 23);
            tb_replayLength.TabIndex = 17;
            tb_replayLength.Leave += tb_replayLength_Leave;
            // 
            // cb_replayKey
            // 
            cb_replayKey.FormattingEnabled = true;
            cb_replayKey.Location = new Point(133, 183);
            cb_replayKey.Name = "cb_replayKey";
            cb_replayKey.Size = new Size(121, 23);
            cb_replayKey.TabIndex = 18;
            cb_replayKey.SelectedIndexChanged += cb_replayKey_SelectedIndexChanged;
            // 
            // lb_replayKey
            // 
            lb_replayKey.AutoSize = true;
            lb_replayKey.Location = new Point(38, 186);
            lb_replayKey.Name = "lb_replayKey";
            lb_replayKey.Size = new Size(83, 15);
            lb_replayKey.TabIndex = 19;
            lb_replayKey.Text = "Replay Hotkey";
            // 
            // rtb_clipsCreated
            // 
            rtb_clipsCreated.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            rtb_clipsCreated.BackColor = Color.White;
            rtb_clipsCreated.Location = new Point(38, 326);
            rtb_clipsCreated.Name = "rtb_clipsCreated";
            rtb_clipsCreated.ReadOnly = true;
            rtb_clipsCreated.Size = new Size(493, 154);
            rtb_clipsCreated.TabIndex = 21;
            rtb_clipsCreated.Text = "";
            // 
            // lb_redemptions
            // 
            lb_redemptions.AutoSize = true;
            lb_redemptions.Location = new Point(38, 229);
            lb_redemptions.Name = "lb_redemptions";
            lb_redemptions.Size = new Size(118, 15);
            lb_redemptions.TabIndex = 22;
            lb_redemptions.Text = "Redemptions Hotkey";
            // 
            // cb_redemptions
            // 
            cb_redemptions.FormattingEnabled = true;
            cb_redemptions.Location = new Point(162, 226);
            cb_redemptions.Name = "cb_redemptions";
            cb_redemptions.Size = new Size(121, 23);
            cb_redemptions.TabIndex = 23;
            cb_redemptions.SelectedIndexChanged += cb_redemptions_SelectedIndexChanged;
            // 
            // chkbx_twitchAPI
            // 
            chkbx_twitchAPI.AutoSize = true;
            chkbx_twitchAPI.Enabled = false;
            chkbx_twitchAPI.Location = new Point(357, 139);
            chkbx_twitchAPI.Name = "chkbx_twitchAPI";
            chkbx_twitchAPI.Size = new Size(135, 19);
            chkbx_twitchAPI.TabIndex = 24;
            chkbx_twitchAPI.Text = "Connected to Twitch";
            chkbx_twitchAPI.UseVisualStyleBackColor = true;
            // 
            // chkbx_twitchWSS
            // 
            chkbx_twitchWSS.AutoSize = true;
            chkbx_twitchWSS.Enabled = false;
            chkbx_twitchWSS.Location = new Point(357, 164);
            chkbx_twitchWSS.Name = "chkbx_twitchWSS";
            chkbx_twitchWSS.Size = new Size(141, 19);
            chkbx_twitchWSS.TabIndex = 25;
            chkbx_twitchWSS.Text = "Shoutouts Connected";
            chkbx_twitchWSS.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(929, 492);
            Controls.Add(chkbx_twitchWSS);
            Controls.Add(chkbx_twitchAPI);
            Controls.Add(cb_redemptions);
            Controls.Add(lb_redemptions);
            Controls.Add(rtb_clipsCreated);
            Controls.Add(lb_replayKey);
            Controls.Add(cb_replayKey);
            Controls.Add(tb_replayLength);
            Controls.Add(lb_replayLength);
            Controls.Add(lb_createdClips);
            Controls.Add(btn_startReplay);
            Controls.Add(lb_sources);
            Controls.Add(lb_scences);
            Controls.Add(lstbx_Sources);
            Controls.Add(lstbx_Scences);
            Controls.Add(lb_obsPassword);
            Controls.Add(tb_obsPassword);
            Controls.Add(lb_obsUrl);
            Controls.Add(tb_obsUrl);
            Controls.Add(lb_streamerName);
            Controls.Add(tb_streamerName);
            Controls.Add(btn_twtichConnect);
            Name = "Main";
            Text = "Twitch Small Streamer Modbot";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_twtichConnect;
        private TextBox tb_streamerName;
        private Label lb_streamerName;
        private Label lb_obsUrl;
        private TextBox tb_obsUrl;
        private Label lb_obsPassword;
        private TextBox tb_obsPassword;
        private ListBox lstbx_Scences;
        private ListBox lstbx_Sources;
        private Label lb_scences;
        private Label lb_sources;
        private Button btn_startReplay;
        private Label lb_createdClips;
        private Label lb_replayLength;
        private TextBox tb_replayLength;
        private ComboBox cb_replayKey;
        private Label lb_replayKey;
        private RichTextBox rtb_clipsCreated;
        private Label lb_redemptions;
        private ComboBox cb_redemptions;
        private CheckBox chkbx_twitchAPI;
        private CheckBox chkbx_twitchWSS;
    }
}