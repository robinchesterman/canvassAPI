using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Canvass.Api.Models;
using OpenIddict;
using OpenIddict.Models;

namespace Canvass.Api.Contexts
{
    public class ApplicationDbContext : OpenIddictContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
    
}
