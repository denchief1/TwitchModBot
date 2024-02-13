using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.WebSocket.Models
{
    public class WelcomeMessage
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("message_id")]
        public Guid MessageId { get; set; }

        [JsonProperty("message_type")]
        public string MessageType { get; set; }

        [JsonProperty("message_timestamp")]
        public DateTimeOffset MessageTimestamp { get; set; }
    }

    public partial class Payload
    {
        [JsonProperty("session")]
        public Session Session { get; set; }
    }

    public class Session
    {
        [JsonProperty("id")]
        public string SessionId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("connected_at")]
        public DateTimeOffset ConnectedAt { get; set; }

        [JsonProperty("keepalive_timeout_seconds")]
        public long KeepaliveTimeoutSeconds { get; set; }

        [JsonProperty("reconnect_url")]
        public object ReconnectUrl { get; set; }
    }
}
