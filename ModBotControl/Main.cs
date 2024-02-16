using Newtonsoft.Json.Linq;
using OBSConnector;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TwitchAPI;
using TwitchAPI.API.Core;
using TwitchAPI.Models;
using OBSWebsocketDotNet.Types;
using TwitchAPI.API.Models;

namespace ModBotControl
{
    public partial class Main : Form
    {
        private TwtichLogin login;
        private string configPath = @"config.xml";

        private string twitchDisconnectText = "Disconnect From Twitch";
        private string twitchConnectText = "Connect To Twitch";
        private bool connnectedToTwitch = false;
        private KeyHandler ghk;

        private TwitchAPI.StartConnection twitchApi;
        private OBSConnector.StartConnection obsConnector;

        public Main()
        {
            InitializeComponent();
            btn_twtichConnect.Enabled = connnectedToTwitch;
            btn_twtichConnect.Text = twitchConnectText;
            LoadConfig();
            CheckValuesTwitchEnable();
            LoadHotKeyOptions(cb_replayKey);
            LoadHotKeyOptions(cb_redemptions);
        }

        internal void LoadScences()
        {
            GetScenceAndSourceInfo scenceAndSourceInfo = new GetScenceAndSourceInfo();

            while (!scenceAndSourceInfo.obs.IsConnected)
            {
                Thread.Sleep(100);
            }
            if (InvokeRequired)
            {
                // If we're not on the UI thread, invoke this method on the UI thread
                BeginInvoke(new Action(LoadScences));
            }
            else
            {
                lstbx_Scences.Items.Clear();

                var scences = scenceAndSourceInfo.GetScences();
                foreach (var scence in scences.Scenes)
                {
                    lstbx_Scences.Items.Add(scence.Name);
                }
            }

        }

        private void SetReplayKey()
        {
            Enum.TryParse(cb_replayKey.SelectedItem.ToString(), out Keys hotkey);
            ghk = new KeyHandler(hotkey, this);
            ghk.Register();
        }
        private void LoadHotKeyOptions(ComboBox comboBox)
        {
            string defaultText = "Select Key";
            Keys[] keys = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToArray();

            foreach (Keys key in keys)
            {
                comboBox.Items.Add(key);
            }
            string hotkey = GetItemFromConfig("replayHotkey");
            if (hotkey != "")
            {
                Enum.TryParse(hotkey, out Keys replayHotkey);
                cb_replayKey.SelectedItem = replayHotkey;
            }
            else
            {
                comboBox.Items.Add(defaultText);
                comboBox.SelectedItem = defaultText;
            }

        }
        private void lstbx_Scences_IndexChanged(object sender, EventArgs e)
        {
            GetScenceAndSourceInfo scenceAndSourceInfo = new GetScenceAndSourceInfo();
            List<SceneItemDetails> sources = scenceAndSourceInfo.GetSources(lstbx_Scences.Text);
            lstbx_Sources.Items.Clear();
            foreach (SceneItemDetails source in sources)
            {
                lstbx_Sources.Items.Add(source.SourceName);
            }

        }
        private void btn_startReplay_Click(object sender, EventArgs e)
        {

            if (btn_startReplay.Text == "Disable Replays")
            {
                lstbx_Scences.Enabled = true;
                lstbx_Sources.Enabled = true;
                tb_replayLength.Enabled = true;
                cb_replayKey.Enabled = true;
                ReplayBuffer rb = new ReplayBuffer();
                rb.StopReplay();
                ghk.Unregiser();
                btn_startReplay.Text = "Enable Replays";
            }
            else if (btn_startReplay.Text == "Enable Replays")
            {
                if (!string.IsNullOrEmpty(lstbx_Scences.Text) && !string.IsNullOrEmpty(lstbx_Sources.Text) && !string.IsNullOrEmpty(tb_replayLength.Text))
                {
                    lstbx_Scences.Enabled = false;
                    lstbx_Sources.Enabled = false;
                    tb_replayLength.Enabled = false;
                    cb_replayKey.Enabled = false;
                    ReplayBuffer rb = new ReplayBuffer();
                    SetReplayKey();
                    rb.StartReplay(lstbx_Scences.Text, lstbx_Sources.Text);
                    btn_startReplay.Text = "Disable Replays";
                }
                else
                {
                    string message = "Please make sure a scence, source, and replay length is set";
                    string title = "Error";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBox.Show(message, title, buttons);
                }
            }




        }
        private void LoadConfig()
        {
            tb_streamerName.Text = GetItemFromConfig("streamer");
            tb_obsUrl.Text = GetItemFromConfig("obsUrl");
            tb_obsPassword.Text = GetItemFromConfig("obsPassword");
            tb_replayLength.Text = GetItemFromConfig("replayLength");
        }
        private string GetItemFromConfig(string elementKey)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configPath);

            // Get the value of the 'streamer' element
            XmlNode node = xmlDoc.SelectSingleNode($"/config/{elementKey}");

            if (node != null)
            {
                return node.InnerText;
            }
            else
            {
                return "";
            }
        }
        private bool SaveItemToConfig(string elementKey, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configPath);

            // Get the streamer node
            XmlNode node = xmlDoc.SelectSingleNode($"/config/{elementKey}");

            if (node != null)
            {
                node.InnerText = value;
                xmlDoc.Save(configPath);

                return true;
            }
            else
            {
                return false;
            }
        }
        private void ConnectToTwitch_Click(object sender, EventArgs e)
        {
            if (connnectedToTwitch == false)
            {
                login = new TwtichLogin();
                DataToken token = new DataToken();
                Tuple<string, string> urlState = Authenticate.GenerateAuthUrl();
                login.TwitchWebView.Source = new Uri(urlState.Item1);
                login.Show();
                Task<Tuple<DataToken, ClientInfo>> tokenResult = Task.Run(() => Authenticate.StartOauth(urlState.Item2));

                Task continuationTask = tokenResult.ContinueWith((completedTask) =>
                {
                    CloseFormOnUIThread();
                    StartBot(tokenResult.Result.Item1, tb_streamerName.Text, tb_obsUrl.Text, tb_obsPassword.Text, tokenResult.Result.Item2);

                });
            }
            else if (connnectedToTwitch == true)
            {
                StopBot();
                UpdateTwitchConnectionStatus(false);
            }



        }
        private void StartBot(DataToken token, string streamer, string obsWsUrl, string obsWsPassword, ClientInfo clientInfo)
        {
            twitchApi = new TwitchAPI.StartConnection(token, streamer, clientInfo);
            obsConnector = new OBSConnector.StartConnection(obsWsUrl, obsWsPassword);
            Task.Run(() => LoadScences());
            
        }

        private void StopBot()
        {
            obsConnector.Dispose();
            twitchApi.Dispose();
            
        }
        private void CloseFormOnUIThread()
        {
            if (InvokeRequired)
            {
                // If we're not on the UI thread, invoke this method on the UI thread
                BeginInvoke(new Action(CloseFormOnUIThread));
            }
            else
            {
                login.Close();
                UpdateTwitchConnectionStatus(true);

            }
        }

        private void UpdateTwitchConnectionStatus(bool connected)
        {
            connnectedToTwitch = connected;
            tb_streamerName.Enabled = !connnectedToTwitch;
            tb_obsUrl.Enabled = !connnectedToTwitch;
            tb_obsPassword.Enabled = !connnectedToTwitch;
            cb_redemptions.Enabled = !connnectedToTwitch;
            if (connnectedToTwitch == false)
            {
                btn_twtichConnect.Text = twitchConnectText;
            }
            else if (connnectedToTwitch == true)
            {
                btn_twtichConnect.Text = twitchDisconnectText;
            }

        }

        private void CheckValuesTwitchEnable()
        {
            if (!string.IsNullOrEmpty(tb_streamerName.Text) && !string.IsNullOrEmpty(tb_obsUrl.Text) && !string.IsNullOrEmpty(tb_obsPassword.Text) && !string.IsNullOrEmpty(cb_redemptions.Text))
            {
                btn_twtichConnect.Enabled = true;
            }
        }
        private void tb_streamerName_Leave(object sender, EventArgs e)
        {
            CheckValuesTwitchEnable();
            SaveItemToConfig("streamer", tb_streamerName.Text);
        }
        private void tb_obsUrl_Leave(object sender, EventArgs e)
        {
            CheckValuesTwitchEnable();
            SaveItemToConfig("obsUrl", tb_obsUrl.Text);
        }
        private void tb_obsPassword_Leave(object sender, EventArgs e)
        {
            CheckValuesTwitchEnable();
            SaveItemToConfig("obsPassword", tb_obsPassword.Text);
        }
        private void tb_replayLength_Leave(object sender, EventArgs e)
        {
            if (Int32.TryParse(tb_replayLength.Text, out int replayLength))
            {
                SaveItemToConfig("replayLength", tb_replayLength.Text);
            }
            else
            {
                tb_replayLength.Text = "";
                string message = "Please enter a valid number";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons);
            }


        }
        private void HandleHotkey()
        {

            if (tb_replayLength.Enabled == true)
            {
                string message = "Please enable replays";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons);
            }
            else
            {
                ReplayBuffer rb = new ReplayBuffer();
                int clipLengthMilli = Int32.Parse(tb_replayLength.Text) * 1000;
                ClipList clips = rb.SaveReplay(lstbx_Scences.Text, lstbx_Sources.Text, tb_streamerName.Text, clipLengthMilli);
                if (clips.ClipData is not null)
                {
                    foreach (var clip in clips.ClipData)
                    {
                        rtb_clipsCreated.AppendText($"{clip.EditUrl}\r\n");
                    }
                }

            }

        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }
        private void cb_replayKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveItemToConfig("replayHotkey", cb_replayKey.Text);
        }

        private void cb_redemptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveItemToConfig("redemptionHotkey", cb_redemptions.Text);
            CheckValuesTwitchEnable();
        }
    }
}