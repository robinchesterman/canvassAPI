using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Canvass.Api.Models
{
    public class LocalAccessTokenRequest
    {
        [Required]
        public string ClientId { get; set; }
        [JsonProperty(PropertyName = "code")]
        [Required]
        public string AuthorizationCode { get; set; }
        [JsonProperty(PropertyName = "redirectUri")]
        public string RedirectUri { get; set; }
    
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}