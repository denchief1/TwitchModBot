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


    }

    
}
