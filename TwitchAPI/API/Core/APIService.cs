using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwitchAPI.API.Models;
using TwitchAPI.Models;

namespace TwitchAPI.API.Core
{
    public class APIService 
    {
        internal AuthToken authToken;
        private readonly SendApiRequest apiSender;
        private readonly ClientInfo clientInfo;

        public APIService(AuthToken _authToken, SendApiRequest _apiSender, ClientInfo _clientInfo)
        {

            authToken = _authToken;
            apiSender = _apiSender;
            clientInfo = _clientInfo;
        }


        internal Task<string> SendRequestAsync(APIRequest request)
        {
            HttpResponseMessage response = apiSender.SendRequest(request, authToken).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                RefreshToken();
                return SendRequestAsync(request);
            }
            return response.Content.ReadAsStringAsync();


        }

        internal void RefreshToken()
        {
            var values = new Dictionary<string, string>
            {
                { "client_id", clientInfo.ClientId },
                { "client_secret", clientInfo.ClientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", authToken.RefreshToken }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.PostAsync("https://id.twitch.tv/oauth2/token", content).Result;

            string responseString = response.Content.ReadAsStringAsync().Result;

            RefreshToken refreshToken = JsonConvert.DeserializeObject<RefreshToken>(responseString);
            authToken.RefreshToken = refreshToken.NewRefreshToken;
            authToken.AccessToken = refreshToken.AccessToken;
        }

    }
}
