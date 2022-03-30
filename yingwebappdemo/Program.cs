using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yingwebappdemo
{
    public class Program
    {

        public static ServiceCancellationTokenSource cancellationTokenSource = new ServiceCancellationTokenSource(900, 60, 5);

        private static void ProcessExit(object? sender, EventArgs eventArgs)
        {
            Console.WriteLine($"Process exit triggered");
            cancellationTokenSource.Cancel();
            shutdown();
        }

        private static void ControlCHandler(object? sender, EventArgs eventArgs)
        {
            Console.WriteLine($"Ctrl+C triggered");
            cancellationTokenSource.Cancel();
            shutdown();
        }

        public static async Task Main(string[] args)
        {

            try
            {
                AppDomain.CurrentDomain.ProcessExit += ProcessExit;
                Console.CancelKeyPress += ControlCHandler;

                CreateHostBuilder(args).Build().RunAsync();
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromMilliseconds(100))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
