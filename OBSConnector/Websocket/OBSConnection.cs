using Microsoft.Extensions.Hosting;
using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSConnector.Websocket
{
    public class OBSConnection
    //internal class OBSConnection : IHostedService
    {
        internal OBSWebsocket obs;
        private string connectionUrl;
        private string connectionPassword;
        private TaskCompletionSource<bool> disconnected = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> connected = new TaskCompletionSource<bool>();
        private bool obsConnected = false;
        public OBSConnection(string _connectionUrl, string _connecitonPassword, CancellationToken token) 
        { 
            connectionUrl = _connectionUrl;
            connectionPassword = _connecitonPassword;
            obs = new OBSWebsocket();
            StartAsync(token);
            while (obsConnected == false)
            {
                Thread.Sleep(10);
            }
            token.Register(() => StopAsync(token));
        }

        internal OBSWebsocket GetObsWS()
        {
            return obs;
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            obs.Disconnect();
            obs.Disconnected += Obs_Disconnected;
            return disconnected.Task;
        }

        private void Obs_Disconnected(object? sender, OBSWebsocketDotNet.Communication.ObsDisconnectionInfo e)
        {
            disconnected.SetResult(true);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            obs = new OBSWebsocket();
            obs.ConnectAsync(connectionUrl, connectionPassword);
            obs.Connected += Obs_Connected;
            return connected.Task;

        }

        private void Obs_Connected(object? sender, EventArgs e)
        {
            connected.SetResult(true);
            obsConnected = true;
        }
    }
}
