//using Canvass.Api.Models;
//using Facebook;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Mvc;
//using System.Threading.Tasks;

//namespace Canvass.Api.Helpers
//{
//    public class FacebookHelper : OAuth2Helper
//    {
     
//        public static AppSettings AppSettings { get; set; }
//        public static FacebookHelper Instance(string clientId)
//        {
//            return new FacebookHelper(clientId);
//        }

//        private string _clientId;
//        private FacebookClient _client;

//        public FacebookHelper(string clientId)
//        {
//            _clientId = clientId;
//            _client = new FacebookClient();
//        }

//        private string CallbackUrl
//        {
//            get
//            {
//                return AppSettings.FacebookCallbackUrl;
//            }
//        }

//        private string ClientSecret
//        {
//            get
//            {
//                return AppSettings.FacebookClientSecret;
//            }
//        }
//        public async Task<ExternalLoginDetails> GetExternalLoginDetails(string authorizationCode)
//        {
//            dynamic result = await _client.GetTaskAsync("oauth/access_token", new
//                {
//                    client_id = _clientId,
//                    client_secret = ClientSecret,
//                    redirect_uri = CallbackUrl,
//                    code = authorizationCode
//                });
//            _client.AccessToken = result.access_token;
//            dynamic user = await _client.GetTaskAsync("me?fields=name,email");

//            var details = new ExternalLoginDetails
//            {
//                Email = user.email,
//                Login = new UserLoginInfo("google", user.id, user.Email)
//            };
//            return details;
//        }

//    }
//}
