//using Canvass.Api.Models;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Auth.OAuth2.Flows;
//using Google.Apis.Auth.OAuth2.Responses;
//using Google.Apis.Oauth2.v2;
//using Google.Apis.Oauth2.v2.Data;
//using Google.Apis.Services;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Mvc;
//using System.Threading;
//using System.Threading.Tasks;


//namespace Canvass.Api.Helpers
//{
//    public interface OAuth2Helper
//    {
//        Task<ExternalLoginDetails> GetExternalLoginDetails(string authorizationCode);
//    }
//    public class GoogleHelper : OAuth2Helper
//    {
//        public static AppSettings AppSettings { get; set; }
//        public static GoogleHelper Instance (string clientId)
//        {
//            return new GoogleHelper(clientId);
//        }

//        private string _clientId;
//        private GoogleAuthorizationCodeFlow _flow;
//        public GoogleHelper(string clientId)
//        {
//            var initializer = new GoogleAuthorizationCodeFlow.Initializer();
//            initializer.ClientSecrets = new ClientSecrets { ClientId = clientId, ClientSecret = ClientSecret };
//            _flow = new GoogleAuthorizationCodeFlow(initializer);
//        }

//        private string CallbackUrl
//        {
//            get
//            {
//                return AppSettings.GoogleCallbackUrl;
//            }
//        }

//        private string ClientSecret
//        {
//            get
//            {
//                return AppSettings.GoogleClientSecret;
//            }
//        }
//        public async Task<ExternalLoginDetails> GetExternalLoginDetails(string authorizationCode)
//        {
//            var token = await ExchangeAuthorizationCodeForToken(authorizationCode);
//            var credentials = new UserCredential(_flow, _clientId, token);
//            var oauthService = new Oauth2Service(new BaseClientService.Initializer { HttpClientInitializer = credentials });
//            Userinfoplus info = await oauthService.Userinfo.Get().ExecuteAsync();
//            var details = new ExternalLoginDetails
//            {
//                Email = info.Email,
//                Login = new UserLoginInfo("google", info.Id, info.Email)
//            };
//            return details;
//        }

//        private async Task<TokenResponse> ExchangeAuthorizationCodeForToken(string authorizationCode)
//        {
//            var token = await _flow.ExchangeCodeForTokenAsync(_clientId, authorizationCode, CallbackUrl,
//                new CancellationToken());
//            return token;
//        }

//    }
//}