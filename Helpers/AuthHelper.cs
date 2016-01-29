//using Newtonsoft.Json;
//using RestSharp;
//using System;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Newtonsoft.Json.Linq;
//using System.Net;
//using Microsoft.AspNet.Mvc;
//using Canvass.Api.Models;
//using Microsoft.Owin.Security.OAuth;
//using Microsoft.AspNet.Http.Authentication;
//using Microsoft.AspNet.Authentication;

//namespace Canvass.Api.Helpers
//{
  
//    public class AuthHelper
//    {
//        public AppSettings AppSettings { get; set; }
//        public static async Task<AccessTokenResponse> Login(IUrlHelper url, string username, string password)
//        {
//            var endpoint = url.Content("~/");
//            var client = new RestClient(endpoint);
//            var request = new RestRequest("token", Method.POST);
//            request.AddParameter("username", username);
//            request.AddParameter("password", password);
//            request.AddParameter("grant_type", "password");
//            request.AddHeader("Accept-Content", "x-www-form-urlencoded");
//            var taskCompletionSource = new TaskCompletionSource<AccessTokenResponse>();
//            var result = client.ExecuteAsync(request, response =>
//            {
//                var json = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content);
//                json.success = response.StatusCode == HttpStatusCode.OK;
//                taskCompletionSource.SetResult(json);
//            });
//            return await taskCompletionSource.Task;
//        }

//        //public static JObject GenerateLocalAccessTokenResponse(string userName)
//        //{

//        //    var tokenExpiration = TimeSpan.FromDays(1);

//        //    var identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

//        //    identity.AddClaim(new Claim(ClaimTypes.Name, userName));
//        //    identity.AddClaim(new Claim("role", "user"));

//        //    var props = new AuthenticationProperties()
//        //    {
//        //        IssuedUtc = DateTime.UtcNow,
//        //        ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
//        //    };

//        //    var ticket = new AuthenticationTicket(identity, props);

//        //    var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

//        //    var tokenResponse = new JObject(
//        //                                new JProperty("userName", userName),
//        //                                new JProperty("access_token", accessToken),
//        //                                new JProperty("token_type", "bearer"),
//        //                                new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
//        //                                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
//        //                                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
//        //    );
//        //    return tokenResponse;
//        //}
//    }

//}