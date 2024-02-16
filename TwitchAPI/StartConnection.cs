using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.API.Core;
using TwitchAPI.API.Models;
using TwitchAPI.Models;
using TwitchAPI.WebSocket;

namespace TwitchAPI
{
    public  class StartConnection : IDisposable
    {
        public static IHost host;
        public StartConnection(DataToken token, string streamer, ClientInfo clientInfo)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.Services.AddSingleton(serviceProvider => new APIService(token, serviceProvider.GetRequiredService<SendApiRequest>(), clientInfo));
            builder.Services.AddTransient<SendApiRequest>();
            //builder.Services.AddTransient(serviceProvider => new SendApiRequest(serviceProvider.GetRequiredService<APIService>()));
            builder.Services.AddHostedService(serviceProvider => new WssConnect(token.AuthToken, streamer, serviceProvider.GetRequiredService<APIService>()));


            IHost _host = builder.Build();
            _host.RunAsync();
            host = _host;
        }

        public void Dispose()
        {
            Task stop = host.StopAsync();
            stop.Wait();
            host.Dispose();
        }

        
    }
}
