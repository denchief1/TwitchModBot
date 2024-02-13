using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
    public class ClipEndpoint
    {
        private readonly APIService apiSender;

        public ClipEndpoint()
        {
            apiSender = StartConnection.host.Services.GetRequiredService<APIService>();
        }

        public Task<string> CreateClipById(string broadcasterId)
        {
            string url = $"https://api.twitch.tv/helix/clips?broadcaster_id={broadcasterId}";
            APIRequest request = new APIRequest();
            request.Url = url;
            request.RequestType = "post";
            return apiSender.SendRequestAsync(request);
        }

        public Task<string> CreateClipByLogin(string broadcasterLogin)
        {
            UsersEndpoint getUserInfo = new UsersEndpoint();
            string[] userNames = { broadcasterLogin };
            Users users = getUserInfo.GetUserId(userNames);

            string url = $"https://api.twitch.tv/helix/clips?broadcaster_id={users.UserData.Where(x => x.Login == broadcasterLogin).First().Id}";
            APIRequest request = new APIRequest();
            request.Url = url;
            request.RequestType = "post";
            return apiSender.SendRequestAsync(request);
        }
    }
}
