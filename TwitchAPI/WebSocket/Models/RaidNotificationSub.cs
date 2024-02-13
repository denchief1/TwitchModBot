using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.WebSocket.Models
{
    public class RaidNotificationSub
    {
        public RaidNotificationSub() 
        {
            Condition = new Condition();
            Transport = new Transport();    
        }    

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("transport")]
        public Transport Transport { get; set; }
    }

    public class Condition
    {
        [JsonProperty("to_broadcaster_user_id")]
        public string ToBroadcasterUserId { get; set; }
    }

    public class Transport
    {
        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
    }
}
