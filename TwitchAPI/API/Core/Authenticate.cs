using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using TwitchAPI.API.Models;
using TwitchAPI.Models;

namespace TwitchAPI.API.Core
{
    public class Authenticate
    {
        private static readonly HttpClient client = new HttpClient();
        internal static readonly string clientId = ;
        private static readonly string clientSecret = ;
        private static readonly string redirect = "http://localhost:8080";
        private static HttpListener listener = new HttpListener();

        public static Tuple<string, string> GenerateAuthUrl()
        {
            string shoutoutScope = "moderator:manage:shoutouts";
            string markerScope = "channel:manage:broadcast";
            string clipScope = "clips:edit";
            string chatScope = "user:write:chat";
            string readScope = "user:read:chat";
            string moderateScope = "channel:moderate";
            string scope = $"{shoutoutScope}+{markerScope}+{clipScope}+{chatScope}+{moderateScope}+{readScope}";
            string responseType = "code";
            string state = RandomString(32);

            string url = $"https://id.twitch.tv/oauth2/authorize?response_type={responseType}&client_id={clientId}&redirect_uri={redirect}&state={state}&scope={scope}&force_verify=true";
            return new Tuple<string, string>(url, state);
        }
        public static Tuple<AuthToken,ClientInfo> StartOauth(string state)
        {

            string rawResponse = Listener(redirect);
            string[] respones = rawResponse.Split('&');
            List<Tuple<string, string>> responseValues = new List<Tuple<string, string>>();
            foreach (string response in respones)
            {
                string[] values = response.Split('=');
                Tuple<string, string> tuple = new Tuple<string, string>(values[0], values[1]);
                responseValues.Add(tuple);
            }
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.ClientId = clientId;
            clientInfo.ClientSecret = clientSecret;
            return new Tuple<AuthToken, ClientInfo>(GetToken(responseValues, state), clientInfo);
        }

        private static AuthToken GetToken(List<Tuple<string, string>> responseValues, string state)
        {

            string code = string.Empty;
            string scope = string.Empty;
            string returnState = string.Empty;

            foreach (Tuple<string, string> responseValue in responseValues)
            {
                string key = responseValue.Item1;
                string value = responseValue.Item2;

                switch (key)
                {
                    case "/?code":
                        code = value;
                        break;
                    case "scope":
                        scope = value;
                        break;
                    case "state":
                        returnState = value;
                        break;
                    default:
                        break;
                }
            }

            var values = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirect }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = client.PostAsync("https://id.twitch.tv/oauth2/token", content).Result;

            string responseString = response.Content.ReadAsStringAsync().Result;

            AuthToken token = JsonConvert.DeserializeObject<AuthToken>(responseString);
            string url = "https://id.twitch.tv/oauth2/userinfo";
            responseString = SendGetRequest(url,token);
            OauthUserInfo userInfo = JsonConvert.DeserializeObject<OauthUserInfo>(responseString);
            token.UserID = userInfo.UserId;
            return token;

        }

        private static string SendGetRequest(string url, AuthToken token)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                requestMessage.Headers.Add("Client-Id", Authenticate.clientId);
                HttpResponseMessage response = client.SendAsync(requestMessage).Result;
                return response.Content.ReadAsStringAsync().Result;
            }

        }

        private static string Listener(string prefix)
        {
            if (!HttpListener.IsSupported)
            {
                return "Windows XP SP2 or Server 2003 is required to use the HttpListener class.";

            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefix == null || prefix.Length == 0)
                throw new ArgumentException("prefixes");
            prefix = prefix + "/";
            // Add the prefixes.
            listener.Prefixes.Add(prefix);
            listener.Start();
            // Note: The GetContext method blocks while waiting for a request.
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // You must close the output stream
            listener.Stop();
            return request.RawUrl;
        }


        private static string RandomString(int length)
        {
            Random random = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

    }
}