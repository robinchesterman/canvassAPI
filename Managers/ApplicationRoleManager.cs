using Canvass.Api.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Canvass.Api.Managers
{
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole> store, IEnumerable<IRoleValidator<IdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IOptions<IdentityOptions> optionsAccessor,
              IdentityErrorDescriber errors, ILogger<RoleManager<IdentityRole>> logger, IHttpContextAccessor contextAccessor)
            : base(
                store, roleValidators, keyNormalizer, errors, logger, contextAccessor)
        {
        }

    }
}