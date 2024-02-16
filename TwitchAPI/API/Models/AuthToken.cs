using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Models
{
    public class AuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string[] Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        
    }

    public class DataToken
    {
        public AuthToken AuthToken { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserID { get; set; }
    }
}
