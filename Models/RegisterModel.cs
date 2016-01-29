using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Canvass.Api.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email too long")]
        [JsonProperty(PropertyName = "email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(100, ErrorMessage = "Password should be at least {2} characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [JsonProperty(PropertyName = "confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}