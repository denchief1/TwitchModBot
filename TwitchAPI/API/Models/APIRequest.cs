using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Models;

namespace TwitchAPI.API.Models
{
    public class APIRequest
    {
        public string RequestType { get; set; }
        public string Url { get; set; }
        public object Content { get; set; }
    }
    
    
}
