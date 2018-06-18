﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Phonelist
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default1", template: "{page}", defaults: new { controller = "Person", action = "Index" });

                routes.MapRoute(name: "default2", template: "{controller=Person}/{action=Index}/{page?}");
                
                routes.MapRoute(name: "default3", template: "{controller=Person}/{action=Index}/{id?}");

                //routes.MapRoute(name: "default3", template: "{id?}", defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
