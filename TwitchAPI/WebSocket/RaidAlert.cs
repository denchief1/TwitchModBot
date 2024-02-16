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
        internal bool SubscribeToRaidAlert(string broadcasterLogin,WebsocketMessage message)
        {
            string[] userNames = { broadcasterLogin};

            UsersEndpoint getUserInfo = new UsersEndpoint();
            Tuple<bool, Users> usersResponse = getUserInfo.GetUserId(userNames);
            if (usersResponse.Item1 == false)
            {
                return false;
            }
            else
            {
                Users users = usersResponse.Item2;

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
                if(string.IsNullOrEmpty(result))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        internal bool ShoutoutRaider(RaidNotificationAlert alert)
        {
            ShoutoutEndpoint shoutout = new ShoutoutEndpoint();
            bool soResult = shoutout.SendShoutOut(alert.Payload.RaidInfo.ToBroadcasterUserId, alert.Payload.RaidInfo.FromBroadcasterUserId);
            ChatEndpoint chat = new ChatEndpoint();
            bool chatResult = chat.SendChatMessageById (alert.Payload.RaidInfo.ToBroadcasterUserId, $@"!so @{alert.Payload.RaidInfo.FromBroadcasterUserName}");

            if (soResult == true && chatResult == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }

    
}
