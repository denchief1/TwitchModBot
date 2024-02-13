using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.API.Core;
using TwitchAPI.API.Models;
using TwitchAPI.Models;

namespace TwitchAPI
{
    internal class ShoutoutEndpoint
    {

        private readonly APIService apiSender;

        public ShoutoutEndpoint()
        {
            apiSender = StartConnection.host.Services.GetRequiredService<APIService>();
        }

        /// <summary>
        /// Sends a built in twitch shoutout
        /// </summary>
        /// <param name="token">Authentication Token</param>
        /// <param name="broadcasterChannelId">Twitch User ID of the streamer's chat to shoutout in</param>
        /// <param name="targetBroadcasterId">Twitch User ID of the target of the shoutout</param>
        /// <param name="senderId">Twitch ID of the sender (has to be the auth token user)</param>
        internal Task<string> SendShoutOut(string broadcasterChannelId, string targetBroadcasterId)
        {
            string soUrl = $"https://api.twitch.tv/helix/chat/shoutouts?from_broadcaster_id={broadcasterChannelId}&to_broadcaster_id={targetBroadcasterId}&moderator_id={apiSender.authToken.UserID}";
            APIRequest request = new APIRequest();
            request.Url = soUrl;
            request.RequestType = "post";
            return apiSender.SendRequestAsync(request);
           
        }

    }
}

