using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace SSO.DefaultSite
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDataProtection()
                    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"E:\jointown"))
                    .SetApplicationName("SharedCookieApp");

            services.AddAuthentication("Identity.Application")
                .AddCookie("Identity.Application", options =>
                {
                    options.Cookie.Domain = ".jointown.com";
                    options.Cookie.Name = ".AspNet.SharedCookie";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var options = new RewriteOptions()
                              .AddRedirectToHttps();

            app.UseRewriter(options);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc(route =>
            {
                route.MapRoute("defaultRoute", "", new { controller = "Home", action = "Index" });
            });
            //app.Run(async (context) =>
            //{
            //  // var user=context.Request.
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
