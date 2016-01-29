using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Canvass.Api.Models
{
    public class AccessTokenResponse
    {
        public string userId { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
        public bool success { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public long expires_in { get; set; }
        public string token_type { get; set; }
        public long expires
        {
            get { return expires_in; }
            set { expires_in = expires; }
        }
    }
}
