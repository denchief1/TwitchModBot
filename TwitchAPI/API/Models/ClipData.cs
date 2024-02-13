using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.API.Models
{
    public partial class ClipList
    {
        [JsonProperty("data")]
        public ClipData[] ClipData { get; set; }
    }

    public partial class ClipData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("edit_url")]
        public Uri EditUrl { get; set; }
    }
}
