using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using OBSWebsocketDotNet.Types.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OBSConnector.Websocket;
using Newtonsoft.Json.Linq;
using System.IO;
using TwitchAPI.API.Core;
using TwitchAPI.WebSocket;

namespace OBSConnector
{
    public class StartConnection
    {
        public static IHost host;
        public  StartConnection(string obsWsUrl, string obsWsPassword)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();

            builder.Services.AddSingleton(serviceProvider => new OBSConnection(obsWsUrl, obsWsPassword));


            IHost _host = builder.Build();
            _host.RunAsync();
            host = _host;   

        }


    }
}
