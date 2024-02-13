using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.API.Core;
using TwitchAPI.API.Endpoints;
using TwitchAPI.API.Models;
using TwitchAPI.Models;
using TwitchAPI.WebSocket.Models;

namespace TwitchAPI.WebSocket
{
    internal class RaidAlert
    {

        private readonly APIService apiSender;

        public RaidAlert()
        {
            apiSender = StartConnection.host.Services.GetRequiredService<APIService>();
        }
        internal void SubscribeToRaidAlert(string broadcasterLogin,WelcomeMessage message)
        {
            string[] userNames = { broadcasterLogin};

            UsersEndpoint getUserInfo = new UsersEndpoint();
            Users users = getUserInfo.GetUserId(userNames);

            RaidNotificationSub sub = new RaidNotificationSub();
            sub.Type = "channel.raid";
            sub.Version = "1";
            sub.Condition.ToBroadcasterUserId = users.UserData[0].Id;
            sub.Transport.Method = "websocket";
            sub.Transport.SessionId = message.Payload.Session.SessionId.ToString();
            APIRequest request = new APIRequest();
            request.Content = sub;
            request.Url = "https://api.twitch.tv/helix/eventsub/subscriptions";
            //request.Url = "http://127.0.0.1:8000/eventsub/subscriptions";
            request.RequestType = "post";

            string result = apiSender.SendRequestAsync(request).Result;
        }

        internal void ShoutoutRaider(RaidNotificationAlert alert)
        {
            ShoutoutEndpoint shoutout = new ShoutoutEndpoint();
            var soResult = shoutout.SendShoutOut(alert.Payload.RaidInfo.ToBroadcasterUserId, alert.Payload.RaidInfo.FromBroadcasterUserId);
            ChatEndpoint chat = new ChatEndpoint();
            var chatResult = chat.SendChatMessageById (alert.Payload.RaidInfo.ToBroadcasterUserId, $@"!so @{alert.Payload.RaidInfo.FromBroadcasterUserName}");
            var test1 = soResult.Result;
            var test2 = chatResult.Result;
        }

    }

    
}
