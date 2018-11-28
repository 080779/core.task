using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Filters;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var serviceAsm = Assembly.Load(new AssemblyName("Service"));
            foreach (Type serviceType in serviceAsm.GetTypes()
            .Where(t => typeof(IServiceSupport).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract))
            {
                var interfaceTypes = serviceType.GetInterfaces();
                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddSingleton(interfaceType, serviceType);
                }
            }
            //services.AddTransient<IRazorViewEngine>();
            //services.AddSingleton(typeof(ITempDataProvider));

            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(WebAuthorizationFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "admin",
                    template: "Admin/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "Default",
                    template: "Api/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
