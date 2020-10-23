using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpClientFactory.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HttpClientFactory
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
            services.AddHttpClient();
            services.AddScoped<OrderServiceClient>();
            services.AddSingleton<RequestDelegateHandler>();

            services.AddHttpClient("NamedOrderService", client =>
            {
                client.DefaultRequestHeaders.Add("chen1", "qiang1");
                client.BaseAddress = new Uri("https://localhost:5003");
            }).AddHttpMessageHandler(provider => provider.GetService<RequestDelegateHandler>())
            .SetHandlerLifetime(TimeSpan.FromMinutes(20));
            services.AddScoped<NamedOrderService>();

            services.AddHttpClient<TypedOrderService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5003");
            }).AddHttpMessageHandler(provider => provider.GetService<RequestDelegateHandler>())
            .SetHandlerLifetime(TimeSpan.FromMinutes(20));


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
