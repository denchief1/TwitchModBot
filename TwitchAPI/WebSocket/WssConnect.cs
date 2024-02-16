using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Newtonsoft.Json;
using TwitchAPI.WebSocket.Models;
using TwitchAPI.API.Core;
using TwitchAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading;

namespace TwitchAPI.WebSocket
{
    public class WssConnect : IHostedService
    {
        internal enum TwitchWebSocketCloseStatus
        {
            Empty = 1005,
            EndpointUnavailable = 1001,
            InternalServerError = 4000,
            InvalidMessageType = 1003,
            InvalidPayloadData = 1007,
            MandatoryExtension = 1010,
            MessageTooBig = 1009,
            NormalClosure = 1000,
            PolicyViolation = 1008,
            ProtocolError = 1002,
            ClientSentInboundTraffic = 4001,
            ClientFailedPingPong = 4002,
            ConnectionUnused = 4003,
            ReconnectGraceTimeExpired = 4004,
            NetworkTimeout = 4005,
            NetworkError = 4006,
            InvalidReconnect = 4007
        }
        private AuthToken authToken;

        private readonly APIService apiSender;
        private readonly string streamerLogin;
        public int ReceiveBufferSize { get; set; } = 8192;
        private ClientWebSocket WS;
        private bool webSocketConnected = false;
        int keepAlive = 30;
        public event EventHandler WssDiconnected;

        //, APIService _apiSender
        public WssConnect(AuthToken _authToken,string _streamerLogin, APIService _apiSender)
        {
            streamerLogin = _streamerLogin;
            authToken = _authToken;
            apiSender = _apiSender;
        }

        
        private async Task ConnectAsync(string streamerLogin, CancellationToken cancellationToken)
        {
            
            
            //Uri eventSub = new Uri($"ws://127.0.0.1:8000/ws?keepalive_timeout_seconds={keepAlive}");
            Uri eventSub = new Uri($"wss://eventsub.wss.twitch.tv/ws?keepalive_timeout_seconds={keepAlive}");
            if (WS != null)
            {
                if (WS.State == WebSocketState.Open) return;
                else WS.Dispose();
            }
            WS = new ClientWebSocket();
            await WS.ConnectAsync(eventSub, cancellationToken);
            await Task.Factory.StartNew(() => ReceiveLoop(cancellationToken),
                       cancellationToken,
                       TaskCreationOptions.LongRunning,
                       TaskScheduler.Default);
            await Task.Factory.StartNew(() => CheckForDisconnect(cancellationToken),
                       cancellationToken,
                       TaskCreationOptions.LongRunning,
                       TaskScheduler.Default);
        }

        

        private async Task DisconnectAsync(CancellationToken cancellationToken)
        {
            if (WS is null) return;
            if (WS.State == WebSocketState.Open)
            {
                await WS.CloseOutputAsync(WebSocketCloseStatus.Empty, "", cancellationToken);
                await WS.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cancellationToken);
            }
            WS.Dispose();
        }

        private async Task CheckForDisconnect(CancellationToken cancellationToken)
        {
            var loopToken = cancellationToken;
            try
            {
                while (!loopToken.IsCancellationRequested)
                {
                    do
                    {
                        webSocketConnected = false;
                        Thread.Sleep(keepAlive * 1000 + 5);

                    } while (webSocketConnected == true);
                    if (webSocketConnected == false)
                    {
                        WssDiconnected?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            catch (TaskCanceledException) { }

        }

        private async Task ReceiveLoop(CancellationToken cancellationToken)
        {
            var loopToken = cancellationToken;
            MemoryStream outputStream = null;
            WebSocketReceiveResult receiveResult = null;
            var buffer = new byte[ReceiveBufferSize];
            try
            {
                while (!loopToken.IsCancellationRequested)
                {
                    outputStream = new MemoryStream(ReceiveBufferSize);
                    do
                    {
                        receiveResult = await WS.ReceiveAsync(buffer, cancellationToken);
                        if (receiveResult.MessageType != WebSocketMessageType.Close)
                            outputStream.Write(buffer, 0, receiveResult.Count);
                    }
                    while (!receiveResult.EndOfMessage);

                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;
                    outputStream.Position = 0;
                    ResponseReceived(outputStream,cancellationToken);
                }
            }
            catch (TaskCanceledException) { }
            finally
            {
                outputStream?.Dispose();
            }
        }

        private void ResponseReceived(Stream inputStream, CancellationToken cancellationToken)
        {
            
            string wssResponse = ConvertStreamToString(inputStream);
            if (wssResponse.Contains("session_welcome"))
            {
                WebsocketMessage welcomeMessage = JsonConvert.DeserializeObject<WebsocketMessage>(wssResponse);
                RaidAlert alert = new RaidAlert();
                alert.SubscribeToRaidAlert(streamerLogin, welcomeMessage);
                webSocketConnected = true;
            }
            else if (wssResponse.Contains("from_broadcaster_user_name"))
            {
                try
                {
                    Console.WriteLine($"raid alert: {wssResponse}");
                    RaidNotificationAlert raidNotification = JsonConvert.DeserializeObject<RaidNotificationAlert>(wssResponse);
                    RaidAlert alert = new RaidAlert();
                    alert.ShoutoutRaider(raidNotification);
                }
                catch (Exception)
                {

                    throw;
                }
                
                
            }
            else if (wssResponse.Contains("reconnect_url"))
            {
                Console.WriteLine($" WSS Message: {wssResponse}");
                WebsocketMessage reconnectMessage = JsonConvert.DeserializeObject<WebsocketMessage>(wssResponse);
                ReconnctWssAsync(reconnectMessage.Payload.Session.ReconnectUrl.ToString(), cancellationToken);
            }
            else if(wssResponse.Contains("session_keepalive"))
            {
                webSocketConnected = true;
            }
            Console.WriteLine($" WSS Message: {wssResponse}");



            inputStream.Dispose();
        }
        

        private async void ReconnctWssAsync(string reconnectUrl, CancellationToken cancellationToken)
        {
            
            DisconnectAsync(cancellationToken).Wait();
            Uri eventSub = new Uri(reconnectUrl);
            if (WS != null)
            {
                if (WS.State == WebSocketState.Open) return;
                else WS.Dispose();
            }
            WS = new ClientWebSocket();
            await WS.ConnectAsync(eventSub, cancellationToken);
            await Task.Factory.StartNew(() => ReceiveLoop(cancellationToken),
                       cancellationToken,
                       TaskCreationOptions.LongRunning,
                       TaskScheduler.Default);
            await Task.Factory.StartNew(() => CheckForDisconnect(cancellationToken),
                       cancellationToken,
                       TaskCreationOptions.LongRunning,
                       TaskScheduler.Default);


        }

        private string ConvertStreamToString(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return ConnectAsync(streamerLogin, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return DisconnectAsync(cancellationToken);
        }
    }

    
}
