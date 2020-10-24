﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace WebApp
{
    public class Startup
    {
        // TODO 0: Solved
        //Something broken in ConfigureServices.
        // Don't focus attention on it right now, you will faced with problem in process of meeting the challenges TODOs
        // Pay attention to different possible solutions of the problem

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAccountDatabase, AccountDatabaseStub>();
            services.AddSingleton<IAccountCache, AccountCache>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddDistributedMemoryCache();
            services.AddMvc(options=>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddAuthentication("Cookie").AddCookie("Cookie", options => {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "Admin");
                });
                options.AddPolicy("user", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "user");
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();  
            app.UseAuthorization();
            app.UseMvc();
        }
    }
}