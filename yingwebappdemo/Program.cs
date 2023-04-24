using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace yingwebappdemo
{
    public class Program
    {

        public static ServiceCancellationTokenSource cancellationTokenSource = new ServiceCancellationTokenSource(900, 60, 5);


        public static async Task Main(string[] args)
        {
            try
            {
                WebApplicationBuilder appBuilder = WebApplication.CreateBuilder(args);
                appBuilder.Host.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromSeconds(60)); //valid in .net6
                appBuilder.Services.AddControllers();

                WebApplication app = appBuilder.Build();

                var lifetime = app.Services.GetService<IHostApplicationLifetime>();

                lifetime.ApplicationStarted.Register(() =>
                {
                    Console.WriteLine($"{DateTime.UtcNow} Application started");
                });

                lifetime.ApplicationStopping.Register(() =>
                {
                    Console.WriteLine($"{DateTime.UtcNow} Application stopping");
                    //Thread.Sleep(120 * 1000);
                    cancellationTokenSource.Cancel();
                });

                lifetime.ApplicationStopped.Register(() =>
                {
                    Console.WriteLine($"{DateTime.UtcNow} Application stopped");
                });
                app.UseRouting();
                app.MapControllers();
                app.RunAsync();

                await pollingTasks(cancellationTokenSource);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Thrown exception: {e}");
            }

        }

        public static void shutdown()
        {
            Console.WriteLine($"{DateTime.UtcNow} : Main-Shutdown: Handling the in-progressing tasks");
            if (cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.WaitAndExit();
            }
            Console.WriteLine($"{DateTime.UtcNow} : Main-Shutdown: Finished the in-progressing tasks");
            System.Threading.Thread.Sleep(60 * 1000); //keep the same shutdown timeout as web host
            Console.WriteLine($"{DateTime.UtcNow} : Main-Shutdown: Completely shutdown");
        }


        public async static Task pollingTasks(ServiceCancellationTokenSource cancellationTokenSource)
        {

            int count = 0;
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine($"{DateTime.UtcNow}: pollingTasks: task {count} started");
                cancellationTokenSource.IncrementExecutionCount();
                await Task.Delay(10 * 1000); //10s
                cancellationTokenSource.DecrementExecutionCount();
                Console.WriteLine($"{DateTime.UtcNow}: pollingTasks: task {count} ended");
                count++;
            }
            Console.WriteLine($"{DateTime.UtcNow}: pollingTasks: stop task polling when the process is cancelled.");
        }

    }
}
