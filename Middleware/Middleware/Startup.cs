using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Middleware.Middleware;

namespace Middleware
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (context, next) =>
            //{
            //    //await context.Response.WriteAsync("Hello");
            //    await next();
            //    await context.Response.WriteAsync("Hello1");
            //});

            //app.Map("/abc", applicationBuilder =>
            //{
            //    applicationBuilder.Use(async (context, next) =>
            //    {
            //        await next();
            //        await context.Response.WriteAsync("Hello2");
            //    });
            //});

            //app.MapWhen(context =>
            //{
            //    return context.Request.Query.Keys.Contains("abc");
            //}, applicationBuilder => {
            //    applicationBuilder.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("Hello3");
            //    });
            //});

            app.UseMyRequestMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public delegate Task myDelegate(int count);

    public class myApplicationBuilder
    {
        private IList<Func<myDelegate, myDelegate>> delegateList = new List<Func<myDelegate, myDelegate>>();

        public myApplicationBuilder Use(Func<myDelegate, myDelegate> expression)
        {
            this.delegateList.Add(expression);
            return this;
        }

        public myDelegate Build()
        {
            myDelegate myDelegate = (count) =>
            {
                Console.WriteLine("---- start build ----");
                return Task.CompletedTask;
            };

            foreach (var item in delegateList.Reverse())
            {
                myDelegate = item(myDelegate);
            }

            return myDelegate;
        }
    }

    public static class myApplicationBuilderExtension
    {
        public static myApplicationBuilder UseExtension(this myApplicationBuilder myApp, Func<int, Func<Task>, Task> middleware)
        {
            return myApp.Use((next) =>
            {
                return (count) =>
                {
                    Func<Task> simpleNext = () => next(count);
                    return middleware(count, simpleNext);
                };
            });
        }
    }
}
