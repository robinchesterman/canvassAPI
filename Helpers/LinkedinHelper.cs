//using Canvass.Api.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Mvc;
//using RestSharp;
//using System.Threading.Tasks;
//using Newtonsoft.Json;


//namespace Canvass.Api.Helpers
//{
   

//    public class LinkedinHelper : OAuth2Helper 
//    {
      
//        public static AppSettings AppSettings { get; set; }

//        private string _clientId;
//        private RestClient _restClient;
//        private string OAuthApi = "https://www.linkedin.com/uas/oauth2/";
//        private string ResourceApi = "https://api.linkedin.com/v1/";
//        public static LinkedinHelper Instance(string clientId)
//        {
//            return new LinkedinHelper(clientId);
//        }

//        public LinkedinHelper(string clientId)
//        {
//            _clientId = clientId;
//            _restClient = new RestClient(ResourceApi);
//        }

//        private string ClientId
//        {
//            get
//            {
//                return AppSettings.LinkedInClientId;
//            }
//        }

//        private string ClientSecret
//        {
//            get
//            {
//                return AppSettings.LinkedInClientSecret;
//            }
//        }

//        private string CallbackUrl
//        {
//            get
//            {
//                return AppSettings.LinkedInCallbackUrl;
//            }
//        }

//        public async Task<ExternalLoginDetails> GetExternalLoginDetails(string authorizationCode)
//        {
//            AccessTokenResponse token = await ExchangeAuthorizationCodeForToken(authorizationCode);
//            LinkedInProfileResponse details = await GetUserDetails(token);

//            return new ExternalLoginDetails()
//            {
//                Email = details.emailAddress,
//                Login = new UserLoginInfo("linkedin", details.id, details.emailAddress)
//            };
//        }

//        private async Task<LinkedInProfileResponse> GetUserDetails(AccessTokenResponse accessToken)
//        {
//            var client = new RestClient(ResourceApi);
            
//            var request = new RestRequest("people/~:(id,email-address)?format=json", Method.GET);
//            request.AddHeader("Authorization", "Bearer " + accessToken.access_token);

//            var taskCompletionSource = new TaskCompletionSource<LinkedInProfileResponse>();
//            var result = _restClient.ExecuteAsync(request, response =>
//            {
//                var tokenDetails =  JsonConvert.DeserializeObject<LinkedInProfileResponse>(response.Content);
//                taskCompletionSource.SetResult(tokenDetails);
//            });
//            return await taskCompletionSource.Task;

//        }

//        public class LinkedInProfileResponse
//        {
//            public string emailAddress { get; set; }
//            public string id { get; set; }
//        }

//        private async Task<AccessTokenResponse> ExchangeAuthorizationCodeForToken(string authorizationCode)
//        {
//            var client = new RestClient(OAuthApi);
//            var request = new RestRequest("accessToken", Method.POST);
//            request.AddParameter("grant_type", "authorization_code");
//            request.AddParameter("code", authorizationCode);
//            request.AddParameter("redirect_uri", CallbackUrl);
//            request.AddParameter("client_id", _clientId);
//            request.AddParameter("client_secret", ClientSecret);
//            request.AddHeader("ContentType", "application/x-www-form-urlencoded");
          
//            var taskCompletionSource = new TaskCompletionSource<AccessTokenResponse>();
//            var result = client.ExecuteAsync(request, response =>
//            {
//                var tokenDetails = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);
//                taskCompletionSource.SetResult(tokenDetails);
//            });
//            return await taskCompletionSource.Task;
           
//        }

//    }
//}