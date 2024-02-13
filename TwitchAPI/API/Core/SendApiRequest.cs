using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;
using TwitchAPI.Models;
using TwitchAPI.API.Core;
using TwitchAPI.API.Models;
using Microsoft.Extensions.DependencyInjection;

namespace TwitchAPI.API.Core
{
    public class SendApiRequest
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<HttpResponseMessage> SendRequest(APIRequest request, AuthToken token) 
        {
            if (request.RequestType == "get")
            {
                return SendGetRequest(request.Url, token);
            }
            else //post
            {
                if (request.Content != null)
                {
                    return SendPostRequest(request.Url,token, request.Content);
                }
                else
                {
                    return SendPostRequest(request.Url, token);
                }
            }

        }

        private HttpResponseMessage SendGetRequest(string url, AuthToken token)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                requestMessage.Headers.Add("Client-Id", Authenticate.clientId);
                return client.SendAsync(requestMessage).Result;
                
            }
            
        }

        private HttpResponseMessage SendPostRequest(string url, AuthToken token)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                //requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //Add("Content-Type", "application/json");
                requestMessage.Headers.Add("Client-Id", Authenticate.clientId);
                return client.Send(requestMessage);

            }
        }

        internal HttpResponseMessage SendPostRequest(string url, AuthToken token,object content)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                //requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //Add("Content-Type", "application/json");
                requestMessage.Headers.Add("Client-Id",Authenticate.clientId);
                string json = JsonConvert.SerializeObject(content);
                requestMessage.Content = new StringContent(json,Encoding.UTF8, "application/json");
                return client.Send(requestMessage);
            }
        }


        
    }
}
