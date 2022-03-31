using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace yingwebappdemo
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
            services.AddRazorPages();
//            services.Configure<HostOptions>(o => o.ShutdownTimeout = TimeSpan.FromSeconds(60));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostApplicationLifetime hostApplicationLifetime, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            hostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine($"{DateTime.UtcNow} : Services started");
            });

            hostApplicationLifetime.ApplicationStopping.Register(() =>
            {
                Console.WriteLine($"{DateTime.UtcNow} : WebApp: ApplicationStopping handler is called");
                // this will stop the service to polling tasks any more.
                //Program.cancellationTokenSource.Cancel();
                //Program.shutdown();
                //Console.WriteLine($"{DateTime.UtcNow} : WebApp: Cancelled task polling");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
