//using AspNet.Security.OpenIdConnect.Server;
//using Canvass.Api.Contexts;
//using Canvass.Api.Helpers;
//using Canvass.Api.Managers;
//using Canvass.Api.Models;
//using Microsoft.AspNet.Authentication;
//using Microsoft.AspNet.Http.Authentication;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Canvass.Api.Providers
//{
//    public sealed class AuthorizationProvider : OpenIdConnectServerProvider
//    {
//        public override Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
//        {
//            string clientId = string.Empty;
//            string clientSecret = string.Empty;
//            Client client = null;


//            //using (AuthRepository _repo = new AuthRepository())
//            //{
//            //    client = _repo.FindClient(context.ClientId);
//            //}

//            //if (client == null)
//            //{
//            //    context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
//            //    return Task.FromResult<object>(null);
//            //}

//            //if (client.ApplicationType == ApplicationTypes.NativeConfidential)
//            //{
//            //    if (string.IsNullOrWhiteSpace(clientSecret))
//            //    {
//            //        context.SetError("invalid_clientId", "Client secret should be sent.");
//            //        return Task.FromResult<object>(null);
//            //    }
//            //    else
//            //    {
//            //        if (client.Secret != HashHelper.GetHash(clientSecret))
//            //        {
//            //            context.SetError("invalid_clientId", "Client secret is invalid.");
//            //            return Task.FromResult<object>(null);
//            //        }
//            //    }
//            //}

//            //if (!client.Active)
//            //{
//            //    context.SetError("invalid_clientId", "Client is inactive.");
//            //    return Task.FromResult<object>(null);
//            //}

//            //context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
//            //context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

//            //context.Validated();
//            //return Task.FromResult<object>(null);
//            context.Skipped();

//            return Task.FromResult(0);
//        }

//        public override async Task GrantResourceOwnerCredentials(GrantResourceOwnerCredentialsContext context)
//        {
//            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

//            var userManager = context.HttpContext.RequestServices.GetService<ApplicationUserManager>();

//            var user = await userManager.FindByEmailAsync(context.UserName);
//            if(user==null)
//            {
//                context.Rejected("invalid_grant", "The user name or password is incorrect.");
//                return;
//            }

//            var password = await userManager.CheckPasswordAsync(user, context.Password);
//            if(!password)
//            {
//                context.Rejected("invalid_grant", "The user name or password is incorrect.");
//                return;
//            }

            
//            var identity = new ClaimsIdentity();
//            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

//            var props = new AuthenticationProperties(new Dictionary<string, string>
//                {
//                    {
//                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
//                    },
//                    {
//                        "userName", context.UserName
//                    }
//                });

//            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), new AuthenticationProperties(), context.Options.AuthenticationScheme);
//            context.Validated(ticket);
//            return;
//        }
//    }
//}
