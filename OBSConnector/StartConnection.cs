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
    public class StartConnection : IDisposable
    {
        public static IHost host;
        CancellationTokenSource cts;
        public  StartConnection(string obsWsUrl, string obsWsPassword)
        {
            cts = new CancellationTokenSource();
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();

            builder.Services.AddSingleton(serviceProvider => new OBSConnection(obsWsUrl, obsWsPassword,cts.Token ));


            IHost _host = builder.Build();
            _host.RunAsync();
            host = _host;   

        }

        public void Dispose()
        {
            cts.Cancel();
            Task stop = host.StopAsync();
            stop.Wait();
            host.Dispose();
        }


    }
}
