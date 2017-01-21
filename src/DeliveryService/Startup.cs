using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DeliveryService.Data;
using DeliveryService.Models;
using DeliveryService.Services;
using DeliveryService.Data.Initializer;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Services.Config;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace DeliveryService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }


            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.

            var env = services.BuildServiceProvider().GetRequiredService<IHostingEnvironment>();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            },
                ServiceLifetime.Scoped);
            var jwtSigningCert = new X509Certificate2("C:\\Users\\Olga\\Documents\\Visual Studio 2015\\Projects\\my-cert-file.pfx", "password");

      /*      services.AddOpenIddict<ApplicationDbContext>()
                .AddMvcBinders()
                .EnableTokenEndpoint("/connect/token")
                .UseJsonWebTokens()
                .AllowPasswordFlow()
                .AddSigningCertificate(jwtSigningCert)
                .DisableHttpsRequirement(); */


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
               {
                   options.Password.RequireDigit = true;
                   options.Password.RequiredLength = 6;
                   options.Password.RequireLowercase = false;
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequireUppercase = false;
               })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();


            services.AddOpenIddict<ApplicationDbContext>()

               // Register the ASP.NET Core MVC binder used by OpenIddict.
               // Note: if you don't call this method, you won't be able to
               // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
               .AddMvcBinders()

               // Enable the token endpoint.
               .EnableTokenEndpoint("/connect/token")

               // Enable the password flow.
               .AllowPasswordFlow()

               // During development, you can disable the HTTPS requirement.
               .DisableHttpsRequirement()

               // Register a new ephemeral key, that is discarded when the application
               // shuts down. Tokens signed using this key are automatically invalidated.
               // This method should only be used during development.
               .AddEphemeralSigningKey();



            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMvc()
                 .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;


            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IDirectionsService, DirectionsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGoogleMapsUtil, GoogleMapsUtil>();
            services.AddTransient<INotificationService, NotificationService>();


            /*  services.Configure<MvcOptions>(options =>
              {
                  options.Filters.Add(new RequireHttpsAttribute());
              });*/


            services.Configure<AppProperties>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<DeliveryStatusUpdateService>();
            services.AddSingleton<DeliverySearchService>();
            services.AddSingleton<LocationService>();
            services.AddSingleton<RouteCreationService>();
            services.AddSingleton<DriverAssignmentService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

            }

            app.UseStaticFiles();


            app.UseOAuthValidation();




            app.UseIdentity();
            app.UseOpenIddict();

         /*   app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                Audience = "http://localhost:44302/",
                Authority = "http://localhost:44302/"
            });*/


            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseStatusCodePagesWithRedirects("Account/AccessDenied");

            DatabaseInitializer.Initialize(app.ApplicationServices);
        }

    }
}
