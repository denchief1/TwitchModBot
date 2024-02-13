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

namespace TwitchAPI.API.Endpoints
{
    internal class ChatEndpoint
    {
        //private static readonly SendApiRequest apiRequest = new SendApiRequest();

        private readonly APIService apiSender;

        public ChatEndpoint()
        {
            apiSender = StartConnection.host.Services.GetRequiredService<APIService>();
        }

        internal void SendChatMessage(string broadcasterLogin, string chatMessage)
        {
            string url = "https://api.twitch.tv/helix/chat/messages";

            UsersEndpoint getUserInfo = new UsersEndpoint();
            string[] userNames = { broadcasterLogin };
            Users users = getUserInfo.GetUserId(userNames);


            ChatData chatData = new ChatData();
            chatData.SenderId = apiSender.authToken.UserID;
            chatData.BroadcasterId = users.UserData.Where(x => x.Login == broadcasterLogin).First().Id;
            chatData.Message = chatMessage;

            APIRequest request = new APIRequest();
            request.Url = url;
            request.Content = chatData;
            request.RequestType = "post";

            var test = apiSender.SendRequestAsync(request).Result;
            //var test = apiRequest.SendPostRequest(url, token, chatData);

        }


        internal Task<string> SendChatMessageById(string broadcasterID, string chatMessage)
        {
            string url = "https://api.twitch.tv/helix/chat/messages";


            ChatData chatData = new ChatData();
            chatData.SenderId = apiSender.authToken.UserID;
            chatData.BroadcasterId = broadcasterID;
            chatData.Message = chatMessage;

            APIRequest request = new APIRequest();
            request.Url = url;
            request.Content = chatData;
            request.RequestType = "post";

            return apiSender.SendRequestAsync(request);
            //Console.WriteLine(test);
            //var test = apiRequest.SendPostRequest(url, token, chatData);

        }
    }
}
