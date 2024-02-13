using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.API.Core;
using TwitchAPI.API.Endpoints;
using TwitchAPI.Models;

namespace TwitchAPI
{
    public class SendTestMessage
    {
        

        public static void SendHelloWorld(string broadcasterLogin)
        {

            ChatEndpoint chatEndpoint = new ChatEndpoint();
            chatEndpoint.SendChatMessage(broadcasterLogin,"This is the bot");

        }

        public static void SendTestShoutout(string streamerLogin, string soTarget)
        {
            string[] userNames = { streamerLogin, soTarget };

            UsersEndpoint getUserInfo = new UsersEndpoint();
            Users users = getUserInfo.GetUserId(userNames);
            string streamerId = users.UserData.Where(x => x.Login == streamerLogin).First().Id;
            string soTargetId = users.UserData.Where(x => x.Login == soTarget).First().Id;

            ShoutoutEndpoint shoutout = new ShoutoutEndpoint();
            shoutout.SendShoutOut(streamerId, soTargetId);
        }
    }

    
}
