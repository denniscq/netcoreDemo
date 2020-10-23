using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Dennis.Mobile.ApiAggregator.Services;
using Dennis.Ordering.API.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Dennis.Mobile.ApiAggregator
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
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            services.AddGrpcClient<Ordering.API.Grpc.OrderService.OrderServiceClient>(options =>
            {
                options.Address = new Uri(Configuration.GetValue<string>("ServiceUrls:OrderingAPI"));
            }).ConfigurePrimaryHttpMessageHandler(provider =>
            {
                var handler = new SocketsHttpHandler();
                handler.SslOptions.RemoteCertificateValidationCallback = (a, b, c, d) => true;
                return handler;
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryForeverAsync(i => TimeSpan.FromSeconds(i * 3)));

            services.AddControllers();

            services.AddHealthChecks();
            services.AddHttpClient<IOrderService, Services.OrderService>().ConfigureHttpClient(client => 
            {
                client.BaseAddress = new Uri("https://localhost:5001");
            });
            services.AddScoped<IOrderService, Services.OrderService>();

            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {Title = "my api", Description = "api aggregator", Version = "v1" });
                var xmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlFullName = Path.Combine(AppContext.BaseDirectory, xmlName);

                p.IncludeXmlComments(xmlFullName);
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (this.Configuration.GetValue("USE_Forwarded_Headers", false))
            {
                app.UseForwardedHeaders();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "my api");
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/live");
                endpoints.MapHealthChecks("/ready");
                endpoints.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapControllers();
            });
        }
    }
}
