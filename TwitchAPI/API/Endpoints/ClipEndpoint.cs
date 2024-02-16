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

        /// <summary>
        /// Create clip with the broadcaster's Twitch Id
        /// </summary>
        /// <param name="broadcasterId">Broadcaster's Twtich Id</param>
        /// <returns>A tuple of if the clip was created successfully and the edit link</returns>
        public Tuple<bool, string> CreateClipById(string broadcasterId)
        {
            string url = $"https://api.twitch.tv/helix/clips?broadcaster_id={broadcasterId}";
            APIRequest request = new APIRequest();
            request.Url = url;
            request.RequestType = "post";

            string result = apiSender.SendRequestAsync(request).Result;
            if (string.IsNullOrEmpty(result))
            {
                return new Tuple<bool, string>(false, "");
            }
            else
            {
                return new Tuple<bool, string>(true, result);
            }
            

        }

        /// <summary>
        /// Create clip with the broadcaster's Twitch login
        /// </summary>
        /// <param name="broadcasterLogin">Broadcaster's Twitch Login</param>
        /// <returns>A tuple of if the clip was created successfully and the edit link</returns>
        public Tuple<bool,string> CreateClipByLogin(string broadcasterLogin)
        {
            UsersEndpoint getUserInfo = new UsersEndpoint();
            string[] userNames = { broadcasterLogin };
            Tuple<bool, Users> usersResponse = getUserInfo.GetUserId(userNames);
            if (usersResponse.Item1 == false)
            {
                return new Tuple<bool, string>(false, "");
            }
            else
            {
                Users users = usersResponse.Item2;


                string url = $"https://api.twitch.tv/helix/clips?broadcaster_id={users.UserData.Where(x => x.Login == broadcasterLogin).First().Id}";
                APIRequest request = new APIRequest();
                request.Url = url;
                request.RequestType = "post";
                string result = apiSender.SendRequestAsync(request).Result;
                if (string.IsNullOrEmpty(result))
                {
                    return new Tuple<bool, string>(false, "");
                }
                else
                {
                    return new Tuple<bool, string>(true, result);
                }
            }
        }
    }
}
