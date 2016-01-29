
using System.Linq;
using CryptoHelper;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.HttpOverrides;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NWebsec.Middleware;
using OpenIddict;
using OpenIddict.Models;
using Canvass.Api.Contexts;
using Canvass.Api.Models;
using Canvass.Api.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Authentication.Facebook;
using Canvass.Api.Helpers;
using Microsoft.AspNet.IISPlatformHandler;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.Twitter;

namespace Canvass.Api
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var application = new WebApplicationBuilder()
                .UseConfiguration(WebApplicationConfiguration.GetDefault(args))
                .UseStartup<Startup>()
                .Build();

            application.Run();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

           // services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
      
            services.AddMvc();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddOpenIddict();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
       //     services.AddSingleton<IQueueManager, QueueManager>();
        }

        public void Configure(IApplicationBuilder app)
        {
            var factory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            factory.AddConsole();
            factory.AddDebug();

            app.UseIISPlatformHandler(options => {
                options.FlowWindowsAuthentication = false;
            });

            app.UseOverrideHeaders(options => {
                options.ForwardedOptions = ForwardedHeaders.All;
            });
            app.UseStaticFiles();

            // Add a middleware used to validate access
            // tokens and protect the API endpoints.
            app.UseOAuthValidation();
            
            // Alternatively, you can also use the introspection middleware.
            // Using it is recommended if your resource server is in a
            // different application/separated from the authorization server.
            // 
            // app.UseOAuthIntrospection(options => {
            //     options.AutomaticAuthenticate = true;
            //     options.AutomaticChallenge = true;
            //     options.Authority = "http://localhost:54540/";
            //     options.Audience = "resource_server";
            //     options.ClientId = "resource_server";
            //     options.ClientSecret = "875sqd4s5d748z78z7ds1ff8zz8814ff88ed8ea4z4zzd";
            // });

            app.UseIdentity();
            //var settings = app.ApplicationServices.GetService<AppSettings>();
            //app.UseGoogleAuthentication(new GoogleOptions() {
            //    ClientId = settings.OAuth.Google.ClientId,
            //    ClientSecret = settings.OAuth.Google.ClientSecret
            //});

            //app.UseTwitterAuthentication(new TwitterOptions()
            //{
            //    ConsumerKey = settings.OAuth.Twitter.ClientId,
            //    ConsumerSecret = settings.OAuth.Twitter.ClientSecret
            //});
            
            // Note: OpenIddict must be added after
            // ASP.NET Identity and the external providers.
            app.UseOpenIddict(options =>
            {
                // You can customize the default Content Security Policy (CSP) by calling UseNWebsec explicitly.
                // This can be useful to allow your HTML views to reference remote scripts/images/styles.
                options.UseNWebsec(directives =>
                {
                    directives.DefaultSources(directive => directive.Self())
                        .ImageSources(directive => directive.Self().CustomSources("*"))
                        .ScriptSources(directive => directive
                            .Self()
                            .UnsafeInline()
                            .CustomSources("https://my.custom.url"))
                        .StyleSources(directive => directive.Self().UnsafeInline());
                });
            });

            app.UseMvcWithDefaultRoute();
          //  CreateClients(app);
       
        }

        private void CreateClients(IApplicationBuilder app)
        {
            using (var context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>())
            {
                context.Database.EnsureCreated();

                if (!context.Applications.Any())
                {
                    context.Applications.Add(new Application
                    {
                        Id = "myClient",
                        DisplayName = "My client application",
                        RedirectUri = "http://localhost:53507/signin-oidc",
                        LogoutRedirectUri = "http://localhost:53507/",
                        Secret = Crypto.HashPassword("secret_secret_secret"),
                        Type = OpenIddictConstants.ApplicationTypes.Confidential
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
