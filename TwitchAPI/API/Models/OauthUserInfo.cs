using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.API.Models
{
    public class OauthUserInfo
    {
        [JsonProperty("aud")]
        public string clientId { get; set; }

        [JsonProperty("exp")]
        public long Exp { get; set; }

        [JsonProperty("iat")]
        public long Iat { get; set; }

        [JsonProperty("iss")]
        public Uri Iss { get; set; }

        [JsonProperty("sub")]
        public string UserId { get; set; }

        [JsonProperty("azp")]
        public string Azp { get; set; }

        [JsonProperty("preferred_username")]
        public string PreferredUsername { get; set; }
    }
}
