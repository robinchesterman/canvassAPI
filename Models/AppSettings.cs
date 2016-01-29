using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Canvass.Api.Models
{
    public class AzureSettings
    {
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
        public string CommunicationsQueueName { get; set; }
    }
    public class OAuthSettings
    {
        public OAuthSetting Google { get; set; }
        public OAuthSetting Facebook { get; set; }
        public OAuthSetting Twitter { get; set; }
        public OAuthSetting LinkedIn { get; set; }
    }

    public class OAuthSetting
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackUrl { get; set; }
    }
    public class AppSettings
    {
        public OAuthSettings OAuth { get; set; }
        public AzureSettings Azure { get; set; }
    }
}
