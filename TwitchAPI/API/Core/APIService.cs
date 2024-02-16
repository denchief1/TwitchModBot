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
        internal DataToken dataToken;
        private readonly SendApiRequest apiSender;
        private readonly ClientInfo clientInfo;
        private bool requestLoop = false;

        public APIService(DataToken _dataToken, SendApiRequest _apiSender, ClientInfo _clientInfo)
        {

            dataToken = _dataToken;
            apiSender = _apiSender;
            clientInfo = _clientInfo;
        }


        internal Task<string> SendRequestAsync(APIRequest request)
        {
            HttpResponseMessage response = apiSender.SendRequest(request, dataToken.AuthToken).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (requestLoop == false)
                {
                    RefreshToken();
                    requestLoop = true;
                    return SendRequestAsync(request);
                }
                else
                {
                    requestLoop = false;
                    return Task.Run(() => string.Empty);
                }
                
            }
            if ((response.StatusCode == System.Net.HttpStatusCode.OK))
            {
                requestLoop = false;
                return response.Content.ReadAsStringAsync();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return Task.Run(() => "true");
            }
            else
            {
                requestLoop = false;
                return Task.Run(() => string.Empty);
            }


        }

        internal void RefreshToken()
        {
            var values = new Dictionary<string, string>
            {
                { "client_id", clientInfo.ClientId },
                { "client_secret", clientInfo.ClientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", dataToken.AuthToken.RefreshToken }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.PostAsync("https://id.twitch.tv/oauth2/token", content).Result;

            string responseString = response.Content.ReadAsStringAsync().Result;

            RefreshToken refreshToken = JsonConvert.DeserializeObject<RefreshToken>(responseString);
            dataToken.AuthToken.RefreshToken = refreshToken.NewRefreshToken;
            dataToken.AuthToken.AccessToken = refreshToken.AccessToken;
        }

    }
}
