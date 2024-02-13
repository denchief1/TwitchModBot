using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSConnector.Models
{
    public partial class LocalMedia
    {
        [JsonProperty("close_when_inactive")]
        public bool CloseWhenInactive { get; set; }

        [JsonProperty("is_local_file")]
        public bool IsLocalFile { get; set; }

        [JsonProperty("local_file")]
        public string LocalFile { get; set; }
    }
}
