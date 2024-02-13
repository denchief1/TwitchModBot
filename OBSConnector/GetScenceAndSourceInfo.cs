using Microsoft.Extensions.DependencyInjection;
using OBSConnector.Websocket;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSConnector
{
    public class GetScenceAndSourceInfo
    {
        public readonly OBSWebsocket obs;

        public GetScenceAndSourceInfo()
        {
            obs = StartConnection.host.Services.GetRequiredService<OBSConnection>().GetObsWS();
        }

        public GetSceneListInfo GetScences()
        {
            return obs.GetSceneList();
        }
        public List<SceneItemDetails> GetSources(string scence)
        {
            return obs.GetSceneItemList(scence);
        }

    }
}
