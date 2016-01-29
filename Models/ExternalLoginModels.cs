using Microsoft.AspNet.Identity;

namespace Canvass.Api.Models
{
    public class ExternalAccessDetails
    {
        public string user_id { get; set; }
        public string app_id { get; set; }
    }

    public class ExternalLoginDetails {
        public UserLoginInfo Login { get; set; }
        public string Email { get; set; }
    }

}