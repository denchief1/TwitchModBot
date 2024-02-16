using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OBSConnector.Models;
using OBSConnector.Websocket;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.API.Core;
using TwitchAPI.API.Endpoints;
using TwitchAPI.API.Models;

namespace OBSConnector
{
    public  class ReplayBuffer
    {
        protected OBSWebsocket obs;

        private string sceneName = "";
        private string replaySourceName = "";
        private int clipLength = 0;
        public ReplayBuffer()
        {
            obs = StartConnection.host.Services.GetRequiredService<OBSConnection>().GetObsWS();
            
        }

        public void StartReplay(string sceneName, string replaySourceName)
        {
            if (!obs.GetReplayBufferStatus())
            {
                obs.StartReplayBuffer();
            }
            int replaySourceID = obs.GetSceneItemId(sceneName, replaySourceName, 0);
            obs.SetSceneItemEnabled(sceneName, replaySourceID, false);
            
        }

        public void StopReplay()
        {
            if (obs.GetReplayBufferStatus())
            {
                obs.StopReplayBuffer();
            }
        }

        public ClipList CreateClip(string broadcasterLogin)
        {
            ClipEndpoint clipEndpoint = new ClipEndpoint();
            Tuple<bool,string> result = clipEndpoint.CreateClipByLogin(broadcasterLogin);
            ClipList clipList = JsonConvert.DeserializeObject<ClipList>(result.Item2);
            return clipList;
        }

        public ClipList SaveReplay(string sceneName, string replaySourceName,string broadcasterLogin, int clipLength)
        {

            Task<ClipList> clipList = Task.Run(() => CreateClip(broadcasterLogin));
            TriggerOBSReplay(sceneName, replaySourceName, clipLength);
            return clipList.Result;
        }

        public void TriggerOBSReplay(string _sceneName, string _replaySourceName, int _clipLength)
        {
            sceneName = _sceneName;
            replaySourceName = _replaySourceName;   
            clipLength = _clipLength;
            obs.SaveReplayBuffer();
            obs.ReplayBufferSaved += Obs_ReplayBufferSaved;
            
        }

        private void Obs_ReplayBufferSaved(object? sender, OBSWebsocketDotNet.Types.Events.ReplayBufferSavedEventArgs e)
        {
            string replayLoc = obs.GetLastReplayBufferReplay();
            UpdateOBSReplayClip(replayLoc);            
            int replaySourceID = obs.GetSceneItemId(sceneName, replaySourceName, 0);
            obs.SetSceneItemEnabled(sceneName, replaySourceID, true);
            Thread.Sleep(clipLength);
            obs.SetSceneItemEnabled(sceneName, replaySourceID, false);
            Thread.Sleep(1000);
            File.Delete(replayLoc);
            UpdateOBSReplayClip("");
        }

        private void UpdateOBSReplayClip(string replayLoc)
        {
            InputSettings inputSettings = obs.GetInputSettings(replaySourceName);
            LocalMedia replaySource = JsonConvert.DeserializeObject<LocalMedia>(inputSettings.Settings.ToString());
            replaySource.LocalFile = replayLoc;
            inputSettings.Settings = JObject.FromObject(replaySource);
            obs.SetInputSettings(inputSettings);
        }
    }
}
