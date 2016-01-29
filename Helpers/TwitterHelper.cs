//using Canvass.Api.Models;
//using Microsoft.AspNet.Mvc;
//using Newtonsoft.Json.Linq;
//using RestSharp;
//using RestSharp.Authenticators;
//using System;
//using Microsoft.AspNet.WebUtilities;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
//using System.Security.Claims;

//namespace Canvass.Api.Helpers
//{
//    public class TwitterHelper
//    {
       
//        public static AppSettings AppSettings { get; set; }
//        private static string TwitterApiUri = "https://api.twitter.com/oauth/";
//        private static string TwitterProfileUri = "https://api.twitter.com/1.1/account/";
//        private static string ConsumerKey
//        {
//            get
//            {
//                return AppSettings.TwitterConsumerKey;
//            }
//        }

//        private static string ConsumerSecret
//        {
//            get
//            {
//                return AppSettings.TwitterConsumerSecret;
//            }
//        }

//        private static string CallbackUrl
//        {
//            get
//            {
//                return AppSettings.TwitterCallbackUrl;
//            }
//        }

//        public static async Task<JObject> RequestToken() { 
//            var client = new RestClient(TwitterApiUri);
//            var request = new RestRequest("request_token", Method.POST);
//            client.Authenticator = OAuth1Authenticator.ForRequestToken(ConsumerKey, ConsumerSecret, CallbackUrl);
//            var taskCompletionSource = new TaskCompletionSource<JObject>();
//            var result = client.ExecuteAsync(request, response =>
//            {
//                var dict = QueryHelpers.ParseQuery(response.Content);
//                var obj = JObject.FromObject(dict);
//                taskCompletionSource.SetResult(obj);
//            });
//           return await taskCompletionSource.Task;
//        }

//        public static async Task<JObject> AccessToken(string oauthToken, string oauthVerifier)
//        {
//            var client = new RestClient(TwitterApiUri);
//            var request = new RestRequest("access_token", Method.POST);
//            client.Authenticator = OAuth1Authenticator.ForAccessToken(ConsumerKey, ConsumerSecret, oauthToken, string.Empty, oauthVerifier);
     
//            var taskCompletionSource = new TaskCompletionSource<JObject>();
//            var result = client.ExecuteAsync(request, response =>
//            {
//                var dict = QueryHelpers.ParseQuery(response.Content);
//                var obj = JObject.FromObject(dict);
//                taskCompletionSource.SetResult(obj);
//            });
//            return await taskCompletionSource.Task;
//        }

//        public static async Task<ExternalLoginDetails> GetTwitterUserInfo(JObject oauthData)
//        {
//            var screenName = (string)oauthData["screen_name"];
//            var accessToken = (string)oauthData["oauth_token"];
//            var tokenSecret = (string)oauthData["oauth_token_secret"];
//            var client = new RestClient(TwitterProfileUri);
//            var request = new RestRequest("verify_credentials.json", Method.GET);
//            request.AddQueryParameter("include_email", "true");
//            client.Authenticator = OAuth1Authenticator.ForProtectedResource(ConsumerKey, ConsumerSecret, accessToken, tokenSecret);

//            var taskCompletionSource = new TaskCompletionSource<ExternalLoginDetails>();
//            var result = client.ExecuteAsync(request, response =>
//            {
//                try {
//                    var obj = JObject.Parse(response.Content);
//                    var providerKey = (string)obj["id_str"];
//                    var email = (string)obj["email"];
//                    var userLogin = new UserLoginInfo("twitter", providerKey, email);
//                    var info = new ExternalLoginDetails()
//                    {
//                        Login = userLogin,
//                        Email = email
//                    };
//                    taskCompletionSource.SetResult(info);
//                } catch (Exception ex)
//                {
//                    taskCompletionSource.SetResult(null);
//                }
              
//            });
//            return await taskCompletionSource.Task;
//        }

//        public class TwitterProfile
//        {
//            public string id_str { get; set; }
//            public string email { get; set; }
//        }
//    }
//}