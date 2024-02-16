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

        /// <summary>
        /// Send a chat message with the broadcaster's twitch login name
        /// </summary>
        /// <param name="broadcasterLogin">Twitch Login Name</param>
        /// <param name="chatMessage">Chat message to send</param>
        /// <returns>If the chat message was sent successfully</returns>
        internal bool SendChatMessage(string broadcasterLogin, string chatMessage)
        {
            string url = "https://api.twitch.tv/helix/chat/messages";

            UsersEndpoint getUserInfo = new UsersEndpoint();
            string[] userNames = { broadcasterLogin };
            Tuple<bool,Users> usersResponse = getUserInfo.GetUserId(userNames);
            if (usersResponse.Item1 == false)
            {
                return false;
            }
            else
            {
                Users users = usersResponse.Item2;
                ChatData chatData = new ChatData();
                chatData.SenderId = apiSender.dataToken.UserID;
                chatData.BroadcasterId = users.UserData.Where(x => x.Login == broadcasterLogin).First().Id;
                chatData.Message = chatMessage;

                APIRequest request = new APIRequest();
                request.Url = url;
                request.Content = chatData;
                request.RequestType = "post";

                string result = apiSender.SendRequestAsync(request).Result;
                if (String.IsNullOrEmpty(result))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            
            

        }

        /// <summary>
        /// Send a chat message with the broadcaster's Twitch ID
        /// </summary>
        /// <param name="broadcasterID">The twitch ID of the user</param>
        /// <param name="chatMessage">The chat message to send</param>
        /// <returns>If the chat message was sent successfully</returns>
        internal bool SendChatMessageById(string broadcasterID, string chatMessage)
        {
            string url = "https://api.twitch.tv/helix/chat/messages";


            ChatData chatData = new ChatData();
            chatData.SenderId = apiSender.dataToken.UserID;
            chatData.BroadcasterId = broadcasterID;
            chatData.Message = chatMessage;

            APIRequest request = new APIRequest();
            request.Url = url;
            request.Content = chatData;
            request.RequestType = "post";

            string result = apiSender.SendRequestAsync(request).Result;
            if (String.IsNullOrEmpty(result))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
