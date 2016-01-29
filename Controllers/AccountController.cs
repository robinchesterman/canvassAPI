using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using Canvass.Api.Models;
using Canvass.Api.Services;
using Canvass.Api.ViewModels.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using Canvass.Api.Helpers;
using Canvass.Api.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Votify.Api.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {

        UserManager<ApplicationUser> _userManager;
        IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn(SigninModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }
            try {
                AccessTokenResponse response = new AccessTokenResponse();// await AuthHelper.Login(Url, credentials.Email, credentials.Password);
                if (!response.success)
                {
                    return GenerateErrorResult(response);
                }
                return Ok(response);
            } catch (Exception ex)
            {
                return GenerateErrorResult("An unknown error occurred");
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            try
            {
                var temp = new AccessTokenResponse
                {
                    access_token = "sdfsdf",
                    userId = "robin"
                };
                return Ok(temp);

                var user = new ApplicationUser
                {
                    UserName = userModel.UserName,
                    Email = userModel.UserName,
                };
                var result = await _userManager.CreateAsync(user, userModel.Password);
                
                
                if(!result.Succeeded)
                {
                    return GenerateErrorResult(result);
                }
                

                await SendEmailConfirmationTokenAsync(user);

                var response = new AccessTokenResponse();// await AuthHelper.Login(Url, userModel.UserName, userModel.Password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unknown error occurred");
                return HttpBadRequest(ModelState);
            }
        }

        private async Task SendEmailConfirmationTokenAsync(ApplicationUser user)
        {
            try {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var subject = "Canvass - Confirm your account";
                var url = Url.Action("Email", "Confirmation", new { userId = user.Id, code = token }, Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email, subject, url);
            } catch (Exception ex)
            {
                var t = ex;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               
            }

            base.Dispose(disposing);
        }

        private IActionResult GenerateErrorResult(AccessTokenResponse response)
        {
            ModelState.AddModelError("", !string.IsNullOrEmpty(response.error_description) ? response.error_description : response.error);
            return HttpBadRequest(ModelState);
        }

        private IActionResult GenerateErrorResult(string errorMessage)
        {
            ModelState.AddModelError("", errorMessage);
            return HttpBadRequest(ModelState);
        }

        private IActionResult GenerateErrorResult(IdentityResult result)
        {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    return GenerateErrorResult("An unknown error occurred");
                }

                return HttpBadRequest(ModelState);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("Twitter")]
        //public async Task<IActionResult> Twitter(TwitterAccessTokenRequest request)
        //{
        //    if (request == null)
        //    {
        //        var response = await TwitterHelper.RequestToken();
        //        return Ok(response);
        //    } else
        //    {
        //        var response = await TwitterHelper.AccessToken(request.oauth_token, request.oauth_verifier);
        //        var userInfo = await TwitterHelper.GetTwitterUserInfo(response);
        //        return await BuildAccessTokenResponse(userInfo);
        //    }
        //}

        public class TwitterAccessTokenRequest
        {
            public string oauth_token {get;set;}
            public string oauth_verifier { get; set; }
        }

        //private OAuth2Helper GetSuitableHelper(LocalAccessTokenRequest request)
        //{
        //    var provider = request.RedirectUri.Replace("https://votify.", string.Empty).Replace("/", string.Empty);
        //    switch(provider)
        //    {
        //        case "google":
        //       //     return GoogleHelper.Instance(request.ClientId);
        //        case "facebook":
        //         //   return FacebookHelper.Instance(request.ClientId);
        //        case "linkedin":
        //           // return LinkedinHelper.Instance(request.ClientId);
        //        default:
        //            return null;
        //    }

        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("ObtainLocalAccessToken")]
        //public async Task<IActionResult> ObtainLocalAccessToken(LocalAccessTokenRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return HttpBadRequest(ModelState);
        //    }
        //    var helper = GetSuitableHelper(request);
        //    var userInfo = await helper.GetExternalLoginDetails(request.AuthorizationCode);
        //    if (userInfo == null)
        //    {
        //        ModelState.AddModelError("", "Could not connect to your account");
        //        return HttpBadRequest(ModelState);
        //    }
        //    if (string.IsNullOrEmpty(userInfo.Email))
        //    {
        //        ModelState.AddModelError("", "There was no email address connected to this account");
        //        return HttpBadRequest(ModelState);
        //    }
        //    return await BuildAccessTokenResponse(userInfo);   
        //}

        private async Task<IActionResult> BuildAccessTokenResponse(ExternalLoginDetails details)
        {
            try { 
                IdentityUser user = await _userManager.FindByLoginAsync(details.Login.LoginProvider, details.Login.ProviderKey);
                if (user == null)
                {
                    var result = await RegisterExternalUser(details);
                    if (!result.Succeeded)
                    {
                        return GenerateErrorResult(result);
                    }
                    user = await _userManager.FindByEmailAsync(user.Email);
                }

                JObject accessTokenResponse = new JObject(); // AuthHelper.GenerateLocalAccessTokenResponse(user.UserName);
                return Ok(accessTokenResponse);
            } catch (Exception ex)
            {
                ModelState.AddModelError("", "Could not connect to your account");
                return HttpBadRequest(ModelState);
            }
        }

        private async Task<IdentityResult> RegisterExternalUser(ExternalLoginDetails details)
        {
            IdentityResult result;

            var user = await _userManager.FindByEmailAsync(details.Email);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = details.Email,
                    Email = details.Email,
                    EmailConfirmed = true,
                    AccessFailedCount = 0,
                };
                result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return result;
                }
            }
            
            result = await _userManager.AddLoginAsync(user, details.Login);
            return result;
        }

    }
}