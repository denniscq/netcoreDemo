using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StaticFiles
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
            //services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.MapWhen(httpContext => !httpContext.Request.Path.Value.StartsWith("/api"), applicationBuilder =>
            {
                var options = new RewriteOptions();
                options.AddRewrite(".*", "/index.html", true);
                applicationBuilder.UseRewriter(options);

                applicationBuilder.UseStaticFiles();
            });
            IFileProvider
            //设置额外的文件路径规则, 默认的文件路径是wwwroot
            //app.UseStaticFiles(new StaticFileOptions{ 
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "file")),
            //    RequestPath = "/files"
            //});

            //设置开启所有文件中间层
            //app.UseFileServer();

            //设置文件夹虚拟路径显示
            //app.UseDirectoryBrowser();

            //设置默认文件匹配规则index.html index.cshtml
            //app.UseDefaultFiles();

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
