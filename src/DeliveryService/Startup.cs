
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
using DeliveryService.Services.Config;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using DeliveryService.Util;

namespace DeliveryService
{
    /*
     * This class starts up the entire framework and configures the middleware
     */ 
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
                options.UseOpenIddict();
            },
                ServiceLifetime.Scoped);
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

            services.AddOpenIddict()
                // Register the Entity Framework stores.
                .AddEntityFrameworkCoreStores<ApplicationDbContext>()

                // Register the ASP.NET Core MVC binder used by OpenIddict
                .AddMvcBinders()

                // Enable the token endpoint (required to use the password flow).
                .EnableTokenEndpoint("/connect/token")

                // Allow client applications to use the grant_type=password flow.
                .AllowPasswordFlow()
                .DisableHttpsRequirement();

        services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddMvc()
                 .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;


            //Add defined service classes
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<IDirectionsService, DirectionsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGoogleMapsUtil, GoogleMapsUtil>();
            services.AddTransient<INotificationService, NotificationService>();
            services.Configure<AppProperties>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<DeliveryStatusUpdateService>();
            services.AddSingleton<LocationService>();
            services.AddSingleton<RouteCreationService>();
            services.AddSingleton<DriverAssignmentService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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
            

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), branch => {
                // Add a middleware used to validate access
                // tokens and protect the API endpoints.
                branch.UseOAuthValidation();
            });
            
            app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api"), branch => {
                branch.UseStatusCodePagesWithReExecute("/error");

                branch.UseIdentity();
            });
            
            app.UseOpenIddict();

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
