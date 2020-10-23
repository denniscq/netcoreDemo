using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using static GrpcServices.OrderGrpc;

namespace grpcClient
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            services.AddGrpcClient<OrderGrpcClient>(options =>
            {
                options.Address = new Uri("https://localhost:5001");
            }).ConfigurePrimaryHttpMessageHandler(provider =>
            {
                var handler = new SocketsHttpHandler();
                handler.SslOptions.RemoteCertificateValidationCallback = (a, b, c, d) => true;
                return handler;
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryForeverAsync(i => TimeSpan.FromSeconds(i * 3)));

            #region Polly 

            //重试 
            var reg = services.AddPolicyRegistry();
            reg.Add("retryforever", Policy.HandleResult<HttpResponseMessage>(message =>
            {
                return message.StatusCode == System.Net.HttpStatusCode.Created;
            }).RetryForeverAsync());
            services.AddHttpClient("orderclient").AddPolicyHandlerFromRegistry("retryforever");
            services.AddHttpClient("orderclientv2").AddPolicyHandlerFromRegistry((registry, message) =>
            {
                return message.Method == HttpMethod.Get ? registry.Get<IAsyncPolicy<HttpResponseMessage>>("retryforever") : Policy.NoOpAsync<HttpResponseMessage>();
            });

            //熔断
            services.AddHttpClient("orderclientv3").AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>().CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 10,
                durationOfBreak: TimeSpan.FromSeconds(10),
                onBreak: (r, t) => { },
                onReset: () => { },
                onHalfOpen: () => { }
            ));

            //高级熔断
            services.AddHttpClient("orderclientv3").AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>().AdvancedCircuitBreakerAsync(
                failureThreshold: 0.8,
                samplingDuration: TimeSpan.FromSeconds(10),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (r, t) => { },
                onReset: () => { },
                onHalfOpen: () => { }
            ));

            #region 组合策略

            //熔断
            var breakPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>().AdvancedCircuitBreakerAsync(
                failureThreshold: 0.8,
                samplingDuration: TimeSpan.FromSeconds(10),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (r, t) => { },
                onReset: () => { },
                onHalfOpen: () => { });
            
            //降级
            var message = new HttpResponseMessage { Content = new StringContent("{}") };
            var fallback = Policy<HttpResponseMessage>.Handle<BrokenCircuitException>().FallbackAsync(message);

            var retry = Policy<HttpResponseMessage>.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(1));

            var fallbackBreak = Policy.WrapAsync(fallback, retry, breakPolicy);
            services.AddHttpClient("httpv3").AddPolicyHandler(fallbackBreak);

            //限流
            var bulk = Policy.BulkheadAsync<HttpResponseMessage>(
                maxParallelization: 30,
                maxQueuingActions: 20,
                onBulkheadRejectedAsync: context => Task.CompletedTask);
            var message2 = new HttpResponseMessage()
            {
                Content = new StringContent("{}")
            };
            var fallback2 = Policy<HttpResponseMessage>.Handle<BulkheadRejectedException>().FallbackAsync(message);
            var fallbackbulk = Policy.WrapAsync(fallback2, bulk);
            services.AddHttpClient("httpv4").AddPolicyHandler(fallbackbulk);

            #endregion


            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    OrderGrpcClient service = context.RequestServices.GetService<OrderGrpcClient>();
                    try
                    {
                        var reuslt = service.CreateOrder(new GrpcServices.CreateOrderCommand
                        {
                            BuyerId = "test"
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
