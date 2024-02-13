using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.WebSocket.Models
{
    public partial class RaidNotificationAlert
    {
        [JsonProperty("metadata")]
        public RaidAlertMetadata Metadata { get; set; }

        [JsonProperty("payload")]
        public RaidAlertPayload Payload { get; set; }
    }

    public class RaidAlertMetadata
    {
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("message_type")]
        public string MessageType { get; set; }

        [JsonProperty("message_timestamp")]
        public DateTimeOffset MessageTimestamp { get; set; }

        [JsonProperty("subscription_type")]
        public string SubscriptionType { get; set; }

        [JsonProperty("subscription_version")]
        public string SubscriptionVersion { get; set; }
    }

    public class RaidAlertPayload
    {
        [JsonProperty("subscription")]
        public Subscription Subscription { get; set; }

        [JsonProperty("event")]
        public RaidEvent RaidInfo { get; set; }
    }

    public class RaidEvent
    {
        [JsonProperty("from_broadcaster_user_id")]
        public string FromBroadcasterUserId { get; set; }

        [JsonProperty("from_broadcaster_user_login")]
        public string FromBroadcasterUserLogin { get; set; }

        [JsonProperty("from_broadcaster_user_name")]
        public string FromBroadcasterUserName { get; set; }

        [JsonProperty("to_broadcaster_user_id")]
        public string ToBroadcasterUserId { get; set; }

        [JsonProperty("to_broadcaster_user_login")]
        public string ToBroadcasterUserLogin { get; set; }

        [JsonProperty("to_broadcaster_user_name")]
        public string ToBroadcasterUserName { get; set; }

        [JsonProperty("viewers")]
        public long Viewers { get; set; }
    }

    public class Subscription
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }

        [JsonProperty("transport")]
        public Transport Transport { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("cost")]
        public long Cost { get; set; }
    }



}

