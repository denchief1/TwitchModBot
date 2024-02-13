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
        private CancellationTokenSource CTS;

        //, APIService _apiSender
        public WssConnect(AuthToken _authToken,string _streamerLogin, APIService _apiSender)
        {
            streamerLogin = _streamerLogin;
            authToken = _authToken;
            apiSender = _apiSender;
        }

        
        private async Task ConnectAsync(string streamerLogin)
        {
            int keepAlive = 30;
            
            //Uri eventSub = new Uri($"ws://127.0.0.1:8000/ws?keepalive_timeout_seconds={keepAlive}");
            Uri eventSub = new Uri($"wss://eventsub.wss.twitch.tv/ws?keepalive_timeout_seconds={keepAlive}");
            if (WS != null)
            {
                if (WS.State == WebSocketState.Open) return;
                else WS.Dispose();
            }
            WS = new ClientWebSocket();
            if (CTS != null) CTS.Dispose();
            CTS = new CancellationTokenSource();
            await WS.ConnectAsync(eventSub, CTS.Token);
            await Task.Factory.StartNew(ReceiveLoop, CTS.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private async Task DisconnectAsync()
        {
            if (WS is null) return;
            if (WS.State == WebSocketState.Open)
            {
                CTS.CancelAfter(TimeSpan.FromSeconds(2));
                await WS.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
                await WS.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            WS.Dispose();
            CTS.Dispose();
        }

        private async Task ReceiveLoop()
        {
            var loopToken = CTS.Token;
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
                        receiveResult = await WS.ReceiveAsync(buffer, CTS.Token);
                        if (receiveResult.MessageType != WebSocketMessageType.Close)
                            outputStream.Write(buffer, 0, receiveResult.Count);
                    }
                    while (!receiveResult.EndOfMessage);

                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;
                    outputStream.Position = 0;
                    ResponseReceived(outputStream);
                }
            }
            catch (TaskCanceledException) { }
            finally
            {
                outputStream?.Dispose();
            }
        }

        private void ResponseReceived(Stream inputStream)
        {
            
            string wssResponse = ConvertStreamToString(inputStream);
            if (wssResponse.Contains("session_welcome"))
            {
                WelcomeMessage welcomeMessage = JsonConvert.DeserializeObject<WelcomeMessage>(wssResponse);
                RaidAlert alert = new RaidAlert();
                alert.SubscribeToRaidAlert(streamerLogin, welcomeMessage);
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
            Console.WriteLine($" WSS Message: {wssResponse}");



            inputStream.Dispose();
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
            return ConnectAsync(streamerLogin);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return DisconnectAsync();
        }
    }

    
}
