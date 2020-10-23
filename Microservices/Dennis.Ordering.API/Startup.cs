using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dennis.Ordering.API.Extensions;
using Dennis.Ordering.API.Grpc;
using Dennis.Ordering.API.Grpc.Services;
using Dennis.Ordering.Infrastructure;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Dennis.Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        internal static bool IsReady { get; set; } = true;
        internal static bool IsLive { get; set; } = true;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
                options.Interceptors.Add<GrpcExceptionInterceptor>();
            });
            services.AddHealthChecks()
                .AddMySql(this.Configuration.GetValue<string>("Mysql"), name: "mysql", tags: new string[] { "mysql", "live", "all" })
                .AddRabbitMQ(p =>
                {
                    var connectionFactory = new RabbitMQ.Client.ConnectionFactory();
                    this.Configuration.GetSection("RabbitMQ").Bind(connectionFactory);
                    return connectionFactory;
                }, name: "rabbitmq", tags: new string[] { "rabbitmq", "live", "all" })
                .AddCheck("liveChecker", () =>
                {
                    return IsLive ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
                }, new string[] { "live", "all" })
                .AddCheck("readyChecker", () =>
                {
                    return IsReady ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
                }, new string[] { "ready", "all" });

            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson();
            services.AddSwaggerGen(p =>
            {
                p.SwaggerDoc("v1", new OpenApiInfo { Title = "Dennis", Description = "this is Dennis' microsoft service demo", Version = "v1"});
                var xmlName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlFullPath = Path.Combine(AppContext.BaseDirectory, xmlName);
                p.IncludeXmlComments(xmlFullPath);
            });

            services.AddMediatRService();
            services.AddMysqlDomainContext(this.Configuration.GetValue<string>("Mysql"));
            services.AddRepositories();
            services.AddEventBus(this.Configuration);

            services.Configure<ForwardedHeadersOptions>(options => 
            {
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionless();

            if (Configuration.GetValue("USE_PathBase", false))
            {
                app.Use((context, next) =>
                {
                    context.Request.PathBase = new PathString("/mobile");
                    return next();
                });
            }
            if (Configuration.GetValue("USE_Forwarded_Headers", false))
            {
                app.UseForwardedHeaders();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dc = scope.ServiceProvider.GetService<OrderContext>();
                //var created = dc.Database.EnsureCreated();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("/swagger/v1/swagger.json", "order api");
            });

            app.AddMyMiddleware();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/live", new HealthCheckOptions { Predicate = registration => registration.Tags.Contains("live") });
                endpoints.MapHealthChecks("/ready", new HealthCheckOptions { Predicate = registration => registration.Tags.Contains("ready") });
                endpoints.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapControllers();
                endpoints.MapGrpcService<OrderServiceImp>();
            });
        }
    }
}
