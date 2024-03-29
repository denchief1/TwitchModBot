﻿using Microsoft.Extensions.DependencyInjection;
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
    public class UsersEndpoint
    {

        private readonly APIService apiSender;

        public UsersEndpoint()
        {
            apiSender = StartConnection.host.Services.GetRequiredService<APIService>();
        }

        /// <summary>
        /// Get user Ids from their usernames
        /// </summary>
        /// <param name="userNames">an array of usernames</param>
        /// <returns>If it was successful and the user ids</returns>
        internal Tuple<bool,Users> GetUserId (string[] userNames)
        {
            string url = "https://api.twitch.tv/helix/users?";

            if (userNames != null && userNames.Length == 1)
            {
                url = url + $"login={userNames[0]}";
            }
            else if (userNames != null && userNames.Length > 1)
            {
                url = url + $"login={userNames[0]}";
                for (int i = 1; i < userNames.Length; i++) 
                {
                    url = url + $"&login={userNames[i]}";
                }
            }

            APIRequest request = new APIRequest();
            request.Url = url;
            request.RequestType = "get";


            string result = apiSender.SendRequestAsync(request).Result;
            if (string.IsNullOrEmpty(result))
            {
                Users users = JsonConvert.DeserializeObject<Users>(result);
                return new Tuple<bool, Users>(false, users);
            }
            else
            {
                Users users =  JsonConvert.DeserializeObject<Users>(result);
                return new Tuple<bool, Users>(true, users);
            }

            
            
        }

    }
}
