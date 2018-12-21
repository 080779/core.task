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
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = "localhost";
            //    options.InstanceName = "SampleInstance";
            //});
            services.AddDistributedMemoryCache();
            services.AddSession();

            //自动注入Service继承了IServiceSupport接口的方法
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
            //services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();            

            services.AddMvc(option =>
            {
                option.Filters.Add(typeof(WebAuthorizationFilter));//filter拦截验证,实现接口的形式
                //option.Filters.Add(typeof(WebAuthorizeFilter));//filter拦截验证,重写虚方法的形式
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//json返回时间格式化
            });

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options=> {
            //    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//json返回时间格式化
            //});

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN"); //mvvm模式ajax提交需在header提交XSRF-TOKEN
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
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                //区域中的Controller上要添加标记[Area("admin")],admin表示区域名.
                //core中webapi不一定在区域中新建，可以直接在web项目根目录下Controllers目录新建或新建一个目录中的Controllers目录新建
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
