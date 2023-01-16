using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;

namespace WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine(DateTime.Now);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppDomain.CurrentDomain.ProcessExit += ProcessExitHandler;
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    await services.GetRequiredService<IMobileIdStorageService>().Load();
                    await TelegramAPI.Send("Loaded MobileIds");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            await host.RunAsync();
        }

        private static void ProcessExitHandler(object sender, EventArgs e)
        {
            TelegramAPI.Send("Server stopped").Wait();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog((builderContext, config) =>
                {
                    config
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .WriteTo.Seq("http://seq")
                        .ReadFrom.Configuration(builderContext.Configuration)
                        .WriteTo.Console();
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}